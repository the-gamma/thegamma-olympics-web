#if INTERACTIVE
#I "../packages"
#r "Suave/lib/net40/Suave.dll"
#r "DotLiquid/lib/NET45/DotLiquid.dll"
#r "Suave.DotLiquid/lib/net40/Suave.DotLiquid.dll"
#r "FSharp.Formatting/lib/net40/FSharp.Markdown.dll"
#r "Newtonsoft.Json/lib/net40/Newtonsoft.Json.dll"
#else
module OlympicsWeb
#endif

open Suave
open System
open Suave.Filters
open Suave.Operators
open FSharp.Markdown
open Newtonsoft.Json

let (</>) a b = IO.Path.Combine(a, b)

let asm, debug = 
  if System.Reflection.Assembly.GetExecutingAssembly().IsDynamic then __SOURCE_DIRECTORY__, true
  else IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), false
let root = IO.Path.GetFullPath(asm </> ".." </> "web")
let templ = IO.Path.GetFullPath(asm </> ".." </> "templates")

// --------------------------------------------------------------------------------------
// Loading content
// --------------------------------------------------------------------------------------

type Article = 
  { id : string
    index : int
    heading : string
    category : string
    before : string
    code : string
    compiled : string
    after : string
    author : string
    twitter : string
    plaintext : bool }

type Category = 
  { mainArticle : Article
    moreArticles : seq<Article> }

type Home =
  { debug : bool
    categories : seq<Category> }

type Main = 
  { debug : bool 
    mainArticle : Article
    moreArticles : seq<Article> }

let docs = 
  [ "timeline", "countries-timeline" 
    "athlete", "medals-per-athlete"
    "country", "top-5-countries"
    "country", "long-distance-medals"
    "country", "medals-per-country"
    "phelps", "phelps-as-country"
    "phelps", "athlete-drill-down"    
    "phelps", "athlete-break-down"    
    "data", "about-the-data" ]
    
let split pars =
  let rec before head acc = function 
    | MarkdownParagraph.CodeBlock(code, _, _)::MarkdownParagraph.CodeBlock(compiled, _, _)::ps -> head, List.rev acc, (code, compiled), ps
    | MarkdownParagraph.CodeBlock(code, _, _)::ps -> head, List.rev acc, (code, ""), ps
    | MarkdownParagraph.HorizontalRule _::ps -> head, List.rev acc, ("", ""), ps
    | p::ps -> before head (p::acc) ps
    | [] -> head, List.rev acc, ("", ""), []
  match pars with 
  | MarkdownParagraph.Heading(_, [MarkdownSpan.Literal head])::ps -> before head [] ps
  | _ -> failwith "Invalid document: No heading fond"
  
let readArticle i (category, id) = 
  let file = root </> "../docs" </> id + ".md"
  let doc = Markdown.Parse(IO.File.ReadAllText(file))
  let head, before, (code, compiled), after = split doc.Paragraphs
  let format pars = Markdown.WriteHtml(MarkdownDocument(pars, doc.DefinedLinks))
  { id = id; category = category; code = code; index = i; author = ""; twitter = ""
    plaintext = String.IsNullOrEmpty code; compiled = compiled;
    heading = head; before = format before; after = format after }

let loaded = 
  docs 
  |> Seq.mapi readArticle
  |> (if debug then id else Array.ofSeq >> Seq.ofArray)

let categories = 
  loaded
  |> Seq.groupBy (fun a -> a.category)
  |> Seq.map (fun (_, arts) -> 
      { Category.mainArticle = Seq.head arts
        moreArticles = Seq.tail arts })
  |> (if debug then id else Array.ofSeq >> Seq.ofArray)

let loadPage first =
  let first = loaded |> Seq.tryFind (fun a -> a.id = first)
  match first with 
  | Some(first) ->
      let others = 
          loaded 
          |> Seq.filter (fun a -> a.id <> first.id) 
          |> Seq.sortBy (fun a -> a.category <> first.category, a.index)
      { debug = debug; mainArticle = first; moreArticles = others }
  | _ ->
      let main = Seq.head loaded
      let others = loaded |> Seq.tail
      { debug = debug; mainArticle = main; moreArticles = others }
  
let loadHome =
  { debug = debug; categories = categories }

// --------------------------------------------------------------------------------------
// Snippets shared by users
// --------------------------------------------------------------------------------------

type Snippet = 
  { id : int
    likes : int
    posted : DateTime
    title : string
    description : string
    author : string
    twitter : string
    compiled : string
    code : string }

let serializer = JsonSerializer.Create()

let fromJson<'R> str : 'R = 
  use tr = new System.IO.StringReader(str)
  serializer.Deserialize(tr, typeof<'R>) :?> 'R

let titleToUrl (s:string) = 
  let mutable lastDash = false
  let chars = 
    [| for c in s.ToLower() do
        if (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') then lastDash <- false; yield c
        elif not lastDash then lastDash <- true; yield '-' |]
  String(chars).Trim('-')

let loadShared json idOpt =
  let snips = fromJson<Snippet[]> json
  let sorted = 
    snips 
    |> Seq.sortByDescending (fun snip -> 
        Some snip.id = idOpt, snip.likes)
    |> Seq.mapi (fun i snip ->
        let info = Markdown.Parse(snip.description)
        let pars = info.Paragraphs |> List.filter (function MarkdownParagraph.InlineBlock _ -> false | _ -> true)
        let info = Markdown.WriteHtml(MarkdownDocument(pars, info.DefinedLinks))
        let outid = sprintf "outshared-%d-%s" snip.id (titleToUrl snip.title)
        { id = sprintf "shared/%d/%s" snip.id (titleToUrl snip.title); 
          index = i; heading = snip.title; category = "shared"; compiled = snip.compiled.Replace("output-id-placeholder", outid);
          author = snip.author; twitter = snip.twitter.TrimStart('@');
          before = info; code = snip.code; after = ""; plaintext = false })

  let first = sorted |> Seq.head
  let others = sorted |> Seq.tail
  { debug = debug; mainArticle = first; moreArticles = others }

// --------------------------------------------------------------------------------------
// Server
// --------------------------------------------------------------------------------------

let handleShared idOpt ctx = async {
  use wc = new Net.WebClient()
  let! json = wc.AsyncDownloadString(Uri("http://thegamma-snippets.azurewebsites.net/olympics"))
  return! DotLiquid.page "main.html" (loadShared json idOpt) ctx }

let docPath f = pathScan "/%s" (fun s ctx -> 
  if loaded |> Seq.exists (fun a -> a.id = s) then f s ctx
  else async.Return None)

module Filters = 
  let idEncode (id:string) = 
    id.Replace('/', '-')
  let urlEncode (url:string) =
    System.Web.HttpUtility.UrlEncode(url)
  let mailEncode (url:string) =
    urlEncode(url).Replace("+", "%20")

let inits = Lazy.Create(fun () ->
  DotLiquid.setTemplatesDir templ 
  DotLiquid.setCSharpNamingConvention()
  System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
  |> Seq.find (fun ty -> ty.Name = "Filters")
  |> DotLiquid.registerFiltersByType )  
   
let app = request (fun _ ->
  inits.Value
  choose [
    path "/" >=> DotLiquid.page "home.html" (loadHome)
    docPath (fun id -> DotLiquid.page "main.html" (loadPage id))
    pathScan "/shared/%d/%s" (fun (id, _) -> handleShared (Some id)) 
    path "/shared" >=> request (fun _ -> handleShared None)
    Files.browse root ])

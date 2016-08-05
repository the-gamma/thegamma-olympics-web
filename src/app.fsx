#if INTERACTIVE
#I "../packages"
#r "Suave/lib/net40/Suave.dll"
#r "DotLiquid/lib/NET45/DotLiquid.dll"
#r "Suave.DotLiquid/lib/net40/Suave.DotLiquid.dll"
#r "FSharp.Formatting/lib/net40/FSharp.Markdown.dll"
#else
module OlympicsWeb
#endif

open Suave
open System
open Suave.Filters
open Suave.Operators
open FSharp.Markdown

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
    after : string
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
  [ "athlete", "medals-per-athlete"
    "athlete", "athlete-drill-down"    
    "athlete", "phelps-as-country"
    "country", "top-5-countries"
    "country", "medals-per-country"
    ///"country", "countries-timeline" 
    "data", "about-the-data"
    ]
    
let split pars =
  let rec after head before code acc = function
    | MarkdownParagraph.HorizontalRule _::ps -> head, before, code, List.rev acc, ps
    | p::ps -> after head before code (p::acc) ps
    | [] -> head, before, code, List.rev acc, []
  let rec before head acc = function 
    | MarkdownParagraph.CodeBlock(code, _, _)::ps -> after head (List.rev acc) code [] ps
    | MarkdownParagraph.HorizontalRule _::ps -> head, List.rev acc, null, ps, []
    | p::ps -> before head (p::acc) ps
    | [] -> head, (List.rev acc), null, [], []
  match pars with 
  | MarkdownParagraph.Heading(_, [MarkdownSpan.Literal head])::ps -> before head [] ps
  | _ -> failwith "Invalid document: No heading fond"
  
let readArticle i (category, id) = 
  let file = root </> "../docs" </> id + ".md"
  let doc = Markdown.Parse(IO.File.ReadAllText(file))
  let head, before, code, after, alts = split doc.Paragraphs
  let format pars = Markdown.WriteHtml(MarkdownDocument(pars, doc.DefinedLinks))
  { id = id; category = category; code = code; index = i;
    plaintext = String.IsNullOrEmpty code
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

let docPath f = pathScan "/%s" (fun s ctx -> 
  if loaded |> Seq.exists (fun a -> a.id = s) then f s ctx
  else async.Return None)

module Filters = 
  let urlEncode (url:string) =
    System.Web.HttpUtility.UrlEncode(url)
  let mailEncode (url:string) =
    urlEncode(url).Replace("+", "%20")

let inits = Lazy.Create(fun () ->
  DotLiquid.setTemplatesDir templ 
  DotLiquid.setCSharpNamingConvention()
  System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
  |> Seq.find (fun ty -> ty.Name = "Filters")
  |> DotLiquid.registerFiltersByType
)  
   
let app = request (fun _ ->
  inits.Value
  choose [
    path "/" >=> DotLiquid.page "home.html" (loadHome)
    docPath (fun id -> DotLiquid.page "main.html" (loadPage id))
    Files.browse root ])

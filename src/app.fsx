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
DotLiquid.setCSharpNamingConvention()
DotLiquid.setTemplatesDir root 

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
    after : string }

type Content = 
  { debug : bool 
    mainArticle : Article
    moreArticles : seq<Article> }

let docs = 
  [ "athlete", "medals-per-athlete"
    "athlete", "athlete-drill-down"    
    "athlete", "phelps-as-country"
    "country", "top-5-countries"
    "country", "medals-per-country"
    //"country", "countries-timeline" 
    ]

let split pars =
  let rec after head before code acc = function
    | MarkdownParagraph.HorizontalRule _::ps -> head, before, code, List.rev acc, ps
    | p::ps -> after head before code (p::acc) ps
    | [] -> head, before, code, List.rev acc, []
  let rec before head acc = function 
    | MarkdownParagraph.CodeBlock(code, _, _)::ps -> after head (List.rev acc) code [] ps
    | p::ps -> before head (p::acc) ps
    | [] -> failwith "Invalid document: No code block found"
  match pars with 
  | MarkdownParagraph.Heading(_, [MarkdownSpan.Literal head])::ps -> before head [] ps
  | _ -> failwith "Invalid document: No heading fond"
  
let readArticle i (category, id) = 
  let file = root </> "../docs" </> id + ".md"
  let doc = Markdown.Parse(IO.File.ReadAllText(file))
  let head, before, code, after, alts = split doc.Paragraphs
  let format pars = Markdown.WriteHtml(MarkdownDocument(pars, doc.DefinedLinks))
  { id = id; category = category; code = code; index = i;
    heading = head; before = format before; after = format after }

let loaded = 
  docs 
  |> Seq.mapi readArticle
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

let docPath f = pathScan "/%s" (fun s ctx -> 
  if loaded |> Seq.exists (fun a -> a.id = s) then f s ctx
  else async.Return None)

let app =
  choose [
    path "/" >=> DotLiquid.page "index.html" (loadPage "")
    docPath (fun main -> DotLiquid.page "index.html" (loadPage main))
    Files.browse root ]
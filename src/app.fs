module Services.Main

// --------------------------------------------------------------------------------------
// Entry-point for a real compiled server
// --------------------------------------------------------------------------------------

open Suave
open Suave.Filters
open Suave.Operators
open System

let asm = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
let (</>) a b = IO.Path.Combine(a, b)
let root = IO.Path.GetFullPath(asm </> ".." </> "web")

let app =
  choose [
    path "/" >=> Files.browseFile root "index.html"
    Files.browse root ]

// Start the server at port specified on the command line
let serverConfig =
  let port = 
    match System.Environment.GetCommandLineArgs() |> Seq.tryPick (fun s ->
      if s.StartsWith("port=") then Some(int(s.Substring("port=".Length)))
      else None ) with
    | Some p -> p | _ -> failwith "No port specified"

  { Web.defaultConfig with
      homeFolder = Some __SOURCE_DIRECTORY__
      logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Info
      bindings = [ HttpBinding.mkSimple HTTP "127.0.0.1" port ] }

Web.startWebServer serverConfig app

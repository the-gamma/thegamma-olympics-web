#if INTERACTIVE
#I "../packages"
#r "Suave/lib/net40/Suave.dll"
#else
module OlympicsWeb
#endif

// --------------------------------------------------------------------------------------
// Entry-point for a real compiled server
// --------------------------------------------------------------------------------------

open Suave
open Suave.Filters
open Suave.Operators
open System

let (</>) a b = IO.Path.Combine(a, b)

let asm = 
  if System.Reflection.Assembly.GetExecutingAssembly().IsDynamic then __SOURCE_DIRECTORY__
  else IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
let root = IO.Path.GetFullPath(asm </> ".." </> "web")

let app =
  choose [
    path "/" >=> Files.browseFile root "index.html"
    Files.browse root ]
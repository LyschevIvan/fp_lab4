module Tests

open System
open Xunit
open Lab4
open GameLogic
open Types

let initData path : Cell[,] =
    let f (text: string) =
        let arr =
            text.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
            |> Array.map (fun l -> l.Split([| ' ' |], StringSplitOptions.RemoveEmptyEntries))
            |> array2D
            |> Array2D.map (fun s -> match s with | "X" -> X | "O" -> O | _ -> Null)

        arr

    let data = IO.File.ReadAllText(__SOURCE_DIRECTORY__ + @"/inputData/" + path) |> f
    data



[<Theory>]
[<InlineData("all_x.txt", true)>]
[<InlineData("horizontal_x.txt", true)>]
[<InlineData("vert_x.txt", false)>]
[<InlineData("mainDiag_x.txt", false)>]
[<InlineData("subDiag_x.txt", false)>]
let ``test horizontal win`` (path, exp) =
    let data = initData path
    Assert.Equal(exp, checkHorizontal data)

[<Theory>]
[<InlineData("all_x.txt", true)>]
[<InlineData("horizontal_x.txt", false)>]
[<InlineData("vert_x.txt", true)>]
[<InlineData("mainDiag_x.txt", false)>]
[<InlineData("subDiag_x.txt", false)>]
let ``test vertical win`` (path, exp) =
    let data = initData path
    Assert.Equal(exp, checkVertical data)

[<Theory>]
[<InlineData("all_x.txt", true)>]
[<InlineData("horizontal_x.txt", false)>]
[<InlineData("vert_x.txt", false)>]
[<InlineData("mainDiag_x.txt", true)>]
[<InlineData("subDiag_x.txt", false)>]
let ``test diag win`` (path, exp) =
    let data = initData path
    Assert.Equal(exp, checkMainDiag data)

[<Theory>]
[<InlineData("all_x.txt", true)>]
[<InlineData("horizontal_x.txt", false)>]
[<InlineData("vert_x.txt", false)>]
[<InlineData("mainDiag_x.txt", false)>]
[<InlineData("subDiag_x.txt", true)>]
let ``test sub diag win`` (path, exp) =
    let data = initData path
    Assert.Equal(exp, checkSubDiag data)
    
[<Theory>]
[<InlineData("draw.txt", true)>]
[<InlineData("one_x.txt", false)>]
let ``test field filled`` (path, exp) =
    let data = initData path
    Assert.Equal(exp, checkEnd data)

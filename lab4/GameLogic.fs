namespace lab4

open Types
module GameLogic = 

    let checkSequential (array : Cell[]) =
        match array |> Array.distinct with
        | [|X|] | [|O|] -> true
        | _ -> false

    let checkMainDiag (field:Cell[,]) : bool =
        checkSequential [|for i in 0..field.GetLength(0)-1 -> field.[i,i]|]

    let checkSubDiag (field:Cell[,]) : bool =
        checkSequential [|for i in 0..field.GetLength(0)-1 -> field.[i,field.GetLength(0)-1-i]|]

    let checkVertical (field:Cell[,]) : bool =
        seq {for i in 0..field.GetLength(0)-1 -> field.[i ,*]}
        |> Seq.map (fun row -> checkSequential row)
        |> Seq.contains true

    let checkHorizontal (field:Cell[,]) : bool =
        seq {for i in 0..field.GetLength(0)-1 -> field.[* ,i]}
        |> Seq.map (fun col -> checkSequential col)
        |> Seq.contains true   

    let checkEnd (field:Cell[,]) : bool =
        match field |> Seq.cast<Cell> |> Seq.distinct |> Seq.toList with
        | [X;O] | [O;X] -> true
        | _ -> false

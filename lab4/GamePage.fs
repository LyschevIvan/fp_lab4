namespace lab4

open Xamarin.Forms
open Fabulous.XamarinForms
open Types
open type View
open GameLogic
open System


module GamePage =
    type Pos = int * int
    type Msg =
        | Tap of Pos

    type Model = {
        WithAi : bool
        Size: int
        Field : Cell[,]
        Current : Cell
    }

    let init n withAi : Model=
        {
        WithAi = withAi
        Size = n
        Field = Array2D.create n n Null
        Current = X
        }

    let valueByXY (x, y) (field : Cell[,])=
        match field.[x,y] with
            | X -> "𐌢"
            | O -> "᮰"
            | Null -> ""

    let checkWin (model:Model): Model * Option<GoTo> =
        let field = model.Field
        let validations = seq {
            checkVertical(field);
            checkHorizontal(field);
            checkMainDiag(field);
            checkSubDiag(field);
        }
        
        if Seq.contains true validations then model, Some (GoTo.EndGamePage (model.Current, model.Size, model.WithAi))
        elif checkEnd(field) then model, Some (GoTo.EndGamePage (Null, model.Size, model.WithAi))
        else model, None

    let tapHandler (model:Model) (x,y) : Model =
        let field = Array2D.copy model.Field 
        let curr = model.Current
        match field.[x,y] with
        | Null ->
            field.[x,y] <- curr
            {model with Field = field; Current = !curr}
        | _ -> model


    let getAiTap (field : Cell[,])=
        let rnd = new Random()
        let cells = field |> Array2D.mapi (fun x y cell -> match cell with
                                                                    | Null -> (x,y)
                                                                    | _ -> (-1, -1)
        
        )
        let freeCells : (int*int) list = cells |> Seq.cast<int*int> |> Seq.toList |> List.filter (fun pair -> pair <> (-1,-1)) 
        let r = freeCells.[rnd.Next(0,freeCells.Length-1)]
        r



    let update msg model =
        match msg with 
        | Tap pos -> match tapHandler model pos |> checkWin with
                        | m, None when m.WithAi = true ->
                            tapHandler m (getAiTap m.Field) |> checkWin
                        | m, b -> m,b
       
    let view model =
        ContentPage(
            "game",
            (Grid(coldefs = [Star], rowdefs = [ Stars 5.5; Stars 9.; Stars 5.]) {
                Label("Tic-Tac-Toe with F#")
                    .font(namedSize = NamedSize.Title)
                    .centerTextHorizontal()
                    .centerTextVertical()
                    .gridRow(0).gridColumn(0)
                (Grid(coldefs = [for _ in 0..model.Size-1 -> Star], rowdefs = [for _ in 0..model.Size-1 -> Star]) {
                        for row in [0..model.Size-1] do
                            for col in [0..model.Size-1] do
                                Button(valueByXY (row, col) model.Field, Tap (row,col))
                                            .gridRow(row)
                                            .gridColumn(col)
                                            .background(FabColor.fromHex("#ffffff"))
                                            .cornerRadius(0)
                        
                })
                    .background(FabColor.fromHex("#000000"))
                    .margin(10.)
                    .gridRow(1)
                    .gridColumn(0)
            })
        )
        

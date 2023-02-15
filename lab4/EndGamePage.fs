namespace Lab4

open Fabulous.XamarinForms
open Types
open type View

module EndGamePage =
    type Msg =
        | ToMain
        | Retry

    type Model =
        { Winner: Cell
          Size: int
          WithAi: bool }

    let init (winner: Cell, size: int, withAi: bool) =
        { Winner = winner
          Size = size
          WithAi = withAi }

    let update msg model =
        match msg with
        | ToMain -> model, Some MainPage
        | Retry -> model, Some(GamePage(model.Size, model.WithAi))

    let view model =
        ContentPage(
            "end game",
            (VStack() {
                match model.Winner with
                | X
                | O -> Label($"The winner is: {!model.Winner}!")
                | Null -> Label($"It's a draw!!!")

                Button("Retry", Retry)
                Button("Menu", ToMain)

            })
                .center(expand = true)
                .gridRow(1)
                .gridColumn (0)
        )

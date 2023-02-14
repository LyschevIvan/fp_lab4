namespace lab4

open Xamarin.Forms
open Fabulous.XamarinForms
open Types
open type View
open System

module MainPage =

    type Model =
        {
            n : int
            withAi : bool
        }

    type Msg =
        | Slide of float
        | Start
        | AiToggle

    let init () =
        {
            n = 3
            withAi = false
        }

    let update msg model =
        match msg with
        | Slide v -> {model with n = int (Math.Round v)}, None
        | Start -> model, Some (GoTo.GamePage (model.n, model.withAi))
        | AiToggle -> {model with withAi = not model.withAi}, None


    let view model =
        ContentPage(
            "lab4",
            (Grid(coldefs = [Star], rowdefs = [ Stars 5.5; Stars 9.; Stars 5.]) {
                Label("Tic-Tac-Toe with F#")
                    .font(namedSize = NamedSize.Title)
                    .centerTextHorizontal()
                    .centerTextVertical()
                    .gridRow(0).gridColumn(0)
                (VStack(){
                    Button("Start", Start)
                        .font(namedSize = NamedSize.Large)
                        .borderWidth(2.)
                        .borderColor(FabColor.fromHex("#ffffff"))
                    Label($"Field Size : {model.n}")
                        .centerTextHorizontal()
                    Slider(2.,10.,float model.n, (fun v -> Slide v))
                    HStack(){
                        Label("Play with 'AI'? : ").centerHorizontal(true).centerTextVertical()
                        CheckBox(model.withAi, (fun _ -> AiToggle))
                    }
                    
                })
                    .centerHorizontal(true)
                    .centerVertical(true)
                    .gridRow(1)
                    .gridColumn(0)
                
            })
        )

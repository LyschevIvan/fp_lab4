namespace lab4

open Fabulous
open Fabulous.XamarinForms

open type View
open Types

module App =
    let MaxSize = 10

    type Msg =
        | EndGamePageMsg of EndGamePage.Msg 
        | GamePageMsg of GamePage.Msg
        | MainPageMsg of MainPage.Msg
        //| BackNavigated 

    type Model = {
    
        MainPageModel : MainPage.Model option
        GamePageModel : GamePage.Model option
        EndGamePageModel : EndGamePage.Model option
    }

    let init () : Model =
        let mainPageModel= MainPage.init()
        {
            MainPageModel = Some mainPageModel
            GamePageModel = None
            EndGamePageModel = None
        }

    let hangleGoTo model (goTo : GoTo) =
        match goTo with
        | GamePage (n, withAi)->
            {model with MainPageModel = None; GamePageModel = Some (GamePage.init n withAi); EndGamePageModel = None}
        | EndGamePage (winner, size, withAi) ->
            {model with EndGamePageModel = Some (EndGamePage.init (winner, size, withAi)); GamePageModel = None}
        | MainPage -> init()

    let backNavigated (model: Model) =
        let gameModel = model.GamePageModel
        let endGameModel = model.EndGamePageModel

        match gameModel, endGameModel with
        | None, None -> model
        | Some _, None -> {model with GamePageModel = None}
        | _, Some _ -> {model with EndGamePageModel = None}

    let update msg (model:Model) =
        match msg with
            | GamePageMsg msg ->
                let m, goTo = GamePage.update msg (Option.get model.GamePageModel)
                let newModel = {model with GamePageModel = Some m}
                match goTo with
                | Some gt -> hangleGoTo newModel gt
                | None -> newModel
            | MainPageMsg msg ->
                let m, goTo = MainPage.update msg (Option.get model.MainPageModel)
                let newModel = {model with MainPageModel = Some m}
                match goTo with
                | Some gt -> hangleGoTo newModel gt
                | None -> newModel
            | EndGamePageMsg msg ->
                let m, goTo = EndGamePage.update msg (Option.get model.EndGamePageModel)
                let newModel = {model with EndGamePageModel = Some m}
                match goTo with
                | Some gt -> hangleGoTo newModel gt
                | None -> newModel
            //| BackNavigated -> backNavigated model
    let m = Some 1
    let view (model:Model) =
        Application(
            (NavigationPage(){
                
                match model.MainPageModel with
                | Some mainModel -> View.map MainPageMsg (MainPage.view mainModel)
                | None -> ()

                match model.GamePageModel with
                | Some gameModel -> View.map GamePageMsg (GamePage.view gameModel)
                | None -> ()

                match model.EndGamePageModel with
                | Some endGameModel -> View.map EndGamePageMsg (EndGamePage.view endGameModel)
                | None -> ()
            })
                
        )

    let program = Program.stateful init update view

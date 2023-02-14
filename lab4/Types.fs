namespace lab4

module Types =

    type Cell =
        | X
        | O
        | Null

    type GoTo =
        | GamePage of int * bool
        | EndGamePage of Cell*int*bool
        | MainPage 
    let (!) (cell : Cell) =
        match cell with
        | X -> O        
        | O -> X
        | Null -> Null


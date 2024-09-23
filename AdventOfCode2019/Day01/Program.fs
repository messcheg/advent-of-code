open System.IO
open System

// let filename = "..\\..\\..\\example_input.txt"
let filename = "..\\..\\..\\real_input.txt"

let fuel mass : int = (mass / 3) - 2 : int

let sum (ff : int -> int ) = 
            File.ReadLines(filename) |>  
            Seq.map System.Int32.Parse |>
            Seq.map ff |>
            Seq.sum 

printfn "Answer1: %d" (sum fuel)

let rec fuel2 mass = 
    if mass < 8 then 0
    else (fuel mass) + fuel2 (fuel mass)
    
printfn "Answer2: %d" (sum fuel2)


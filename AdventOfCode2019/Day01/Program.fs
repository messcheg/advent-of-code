open System.IO
open System

// let filename = "..\\..\\..\\example_input.txt"
let filename = "..\\..\\..\\real_input.txt"

let fuel mass = (mass / 3) - 2

let sum = File.ReadLines(filename) |>  Seq.map System.Int32.Parse |>
     Seq.map fuel |>
     Seq.sum 

printfn "Answer1: %d" sum ;;

let rec fuel2 mass = 
    if mass < 8 then 0
    else (fuel mass) + fuel2 (fuel mass)
    
let sum2 = File.ReadLines(filename) |>  Seq.map System.Int32.Parse |>
    Seq.map fuel2 |>
    Seq.sum 

printfn "Answer2: %d" sum2 ;;


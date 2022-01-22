
open System.IO
open System

// let filename = "..\\..\\..\\example_input.txt"
let filename = "..\\..\\..\\real_input.txt"

let inp = File.ReadLines(filename) |> 
            Seq.head |> 
            fun s -> s.Split(',') |>
            Seq.map Int32.Parse 


let inparr = Seq.toArray inp

let get4 arrInp i =
    ( Array.get arrInp i,
    Array.get arrInp (Array.get arrInp (i+1)),
    Array.get arrInp (Array.get arrInp (i+2)),
    Array.get arrInp (i+3))

let part2init arrInp a b = 
    Array.set arrInp 1 a
    Array.set arrInp 2 b
    arrInp

let dorun arrInp =
    let mutable i : int = 0
    while (Array.get arrInp i) <> 99 do
        match (get4 arrInp i) with
            | ( a , b , c , d ) when a = 1 -> Array.set arrInp d (b+c)
            | ( a , b , c , d ) when a = 2 -> Array.set arrInp d (b*c)
        i <- i + 4
    Array.get arrInp 0

printfn "Answer1: %d" (dorun (part2init (Array.copy inparr) 12 2))

let determine arrInp =
    let mutable p = 0
    let mutable q = 0
    while (dorun (part2init (Array.copy inparr) p q)) <> 19690720 do
        p <- p + 1
        if p > 99 then
            q <- q + 1
            p <- 0
    p * 100 + q

printfn "Answer2: %d" (determine inparr)

    

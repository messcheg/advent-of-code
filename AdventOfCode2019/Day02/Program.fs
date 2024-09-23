
open System.IO
open System
open IntcodeMachine.Intcode

// let filename = "..\\..\\..\\example_input.txt"
let filename = "..\\..\\..\\real_input.txt"

let inparr = inp filename
let part2init arrInp a b = 
    Array.set arrInp 1 a
    Array.set arrInp 2 b
    arrInp

let runmachine arr a b = 
    let prep = part2init (Array.copy arr) a b
    let res = dorun prep 0
    prep[0]

printfn "Answer1: %d" (runmachine inparr 12 2)

let determine arrInp =
    let mutable p = 0
    let mutable q = 0
    while (runmachine inparr p q) <> 19690720 do
        p <- p + 1
        if p > 99 then
            q <- q + 1
            p <- 0
    p * 100 + q

printfn "Answer2: %d" (determine inparr)
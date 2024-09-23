open System.IO
open System
open IntcodeMachine.Intcode

//let filename = "..\\..\\..\\example_input1.txt"
let filename = "..\\..\\..\\real_input.txt"

let inparr = IntcodeMachine.Intcode.inp filename

printfn "Answer1: %d" (dorun (Array.copy inparr) 1)
printfn "Answer2: %d" (dorun (Array.copy inparr) 5)


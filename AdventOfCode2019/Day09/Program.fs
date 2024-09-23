
open IntcodeMachine.Intcode

let exmpl1 = "..\\..\\..\\example_input1.txt"
let exmpl2 = "..\\..\\..\\example_input2.txt"
let filename = "..\\..\\..\\real_input.txt"

let exmplarr1 = IntcodeMachine.Intcode.inp exmpl1

printfn "Example1:"
for v in dorun1 exmplarr1 [||] do
    printf "%d," v
printfn ""

let exmplarr2 = IntcodeMachine.Intcode.inp exmpl2

printfn "Example2:"
for v in dorun1 exmplarr2 [||] do
    printf "%d," v
printfn ""

let realarr = IntcodeMachine.Intcode.inp filename
printfn "Answer1:"
for v in dorun1 (Array.copy realarr) [| 1 |] do
    printf "%d," v
printfn ""

printfn "Answer2:"
for v in dorun1 (Array.copy realarr) [| 2 |] do
    printf "%d," v
printfn ""

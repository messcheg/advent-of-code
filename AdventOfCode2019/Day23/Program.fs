open System.IO
open IntcodeMachine.Intcode

let filename = "E:\\develop\\advent-of-code-input\\2019\\Day23.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let rec getinstruction pst txt =
    let (a,pst1) = runNextStepArr [||] pst
    let c = (string (char (a)))
    let txt1 = txt + c
    if c = "\n" then (txt1, pst1)
    else getinstruction pst1 txt1

let bootup number =
    let pst = initalprogstate (Array.copy prog1)
    let pst1 = runNextStep number pst
    // runFunInUntilOut arrInp arrExtra (fun x -> getInput[int x]) pc pbase
    pst1

let startSet = 
    [|0..49|] |>
    Array.map(fun a -> bootup a)

printfn "Count: %d" startSet.Length
printfn "trans: %s"  (string (char (73)))




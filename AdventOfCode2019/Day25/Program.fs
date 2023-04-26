open System.IO
open IntcodeMachine.Intcode

let filename = "E:\\develop\\advent-of-code-input\\2019\\Day25.txt"

let prog1 = IntcodeMachine.Intcode.inp filename


let rec runsteps pst cnt = 
    if cnt > 0 then
        let pst1 = runNextStepArr [||] pst
        printf "%s" ((char (fst (fst pst1))).ToString())
        runsteps (snd pst1) (cnt-1)
    else
        true

runsteps (initalprogstate (Array.copy prog1)) 300 |> ignore

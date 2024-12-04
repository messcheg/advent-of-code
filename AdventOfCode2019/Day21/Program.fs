open System.IO
open IntcodeMachine.Intcode

let filename = "E:\\develop\\advent-of-code-input\\2019\\Day21.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let JProg1 = "NOT A J\nNOT B T\nOR T J\nNOT C T\nOR T J\nAND D J\nWALK\n"

let transJProg1 = JProg1 |> Seq.map(fun x -> int64 x) |>  Seq.toArray

let rec p1 (transJProg:int64 array) x pst (txt:string) = 
    if x = 0 then txt 
    elif txt = "Input instructions:\n" && transJProg.Length = 0 then "No Succes!!"
    else
        let (txt1,instr,newJProg) = if txt = "Input instructions:\n" then ("",transJProg, [||]) else (txt, [||], transJProg)  
        let a = runNextStepArr instr pst
        let (b,_) = fst a
        if b > 255 || b = 0 then string b 
        else 
            let txt2 = if txt1.EndsWith '\n' then "" else txt1
            let c = string (char (b))
            printf "%s" c
            let nTxt = txt2 + c
            p1 newJProg (x-1) (snd a) nTxt

let prompt1 = p1 transJProg1 2000 (initalprogstate (Array.copy prog1)) ""

printfn "%s" prompt1

let JProg2 = "NOT E J\nNOT I T\nOR T J\nNOT B T\nAND T J\nNOT A T\nOR T J\nNOT C T\nAND H T\nOR T J\nAND D J\nRUN\n"

let transJProg2 = JProg2 |> Seq.map(fun x -> int64 x) |>  Seq.toArray

let prompt2 = p1 transJProg2 1000 (initalprogstate (Array.copy prog1)) ""

printfn "%s" prompt2
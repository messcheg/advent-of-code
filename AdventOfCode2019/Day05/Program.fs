open System.IO
open System

//let filename = "..\\..\\..\\example_input1.txt"
let filename = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines(filename) |> 
    Seq.head |> 
    fun s -> 
        s.Split(',') |>
        Seq.map Int32.Parse 

let inparr = Seq.toArray inp

let indirect arrInp i = 
    Array.get arrInp (Array.get arrInp i)   

let getOpcode (arrInp : int[]) i =
    let num = Array.get arrInp i
    ( num / 10000,
     (num % 10000) / 1000,
     (num % 1000) / 100,
      num % 100)

let getValue arrInp mode i =
    let p = Array.get arrInp i 
    if mode = 0 then Array.get arrInp p
    else p

let setValue arrInp mode i v =
    let p = 
        if mode = 0 then Array.get arrInp i
        else  Array.get arrInp i // set is always indirect
    Array.set arrInp p v 

let opr opc a b =
    if opc = 1 then a + b
    elif opc = 2 then a * b
    elif opc = 7 then  if a < b then 1 else 0
    elif opc = 8 then  if a = b then 1 else 0
    else 0

let dorun arrInp getInput =
    let mutable i : int = 0
    let mutable outp = 0
    while (Array.get arrInp i) <> 99 do
        match (getOpcode arrInp i) with
            | ( a , b , c , de ) when de = 1 || de = 2 || de = 7 || de = 8 -> 
                setValue arrInp a (i+3) (opr de (getValue arrInp c (i+1)) (getValue arrInp b (i+2)))
                i <- i + 4
            | ( a , b , c , de ) when de = 3 ->
                setValue arrInp c (i+1) getInput
                i <- i + 2
            | ( a , b , c , de ) when de = 4 ->
                outp <- getValue arrInp c (i+1)
                i <- i + 2
            | ( a , b , c , de ) when de = 5 || de = 6 ->
                let cond = getValue arrInp c (i+1)
                if de = 6 && cond = 0 || de = 5 && cond > 0 then i <- getValue arrInp b (i+2)
                else i <- i + 3
            | _ -> i <- i + 1
    outp

printfn "Answer1: %d" (dorun (Array.copy inparr) 1)
printfn "Answer2: %d" (dorun (Array.copy inparr) 5)


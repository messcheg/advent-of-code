
namespace IntcodeMachine
open System.IO
open System

module Intcode =
    let inp filename= 
        File.ReadLines(filename) |> 
        Seq.head |> 
        (fun s -> 
            s.Split(',') |>
            Seq.map Int64.Parse) |> 
        Seq.toArray
    
    let indirect arrInp i = 
        Array.get arrInp (Array.get arrInp i)   
    
    let getOpcode (arrInp : int64[]) i =
        let num = Array.get arrInp i
        ( num / 10000L,
         (num % 10000L) / 1000L,
         (num % 1000L) / 100L,
          num % 100L)
    
    let getValue (arrInp : int64[]) mode i =
        let p = Array.get arrInp i 
        if mode = 0 then Array.get arrInp (int p)
        else p
    
    let setValue (arrInp : int64[]) mode i v =
        let p = 
            if mode = 0 then Array.get arrInp i
            else  Array.get arrInp i // set is always indirect
        Array.set arrInp (int p) v 
    
    let opr opc a b =
        if opc = 1 then a + b
        elif opc = 2 then a * b
        elif opc = 7 then  if a < b then 1 else 0
        elif opc = 8 then  if a = b then 1 else 0
        else 0
    
    let doruntilout (arrInp : int64[] ) (getInput : int64[]) pc  =
        let mutable i : int = pc
        let mutable out = (0L,0)
        let mutable inpcnt = 0
        while (Array.get arrInp i) <> 99 && out = (0L,0) do
            match (getOpcode arrInp i) with
                | ( a , b , c , de ) when de = 1 || de = 2 || de = 7 || de = 8 -> 
                    setValue arrInp (int a) (i+3) (opr (int de) (int (getValue arrInp (int c) (i+1))) (int (getValue arrInp (int b) (i+2))))
                    i <- i + 4
                | ( a , b , c , de ) when de = 3 ->
                    setValue arrInp (int c) (i+1) getInput[inpcnt]
                    inpcnt <- inpcnt + 1
                    i <- i + 2
                | ( a , b , c , de ) when de = 4 ->
                    out <- (getValue arrInp (int c) (i+1), i + 2)
                    i <- i + 2
                | ( a , b , c , de ) when de = 5 || de = 6 ->
                    let cond = getValue arrInp (int c) (i+1)
                    if de = 6 && cond = 0 || de = 5 && cond > 0 then i <- int (getValue arrInp (int b) (i+2))
                    else i <- i + 3
                | _ -> i <- i + 1
        out

    let dorun1 (arrInp : int64[] ) (getInput : int64[]) =
        let mutable i : int = 0
        let mutable inpcnt = 0
        let mutable outp : int64[] = [||]
        while (Array.get arrInp i) <> 99 do
            match (getOpcode arrInp i) with
                | ( a , b , c , de ) when de = 1 || de = 2 || de = 7 || de = 8 -> 
                    setValue arrInp (int a) (i+3) (opr (int de) (int (getValue arrInp (int c) (i+1))) (int (getValue arrInp (int b) (i+2))))
                    i <- i + 4
                | ( a , b , c , de ) when de = 3 ->
                    setValue arrInp (int c) (i+1) getInput[inpcnt]
                    inpcnt <- inpcnt + 1
                    i <- i + 2
                | ( a , b , c , de ) when de = 4 ->
                    outp <- Array.append outp [|getValue arrInp (int c) (i+1)|]
                    i <- i + 2
                | ( a , b , c , de ) when de = 5 || de = 6 ->
                    let cond = getValue arrInp (int c) (i+1)
                    if de = 6 && cond = 0 || de = 5 && cond > 0 then i <- int (getValue arrInp (int b) (i+2))
                    else i <- i + 3
                | _ -> i <- i + 1
        outp
    
    let dorun arrInp (getInput : int) =
        let res = dorun1 arrInp [| getInput |]
        if res = [||] then 0L
        else res[0]
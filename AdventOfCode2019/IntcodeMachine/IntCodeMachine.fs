
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

    let getfromArray (arrInp, (arrExtra : (int * int64)[]))  i : int64=
        if i < Array.length arrInp then Array.get arrInp i
        else 
            let res = arrExtra |> Array.filter(fun (a,b) -> a = i)
            if Array.length res > 0 then snd res[0] 
            else 0L

    let setInArray (arrInp, (arrExtra : (int * int64)[])) i v =
        if i < Array.length arrInp then 
            Array.set arrInp i v
            (arrInp, arrExtra)            
        else    
            if arrExtra |> Array.exists(fun (a,_) -> a = i) then 
                (arrInp, Array.map (fun (a,b) -> if a = i then (a,v) else (a,b)) arrExtra)  
            else 
                (arrInp, Array.append arrExtra [|(i , v)|])
                   
    let getValue arrs mode parambase i =
        let p = getfromArray arrs i 
        if mode = 0 then getfromArray arrs (int p)
        elif mode = 2 then getfromArray arrs ((int p) + parambase)
        else p
    
    let setValue arrs mode parambase i v =
        let p = 
            if mode = 2 then (getfromArray arrs i) + parambase
            else getfromArray arrs i // set is always indirect
        setInArray arrs (int p) v 
    
    let opr (opc : int) (a:int64) (b:int64) : int64 =
        if opc = 1 then a + b
        elif opc = 2 then a * b
        elif opc = 7 then  if a < b then 1 else 0
        elif opc = 8 then  if a = b then 1 else 0
        else 0
    
    let gfst (a,_,_,_,_,_) = a
    let gsnd (_,b,_,_,_,_) = b
    let gtrd (_,_,c,_,_,_) = c
    let gfrt (_,_,_,d,_,_) = d
    let gfft (_,_,_,_,e,_) = e
    let gsxt (_,_,_,_,_,f) = f


    let runFunInUntilOut (arrInp : int64[]) (arrExtra : (int * int64)[]) (funinp : int -> bool*int64) pc (pbase : int) =
        let mutable i : int = pc
        let mutable out = (0L,0,(0, pbase), true, (arrInp, arrExtra), false)
        let mutable inpcnt = 0
        let mutable parambase = pbase
        let mutable arrs = (arrInp, arrExtra)
        while (Array.get arrInp i) <> 99 && (gsnd out) = 0 do
            match (getOpcode arrInp i) with
                | ( a , b , c , de ) when de = 1 || de = 2 || de = 7 || de = 8 -> 
                    arrs <- setValue arrs (int a) parambase (i+3) (opr (int de) (getValue arrs (int c) parambase (i+1)) (getValue arrs (int b) parambase (i+2)))
                    i <- i + 4
                | ( _ , _ , c , de ) when de = 3 ->
                    let (hasInp, inp) = (funinp inpcnt)
                    if hasInp then
                        arrs <- setValue arrs (int c) parambase (i+1) inp
                        inpcnt <- inpcnt + 1
                        out <- (gfst out, gsnd out, (inpcnt, parambase), true, arrs, false)
                        i <- i + 2
                    else
                        out <- (gfst out, i, (inpcnt, parambase), false, arrs, true)
                | ( _ , _ , c , de ) when de = 4 ->
                    out <- (getValue arrs (int c) parambase (i+1), i + 2, (inpcnt, parambase), false, arrs, false)
                    i <- i + 2
                | ( _ , b , c , de ) when de = 5 || de = 6 ->
                    let cond = getValue arrs (int c) parambase (i+1)
                    if de = 6 && cond = 0 || de = 5 && cond > 0 then i <- int (getValue arrs (int b) parambase (i+2))
                    else i <- i + 3
                | ( _ , _ , c , de ) when de = 9 ->
                    parambase <- parambase + int (getValue arrs (int c) parambase (i+1))
                    i <- i + 2
                | _ -> i <- i + 1
        out
    
    let getInpFunc x ar = 
        if x < Array.length ar then (true, ar[x])
        else (false, 0L)

    let doruntilout (arrInp : int64[] ) (arrExtra : (int * int64)[]) (getInput : int64[]) pc (pbase : int) =
        runFunInUntilOut arrInp arrExtra (fun x -> getInpFunc x getInput) pc pbase
    
    let doRunUntilOutOfIn  (arrInp : int64[] ) (arrExtra : (int * int64)[]) (getInput : int64[]) pc (pbase : int) =
        runFunInUntilOut arrInp arrExtra (fun x -> getInpFunc x getInput) pc pbase
        

    let runNextStep (inVal:int64) ((arrExtra : (int * int64)[]), (prog : int64[]), pc, (pbase : int), _) =
        let (outval, pc1, (_,pbase1), finish, (prog1, arrExtra1), waitForInput) = doruntilout prog arrExtra [|inVal|] pc pbase
        ((outval, waitForInput),(arrExtra1, prog1, pc1, pbase1, finish))    
    
    let runNextStepArr (inVals:int64 array) ((arrExtra : (int * int64)[]), (prog : int64[]), pc, (pbase : int), _) =
        let (outval, pc1, (_,pbase1), finish, (prog1, arrExtra1),waitForInput) = doruntilout prog arrExtra inVals pc pbase
        ((outval, waitForInput),(arrExtra1, prog1, pc1, pbase1, finish))    
    

    let isFinished (_,(_,_,_,_,finished)) = finished
    
    let initalprogstate prog  = ([||], prog, 0, 0, false)
      
    let runFirstStep (inVal:int64) prog =
        runNextStep inVal (initalprogstate prog)
    
    let dorun1 (arrInp : int64[] ) (getInput : int64[]) =
        let mutable pc : int = 0
        let mutable inpcnt = 0
        let mutable outp : int64[] = [||]
        let mutable finish = false
        let mutable arrExtra : (int*int64)[] = [||]
        let mutable pbase = 0
        while  not finish do
            let res = doruntilout arrInp arrExtra getInput[inpcnt..] pc pbase
            if (gsnd res) > 0 then outp <- Array.append outp [|gfst res|]
            inpcnt <- fst (gtrd res)
            pc <- gsnd res
            finish <- gfrt res
            arrExtra <- snd (gfft res)
            pbase <- snd (gtrd res)
        outp
    
    let dorun arrInp (getInput : int) =
        let res = dorun1 arrInp [| getInput |]
        if res = [||] then 0L
        else res[0]
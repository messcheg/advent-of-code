open IntcodeMachine.Intcode

// let filename = "..\\..\\..\\example_input1.txt"
let filename = "..\\..\\..\\real_input.txt"

let inparr = IntcodeMachine.Intcode.inp filename

let checksequence (seq1 :int64[]) inputsignal : int64 =
    let mutable out : int64 = inputsignal
    let mutable amplifiers = Map []
    for c in seq1 do
        let newarr = Array.copy inparr
        let res = (doruntilout newarr [||] [| c ; out |] 0 0)
        out <- gfst res
        amplifiers <- amplifiers.Add(c,(newarr,gsnd res))
    
    let mutable finish = false
    while not finish do
        for c in seq1 do
            let nxt = amplifiers[c]
            let arr = fst nxt
            let res = (doruntilout arr [||] [| out |] (snd nxt) 0)
            finish <- finish || gsnd res = 0
            out <- if finish then out else gfst res
            amplifiers <- amplifiers.Remove(c)
            amplifiers <- amplifiers.Add(c, (arr, gsnd res))
    out


let rec bestcombi (s1 : int64[]) (s2 : int64[]) inputsignal : int64 =
    if s2 <> [||] then  
        s2 |>
        Array.map 
            ( fun i -> 
                bestcombi (Array.append s1 [|i|]) (Array.filter (fun j -> j <> i) s2) inputsignal ) |>
        Array.max
    else
        checksequence s1 inputsignal
        
let findbest : int64 =
    bestcombi [||] [|0;1;2;3;4|] 0
printfn "Answer1: %d" findbest

let findamplified : int64 =
    bestcombi [||] [|5;6;7;8;9|] 0
    
printfn "Answer2: %d" findamplified 
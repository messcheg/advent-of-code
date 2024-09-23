open System.IO
open IntcodeMachine.Intcode

let filename = "E:\\develop\\advent-of-code-input\\2019\\Day19.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let getfield x y = fst(fst (runNextStepArr [| x; y|] (initalprogstate (Array.copy prog1)))  )

let rec countfields X Y x y = 
    if y = Y then 0L
    else
        let (nX, nY) = if x = X-1L then (0L, y+1L) else (x+1L,y)
        let v = getfield x y 
        let c = if v=1 then '#' else '.'
        let _ = if x = X-1L then printfn "%c" c else printf "%c" c
        v + countfields X Y nX nY

printfn "Answer1: %d" (countfields 50L 50L 0L 0L)

let rec boundL x y =
    let v1 = getfield x y
    if v1 = 0L then boundL (x+1L) y
    else 
        let v = getfield (x-1L) y
        if v = 0L then x
        else boundL (x-1L) y

let rec boundR x y =
    let v1 = getfield x y
    if v1 = 0 then boundR (x-1L) y
    else 
        let v = getfield (x+1L) y
        if v = 0L then x
        else boundR (x+1L) y

let rec discoverspace (size:int) x1 x2 (y:int64) Y (beampart:List<int64*int64>) =
    if y = Y then (0L,0L)
    else
        let bs = beampart.Length = size
        let Xmax = if bs then  snd ( beampart |> Seq.minBy (fun (_,x) -> x)) else 0L
        let Xmin = if bs then  fst ( beampart |> Seq.maxBy (fun (x,_) -> x)) else 0L 
        if bs && (Xmax-Xmin+1L) >= size then
            (Xmin, y - int64 size)
        else
            let nX1 = boundL x1 y
            let nX2 = boundR x2 y        
            let bp2 = 
                if nX2 - nX1 + 1L >= size then 
                    if bs then beampart.Tail @ [(nX1,nX2)]
                    else beampart @ [(nX1,nX2)]
                else List.empty
            discoverspace size nX1 nX2 (y+1L) Y bp2

let answer2 = 
    let (x,y) = discoverspace 100 320 320 400 10000 List.empty
    (x * 10000L) + y

printfn "Answer2: %d" answer2

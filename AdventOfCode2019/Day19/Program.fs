open System.IO
open IntcodeMachine.Intcode

let filename = "E:\\develop\\advent-of-code-input\\2019\\Day19.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let getfield x y progstate = runNextStepArr [| x; y|] progstate  

let rec countfields X Y x y = 
    if y = Y then 0L
    else
        let (nX, nY) = if x = X-1L then (0L, y+1L) else (x+1L,y)
        let (v, _) = getfield x y (initalprogstate (Array.copy prog1))
        let c = if v=1 then '#' else '.'
        let _ = if x = X-1L then printfn "%c" c else printf "%c" c
        v + countfields X Y nX nY

printfn "%d" (countfields 50L 50L 0L 0L)




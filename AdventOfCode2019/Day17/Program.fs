open IntcodeMachine.Intcode
open System
open System.Threading

let filename = "..\\..\\..\\IntcodeProg.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let rec initialfield ps = 
    let s = runNextStep 0 ps
    if isFinished s then []
    else [fst s] @ initialfield (snd s)

let playfield = 
    initialfield (initalprogstate (Array.copy prog1)) |>
    Seq.mapFold(fun s i -> ((char i, s), if i = 10L then s+1 else s)) 0 |>
    fst |>
    Seq.filter(fun (c,_) -> int c <> 10) |>
    Seq.groupBy(fun (_, s) -> s) |>
    Seq.sortBy(fun (k, _) -> k) |>
    Seq.map(fun (_, sq) -> sq |> Seq.map(fun (c,_)-> c) |> Seq.toArray) |>
    Seq.toArray

let getIntersections pf = 
    let lpf = Array.length pf
    let lr = Array.length pf[0]
    let mutable intersections = []
    for i = 1 to lpf - 2 do 
        for j = 1 to lr - 2 do
            if  pf[i][j] = '#' &&
                pf[i+1][j] = '#' &&
                pf[i-1][j] = '#' &&
                pf[i][j+1] = '#' &&
                pf[i][j-1] = '#' then intersections <- intersections @ [(i,j)]
    intersections
                                       
let printfield pf =
    let insec = getIntersections pf
    for i = 0 to pf.Length - 1 do
        for j = 0 to pf[i].Length - 1 do
            let c = if (insec |> List.contains (i,j)) then 'O' else pf[i][j]
            printf "%c" c
        printfn ""

printfield playfield

let answer1 = 
    getIntersections playfield |>
    Seq.map(fun (i,j) -> i*j) |>
    Seq.sum

printfn "Answer1 = %d" answer1
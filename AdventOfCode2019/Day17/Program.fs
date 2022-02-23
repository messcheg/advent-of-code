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

let numberofscaffolds pf = 
    pf |> 
    Seq.map(fun s -> 
        s |> 
        Seq.where(fun c -> c = '#' || c = '^') |> 
        Seq.length) |>
    Seq.sum 

let findVacuumcleaner pf = 
    let mutable ret = (0,0)
    for i = 0 to (pf |> Array.length) - 1 do
        let l = pf[i]
        for j = 0 to (l |> Array.length)- 1 do
            let c = l[j]
            if c = '^' || c = 'v' || c = '>' || c = '<' then ret <- (i,j)
    ret

let rec getpath pf (i,j) visited scafleft intersections =
    let visited1 = visited @ [(i,j)]
    if scafleft = 0 then (true, (visited1, (i,j)))
    else
        let maxI = (Array.length pf) - 1
        let maxJ = (Array.length pf[0]) - 1
        let check (I,J) = 
            if J <= maxJ && J >= 0 && I <= maxI && I >= 0 &&  
                pf[I][J] = '#' then 
                if  not (List.contains (I,J) visited) then 
                    getpath pf (I, J) visited1 (scafleft - 1) intersections
                elif List.contains (I,J) intersections then
                    getpath pf (I, J) visited1 scafleft (intersections |> List.filter(fun p -> p <> (I,J))) 
                else (false, (visited1, (I,J)))
            else (false, (visited1, (I,J)))
        let p1 = check (i,j+1) 
        if fst p1 then p1
        else    
            let p2 = check (i,j-1) 
            if fst p2 then p2
            else    
                let p3 = check (i+1,j) 
                if fst p3 then p3
                else    
                    check (i-1,j) 
                    
                
                      
let path pf = getpath pf (findVacuumcleaner pf) [] (numberofscaffolds pf) (getIntersections pf)

let answer2 = 
    path playfield

printfn "A2 %d" (Seq.length (fst (snd answer2)))

open System.IO

let ex1 = (165, "..\\..\\..\\Example1.txt")
let ex2 = (13312, "..\\..\\..\\Example3.txt")
let ex3 = (180697, "..\\..\\..\\Example2.txt")
let ex4 = (2210736, "..\\..\\..\\Example4.txt")


let input = ex3

let lookupInp = 
    File.ReadLines(snd input) |>
    Seq.map( fun s -> 
        let s1 = s.Split(" => ")
        (s1[1], s1[0])) |>
    Seq.map( fun (a,b) ->
        let b1 = b.Split ", " 
        let bt = b1 |> Array.map (fun a -> 
            let a1 = a.Split(' ')
            (a1[1], System.Int64.Parse(a1[0])))
        let aa = a.Split(' ')
        (aa[1],(System.Int64.Parse(aa[0]), bt))) |>
    Seq.toArray

let lookup = dict lookupInp

let rec hitFromToOre (n:string) =
    if n = "ORE" then [|n|]
    else
        let mutable arr = [|n|]
        let (_,ingredients) = lookup[n]
        for (n1 , _) in ingredients do   
            arr <- Array.append arr (hitFromToOre n1 )
        arr

let hits = 
    let hitcnt = 
        hitFromToOre "FUEL" |> 
        Array.countBy(fun a -> a) 
    let hc2 = Array.map2(fun (a,b) c -> (a, (b, c))) hitcnt [|0..(Array.length hitcnt) - 1|]
    dict hc2
    
let rec investigateneeds (n:string) number (reqs:int64[][]) = 
    let (gain, ingredients) = if n= "ORE" then (1L, [||]) else lookup[n]
    let (hit, idx) = hits[n]
    reqs[idx] <- Array.append reqs[idx] [|number|]
    if (Array.length reqs[idx]) = hit then
        let needs = (Array.sum reqs[idx])
        let req = needs/gain + (if needs % gain > 0L then 1L else 0L)
        for (ing, need) in ingredients do   
            investigateneeds ing (req * need) reqs |> ignore 
    reqs

let counts = 
    investigateneeds "FUEL" 1L (hits.Keys |> Seq.map(fun _ -> [||]) |> Seq.toArray) |>
    Array.map(fun ar -> Array.fold(+) 0L ar) 

let answer1 =
    counts[snd hits["ORE"]]

printfn "Answer1: %d, expected: %d" answer1 (fst input) 

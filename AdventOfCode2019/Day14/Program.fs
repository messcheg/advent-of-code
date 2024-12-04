open System.IO

let ex1 = (165, "..\\..\\..\\Example1.txt")
let ex2 = (13312, "..\\..\\..\\Example3.txt")
let ex3 = (180697, "..\\..\\..\\Example2.txt")
let ex4 = (2210736, "..\\..\\..\\Example4.txt")
let real1 = (0, "..\\..\\..\\RealInput.txt")

let input = real1

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
    
let rec investigateneeds (n:string) numberToUse (reqs:(int64*int64)[]) = 
    let (gain, ingredients) = if n= "ORE" then (1L, [||]) else lookup[n]
    let (hit, idx) = hits[n]
    let (reqTot1, numTot1) = reqs[idx] 
    let spare = numTot1 - reqTot1
    let needs = numberToUse - spare
    let requested = needs/gain + (if needs % gain > 0L then 1L else 0L)
    reqs[idx] <- (reqTot1 + numberToUse, numTot1 + requested * gain)
    for (ing, need) in ingredients do   
        investigateneeds ing (requested * need) reqs |> ignore 
    reqs

let counts (n:int64) = investigateneeds "FUEL" n (hits.Keys |> Seq.map(fun _ -> (0L,0L)) |> Seq.toArray)

let fuelInOre (n:int64) = snd ((counts n)[snd hits["ORE"]])

let answer1 = fuelInOre 1L

printfn "Answer1: %d, expected: %d" answer1 (fst input) 

let availablOre = 1000000000000L

let rec findMaxFuel (under:int64) ore rest =
    let under1 = 
        if rest < answer1 then under + 1L
        else under + rest / answer1
    let subresult = fuelInOre under1
    if subresult > (ore + rest) then under
    else findMaxFuel under1 subresult (ore + rest - subresult)

let answer2 = findMaxFuel 0L 0L availablOre
printfn "Answer2: %d" answer2 


open System.IO
open System

// let filename = "..\\..\\..\\example_input1.txt"
let filename = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines(filename) |> 
    Seq.map ( fun s ->  s.Split(')'))

let planets arrinp =
    arrinp |>
    Seq.groupBy (fun (s : string[])  -> s[0]) |>
    Seq.map (fun (b,c) -> (b, c |> Seq.map(fun d -> d[1]))) |>
    Seq.toArray

let rec countOrbits plnts start prevcnt=
    let orbits = Array.tryFind (fun (t, l) -> t = start) plnts
    if orbits = None then prevcnt
    else 
        let mutable cnt = prevcnt
        for p in snd orbits.Value do 
            cnt <- cnt + (countOrbits plnts p (prevcnt + 1))
        cnt

let rec findTransfer plnts start san you =
    let orbits = Array.tryFind (fun (t, l) -> t = start) plnts
    if orbits = None then (0,0)
    else 
        let mutable cntSan = 0
        let mutable cntYou = 0
        let mutable res = (0,0)
        for p in snd orbits.Value do 
            if p = san then cntSan <- 1
            elif p = you then cntYou <- 1
            elif cntSan = 0 || cntYou = 0 then
                let cnts = findTransfer plnts p san you
                if fst cnts > 0 && snd cnts = 0 then
                    cntYou <- fst cnts + 1
                elif fst cnts = 0 && snd cnts > 0 then
                    cntSan <- snd cnts + 1
                elif fst cnts > 0 && snd cnts > 0 then
                    res <- cnts
            if cntSan >0 && cntYou > 0 && res = (0,0) then
                let x = cntSan + cntYou - 2
                let xx = (x , x)
                res <- xx
        if res = (0,0) then (cntYou, cntSan)
        else res

let answer1 = countOrbits (planets inp) "COM"
printfn "Answer1: %d" (answer1 0)

let answer2 = findTransfer (planets inp) "COM" "SAN" "YOU"
printfn "Answer2: %d" (fst answer2)

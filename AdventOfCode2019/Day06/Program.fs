
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

let answer1 = countOrbits (planets inp) "COM"
printfn "Answer1: %d" (answer1 0)

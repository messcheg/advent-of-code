open System.IO
open System

// let filename = "..\\..\\..\\example_input.txt"
let filename = "..\\..\\..\\real_input.txt"

let pWide = 25
let pTall = 6

let layers = 
    File.ReadLines(filename) |>  
    Seq.head |>
    Seq.chunkBySize( pWide * pTall)

let prod (arr : int[]) = 
    arr[0] * arr[1]

let answer1 =
    layers |>
    Seq.map (fun s -> (s |> Seq.countBy(fun c -> c) )) |>
    Seq.sortBy(fun arr -> arr |> Seq.filter (fun t -> (fst t = '0')) |> Seq.exactlyOne |> snd) |>
    Seq.head |> 
    Seq.filter (fun t -> (fst t = '1' || fst t = '2')) |>
    Seq.map (fun t -> snd t) |>
    Seq.toArray |>
    prod

printfn "Answer1: %d" answer1

let rec finalPicture arr1 layrs =
    if Seq.isEmpty layrs then arr1
    else
        let arr2 = Seq.head layrs
        let resarr = 
            Array.zip arr1 arr2 |>
            Array.map (fun (a,b) -> if a = '2' then b else a)
        finalPicture resarr (Seq.tail layrs)

let answer2 = finalPicture (Seq.head layers) (Seq.tail layers)

let showmessage =
    for rnge in (answer2 |> Array.chunkBySize pWide) do 
        for c in rnge do
            if c = '1' then printf "O"
            else printf " "
        printfn ""

showmessage
open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let ex2 = "..\\..\\..\\Example2.txt"
let rl = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines ex1 |>
    Seq.toArray

let rec startlocation i j =
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    if inp[i][j] = '@' then (i,j)
    else
        if j+1 < lenJ then startlocation i (j+1)
        elif i+1 < lenI then startlocation (i+1) 0
        else (-1,-1)

let rec allKeys startlocation i j = 
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    let own = if inp[i][j] in {'a'..'z'} then {inp[i][j]} else {}

        if j+1 < lenJ then startlocation i (j+1)
        elif i+1 < lenI then startlocation (i+1) 0
        else (-1,-1)

let reachable i j keys cost =
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    if inp[i][j] = '.' then
        


let rec findpath worklist visited =
    let next = List.head worklist

    // mijn idee voor de oplossing: set-met-sleutels-leeg is "klaar"
    // sleutel pakken is uit-de-set kortstepad naar de sleutel
    // open deuren
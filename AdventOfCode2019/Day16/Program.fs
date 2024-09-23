open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let ex2 = "..\\..\\..\\Example2.txt"
let rl = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines rl |>
    Seq.head |>
    Seq.map (fun c -> (int c) - (int '0')) |>
    Seq.toList

let multi startindex requestedindex =
    let mapsto = ((requestedindex + 1) % (4 * (startindex + 1))) /(startindex + 1)
    [|0;1;0;-1|][mapsto]

let rec FFT count input =
    if count = 0 then input
    else 
        let newlist = 
            [0..(List.length input) - 1] |>
            List.map(fun i -> 
                System.Math.Abs (input[i..] |> 
                Seq.mapi(fun j p -> p * multi i (j+i) ) |>
                Seq.sum) % 10)
        FFT (count - 1) newlist

let FFT2 count input times from until =
    let len = Array.length input
    let rightpart = (times * len) - from
    let res = Array.create rightpart 0  
    let mutable sum = 0
    let mutable l = len
    for i = rightpart-1 downto 0 do
        l <- l - 1
        sum <- sum + input[l]
        if sum > 9 then sum <- sum - 10
        res[i] <- sum
        if l = 0 then l <- len
    for j in 1..(count-1) do
        for i = rightpart-2 downto 0 do
            res[i] <- res[i] + res[i+1]
            if res[i] > 9 then res[i] <- res[i] - 10
    res[0..(until-1)]

let answer1 = 
    FFT 100 inp |>
    Seq.map (fun i -> char (i + (int '0'))) |>
    Seq.toArray

printf "Answer1: "
for i in 0..7 do
    printf "%c" answer1[i]
printfn "" 

let rec multiplylist inp times =
    if times = 1 then inp
    else inp @ multiplylist inp (times - 1)

let part2 from until=
    FFT2 100 (inp |> Seq.toArray) 10000 from until

let answer2 =
    let index = inp[0..6] |> List.fold (fun tot num -> tot * 10 + num) 0
    part2 index 8 

printf "Answer2: "
for i in 0..7 do
    printf "%d" answer2[i]
printfn "" 

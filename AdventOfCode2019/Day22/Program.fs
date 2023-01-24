open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let day = "E:\\develop\\advent-of-code-input\\2019\\Day22.txt"

let inp1 = 
    File.ReadLines ex1 |>
    Seq.toArray

let inp2 = 
    File.ReadLines day |>
    Seq.toArray

//let newCards cnt = [| for i in 0 .. cnt-1 -> i |]

let rec nextposition (x:int64) (i:int) (max:int64) (input:string array) =
    if i = input.Length then x
    else
        let s = input[i]
        let nX = 
            if s = "deal into new stack" then max - ( x + 1L ) 
            else
                let sp = s.Split(' ')
                if sp[0] = "cut" then 
                    let cut = int64 sp[1]
                    if cut > 0 then 
                        if cut < x then x - cut
                        else x + (max - cut)
                    else
                        if max + cut > x then x - cut
                        else x - (max + cut)
                else
                    let inc = int64 sp[3]
                    (x * inc) % max
        nextposition nX (i+1) max input


printfn "example %d" (nextposition 9L 0 10L inp1)
printfn "example %d" (nextposition 2019L 0 10007L inp2)

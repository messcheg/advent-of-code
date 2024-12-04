open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let day = "E:\\develop\\advent-of-code-input\\2019\\Day22.txt"

let inp1 = 
    File.ReadLines ex1 |>
    Seq.toArray

let inp2 = 
    File.ReadLines day |>
    Seq.toArray

let rec reverseMod (x:bigint) (inc:bigint) max = 
    if x % inc = (bigint 0) then x / inc
    else reverseMod (x+max) inc max

// instead of immediately calculating x, we'll only calculate the factors of ax + b
let rec nextposition (reverse:bool) (a:bigint) (b:bigint) (i:int) (max:bigint) (input:string array) =
    if (not reverse) && i = input.Length then (a,b)
    elif reverse && i = -1 then (a,b)
    else
        let s = input[i]
        let (nA, nB) = 
            if s = "deal into new stack" then  (-a, -b + max - (bigint 1L) ) 
            else
                let sp = s.Split(' ')
                if sp[0] = "cut" then 
                    let cut = bigint (int64 sp[1]) 
                    if reverse then (a, b + max + cut) else (a, b + max - cut)
                else
                    let inc = bigint (int64 sp[3])
                    if reverse then (reverseMod a inc max,reverseMod b inc max)                         
                    else (a * inc, b * inc)

        let nI = if reverse then i-1 else i+1
        nextposition reverse nA nB nI max input

let apply (a:bigint) (b:bigint) (x:bigint) (max:bigint) =
    let answ = ((a * x) + b) % max
    if answ >= (bigint 0) then answ else answ+max

let calculateEndpos (reverse:bool) (x:bigint) (max:bigint) (input:string array) =
    let (a,b) = nextposition reverse (bigint 1) (bigint 0) (if reverse then input.Length-1 else 0) max input
    apply a b x max

printfn "example %s" ((calculateEndpos false (bigint 9L) (bigint 10L) inp1).ToString())
printfn "real %s" ((calculateEndpos  false (bigint 2019L) (bigint 10007L) inp2).ToString())

printfn "example reverse %s" ((calculateEndpos  true (bigint 0L) (bigint 10L) inp1).ToString())
printfn "real reverse %s" ((calculateEndpos  true (bigint 3749L) (bigint 10007L) inp2).ToString())

let cardset = (bigint 119315717514047L)
let turns = (bigint 101741582076661L)

let combine (a:bigint,b:bigint) (xA,xB) (max:bigint) =
    let nA =  (a*xA) %  max
    let nB = (b + (a*xB)%max)%max
    (nA, nB)

let rec combineMultiple (count:bigint) (xA:bigint) (xB:bigint) (a:bigint) (b:bigint) (max:bigint) = 
    if count = bigint 0L then (xA,xB)
    else
        let (nXA,nXB) = if (count % bigint 2L) > bigint 0 then combine (a,b) (xA,xB) max else (xA,xB)
        let (nA,nB) = combine (a,b) (a,b) max
        let nCnt = count / bigint 2L
        combineMultiple nCnt nXA nXB nA nB max

let performMultipleTerms (count:bigint) (reverse:bool) (x:bigint) (max:bigint) (input:string array) =
    let (a,b) = nextposition reverse (bigint 1) (bigint 0) (if reverse then input.Length-1 else 0) max input
    let (cA, cB) = combineMultiple count (bigint 1) (bigint 0) a b max
    apply cA cB x max


let answer2 = performMultipleTerms turns true (bigint 2020L) cardset inp2

// check if the answer indeed end-up in position 2020
let check = performMultipleTerms turns false answer2 cardset inp2
printfn "check: %s" (check.ToString())

printfn "answer2: %s" (answer2.ToString())
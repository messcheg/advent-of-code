open System.IO
open System

let fExm1 = "..\\..\\..\\example1_input.txt"
let fExm2 = "..\\..\\..\\example2_input.txt"
let fReal = "..\\..\\..\\real_input.txt"

let input =
    File.ReadLines fReal |>
    Seq.map(fun s-> 
        s[1..s.Length - 2].Split(", ") |> 
        Array.map(fun s1 ->  Int32.Parse s1[2..])) |>
    Seq.map(fun ar -> (ar[0], ar[1], ar[2]))

let X (x,_,_) = x
let Y (_,y,_) = y
let Z (_,_,z) = z

let dif a b =
    if a > b then -1
    elif  a < b then 1
    else 0

let applyStep ar =
    ar |> 
    Array.map (fun ((px,py,pz),(vx, vy,vz)) ->
        let (vx1, vy1, vz1) =
            Array.fold (fun (vx3, vy3, vz3) ((x1,y1,z1),_) ->
                (vx3 + dif px x1, vy3 + dif py y1, vz3 + dif pz z1)) (vx , vy ,vz) ar
        ((px + vx1, py + vy1, pz + vz1), (vx1, vy1, vz1))
        )

let addV0 ar =
    ar |> Seq.map (fun a -> (a,(0,0,0))) |> Seq.toArray

let rec applysteps ar cnt =
    if cnt <= 0 then ar
    else applysteps (applyStep ar) (cnt - 1)

let test1 =
    applysteps (addV0 input) 1000

let printpos ar =
    for ((px,py,pz), (vx,vy,vz) ) in ar do  
        printfn "pos(%d, %d, %d) vel(%d,%d,%d)" px py pz vx vy vz

printpos test1

let abstot (x, y, z) =
    (abs x) + (abs y) + (abs z) 

let energy ar =
    ar |>
    Array.map (fun (p,v) -> (abstot p) * (abstot v)) |>
    Array.sum

let answer1 = energy test1

printfn "Answer1: %d" answer1
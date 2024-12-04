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

let rec applyuntilallOnce0 ar cnt (cx,cy,cz)=
    let cnt1 = cnt + 1L 
    let ar1 = applyStep ar
    let cx1 = 
        if cx > 0L then cx 
        elif ar1 |> Array.forall (fun (_,(vx,vy,vz)) -> vx = 0) then cnt1
        else 0L 
    let cy1 = 
        if cy > 0L then cy 
        elif ar1 |> Array.forall (fun (_,(vx,vy,vz)) -> vy = 0) then cnt1
        else 0L 
    let cz1 = 
        if cz > 0L then cz 
        elif ar1 |> Array.forall (fun (_,(vx,vy,vz)) -> vz = 0) then cnt1
        else 0L 
    if cx1 > 0L && cy1 > 0L && cz1 > 0L then (cx1, cy1, cz1)   
    else applyuntilallOnce0 ar1 cnt1 (cx1, cy1, cz1)

let (tx, ty,tz) =
    applyuntilallOnce0 (addV0 input) 0L (0,0,0)

printfn "Answer2 x,y,z: %d %d %d" tx ty tz

let rec GCD a b = 
    if b = 0L then a
    else GCD b (a % b)

let LCM x y z =
    x * y * z / (GCD (GCD x y) z)

printfn "Answer2: %d" (LCM tx ty tz)
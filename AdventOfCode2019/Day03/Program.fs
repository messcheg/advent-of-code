open System.IO
open System

// let filename = "..\\..\\..\\example_input1.txt"
let filename = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines(filename) |>
    Seq.map( fun s -> s.Split(','))

let inp0 = Seq.item 0 inp
let inp1 = Seq.item 1 inp

let points (inArr : string[]) =
    let mutable p1 = (0,0)
    let mutable reslt = [| |]
    for instr in inArr do
        let p2 = 
            match ( instr[0], Int32.Parse instr[1..((String.length instr)-1)], p1 ) with
               | (c, i, (x,y)) when c = 'U' -> (x, y + i)
               | (c, i, (x,y)) when c = 'D' -> (x, y - i)
               | (c, i, (x,y)) when c = 'R' -> (x + i, y)
               | (c, i, (x,y)) when c = 'L' -> (x - i, y)
               | _ -> (0,0)
        reslt <- (reslt |> Array.insertAt reslt.Length p2) 
        p1 <- p2
    reslt 
 
let between a a1 a2 =
    (a1 < a2 && a1 <= a && a <= a2) ||
    (a1 > a2 && a1 >= a && a >= a2)

let overlap a1 a2 b1 b2 =
    a1 < a2  
      && ( b1 < b2 && a1 <= b2 && b1 <= a2
          || b2 < b1 && a1 <= b1 && b2 <= a2 )
    ||
    a1 > a2  
    && ( b1 < b2 && a2 <= b2 && b1 <= a1
        || b2 < b1 && a2 <= b1 && b2 <= a1 )
  
let overlapsinseq a1 a2 b1 b2 =
    if a1 <= b1 then
        if a2 <= b2 then [| b1 .. a2 |] else [| b1 .. b2 |]
    else
        if a2 <= b2 then [| a1 .. a2 |] else [| a1 .. b2 |]

let getOverlaps a1 a2 b1 b2 =
    if a1 < a2 then 
        if b1 < b2 then overlapsinseq a1 a2 b1 b2
        else overlapsinseq a1 a2 b2 b1
    else
        if b1 < b2 then overlapsinseq a2 a1 b1 b2
        else overlapsinseq a2 a1 b2 b1

let intersect (x0:int, y0:int) (x1, y1) (p0, q0) (p1, q1) step1 step2 =
    let steps = 
        step1 + step2 + Math.Abs(q0 - y0) + Math.Abs(p0 - x0)
    let reslt = 
        if x0 = x1 && q0 = q1 && (between x0 p0 p1) && (between q0 y0 y1) then
            [|((x0, q0), steps) |] 
        elif y0 = y1 && p0 = p1 && (between p0 x0 x1) && (between y0 q0 q1) then    
            [|((p0, y0), steps )|] 
        elif p0 = p1 && x0 = x1 && p0 = x0 && (overlap y0 y1 q0 q1) then
            getOverlaps y0 y1 q0 q1 |> Array.map (fun s -> ((x0, s), steps))
        elif q0 = q1 && y0 = y1 && q0 = y0 && (overlap x0 x1 p0 p1) then
            getOverlaps x0 x1 p0 p1 |> Array.map (fun s -> ((s, y0), steps))
        else [| |]
    reslt

let steps (x1:int, y1:int) (x2, y2) =
    if x1 = x2 then Math.Abs (y2 - y1)
    else Math.Abs (x2 - x1)
 
let intersections arr1 arr2 =
    let mutable reslt = [| |]
    let mutable p1 = (0, 0)
    let mutable step1 = 0
    for s1 in arr1 do
        let mutable p2 = (0, 0)
        let mutable step2 = 0
        for s2 in arr2 do
            reslt <- Array.insertManyAt reslt.Length (intersect p1 s1 p2 s2 step1 step2) reslt
            step2 <- step2 + steps p2 s2
            p2 <- s2
        step1 <- step1 + steps p1 s1
        p1 <- s1
    reslt

let manhattandistance arr = 
    Array.map (fun ((x : int, y : int), z : int) -> (Math.Abs x) + (Math.Abs y)) arr

let combinedpathlen arr = 
    Array.map (fun ((x : int, y : int), z : int) -> z) arr

let getintersections =
    intersections (points inp0) (points inp1)

let answer1 = 
   manhattandistance getintersections |>
   Array.distinct |> 
   Array.filter (fun s -> s <> 0) |>
   Array.min
    
let answer2 = 
   combinedpathlen getintersections |>
   Array.distinct |> 
   Array.filter (fun s -> s <> 0) |>
   Array.min

printfn "Answer1: %d" answer1
printfn "Answer2: %d" answer2

open IntcodeMachine.Intcode
open System
open System.Threading

let filename = "..\\..\\..\\real_input.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let (north, south, west, east) = (1L,2L,3L,4L)

let otherdirection d =
    match d with
        | a when a = north -> south
        | a when a = south -> north
        | a when a = east -> west
        | a when a = west -> east
        | _ -> north
        
let (wall, moved, movedOnOxSys) = (0L, 1L, 2L)

let rec navigateTo path progstate =
    path |>
    Seq.fold (fun (_,ps) d -> runNextStep d ps) (-1L,progstate)

let locationOf path =
    let x = path |> Seq.map (fun d -> if d = east then 1 elif d = west then -1 else 0) |> Seq.sum
    let y = path |> Seq.map (fun d -> if d = north then 1 elif d = south then -1 else 0) |> Seq.sum
    (x,y)

let rec performstep (shortlist: (int * int64 * int64 list) list)  visited (currentpath: int64 list) progstate =
    if shortlist.Length = 0 then (true, (locationOf currentpath), visited, 0, currentpath, shortlist, progstate) 
    else
        let (costs, direction, path) = Seq.head shortlist   
        let path1 = path @ [direction]
        let location = locationOf path1
        if (Seq.exists (fun ((x1, y1),_,_) -> x1 = fst location && y1 = snd location ) visited) then
            performstep (List.tail shortlist) visited currentpath progstate 
        else    
            let overlapping =
                Seq.map2 (fun a b -> if a = b then a else -1L) path1 currentpath |>
                Seq.filter (fun a -> a > -1L) |>
                Seq.toList
            let pathback = 
                currentpath[(List.length overlapping)..] |> 
                List.rev |> 
                List.map(fun d -> otherdirection d)
            let pathforward = path1[(List.length overlapping)..]           
            let (material, progstate1) = navigateTo (pathback @ pathforward) progstate
            if material = wall then 
                performstep (List.tail shortlist) (visited @ [(location, true, costs)]) path progstate1 
            else  
                (material = movedOnOxSys,location, visited, costs, path1, (List.tail shortlist), progstate1)
    
     
let rec getShortestPath (x,y) visited costs currentpath (shortlist: (int * int64 * int64 list) list)  progstate =
    let visited1 = visited @ [((x,y), false ,costs)]
    let targets = [south; north; west; east]        
    let shortlist1 = 
        (shortlist @ (targets |> List.map (fun d -> (costs + 1, d, currentpath)))) |> 
        List.sortBy (fun (c,_,_) -> c )
    let (ready, loc1, vis1, cost1, path1, shrtl1, prog1) = performstep shortlist1 visited1 currentpath progstate 
    if ready then (cost1, (loc1, vis1, cost1, path1, shrtl1, prog1))
    else getShortestPath loc1 vis1 cost1 path1 shrtl1 prog1

let findOgygenSystem =
    let (outval, progstate) = runFirstStep north (Array.copy prog1)
    let (_, progstate1) = 
        if outval = 0L then (outval, progstate)
        else runNextStep south progstate
    getShortestPath (0,0) [] 0 [] [] progstate1 
 
let answer1 = fst findOgygenSystem 

let printfield (loc1, vis1, cost1, path1, shrtl1, prog1) =
    Console.Clear()
    let xmin = vis1 |> List.map (fun ((x,_),_,_) -> x) |> List.min
    let ymin = vis1 |> List.map (fun ((_,y),_,_) -> y) |> List.min
    let ymax = vis1 |> List.map (fun ((_,y),_,_) -> y) |> List.max
    for ((x,y),isWall,_) in vis1 do 
        Console.CursorTop <- ymax - y
        Console.CursorLeft <- x - xmin
        Console.Write (if isWall then "#" else ".")
    Console.SetCursorPosition( 0, (ymax + 1 - ymin))

 
printfield (snd findOgygenSystem)
printfn "Answer1: %d" answer1

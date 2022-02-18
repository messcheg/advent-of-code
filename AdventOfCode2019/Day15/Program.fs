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

    
let (wall, moved, movedOnOxSys, flooded ,ready, start) = (0L, 1L, 2L, 3L, 4L, 5L)

let rec navigateTo path progstate =
    path |>
    Seq.fold (fun (_,ps) d -> runNextStep d ps) (-1L,progstate)

let locationOf path =
    let x = path |> Seq.map (fun d -> if d = east then 1 elif d = west then -1 else 0) |> Seq.sum
    let y = path |> Seq.map (fun d -> if d = north then 1 elif d = south then -1 else 0) |> Seq.sum
    (x,y)

let backtrack path = 
    path |> 
    List.rev |> 
    List.map(fun d -> otherdirection d)
   
let getPathFromAtoB (pathFrom: int64 list) (pathTo: int64 list)= 
    let overlap =
        Seq.map2 (fun a b -> if a = b then a else -1L) pathTo pathFrom |>
        Seq.takeWhile(fun a -> a > -1L) |>
        Seq.length
    let pathback = backtrack pathFrom[overlap..]  
    let pathforward = pathTo[overlap..]
    pathback @ pathforward

let rec performstep (shortlist: (int * int64 * int64 list) list)  visited (currentpath: int64 list) progstate =
    if shortlist.Length = 0 then (ready, (locationOf currentpath), visited, 0, currentpath, shortlist, progstate) 
    else
        let (costs, direction, path) = Seq.head shortlist   
        let path1 = path @ [direction]
        let location = locationOf path1
        if (Seq.exists (fun ((x1, y1),_,_) -> x1 = fst location && y1 = snd location ) visited) then
            performstep (List.tail shortlist) visited currentpath progstate 
        else    
            let pathAtoB = getPathFromAtoB currentpath path1
            let (material, progstate1) = navigateTo pathAtoB progstate
            if material = wall then 
                performstep (List.tail shortlist) (visited @ [(location, wall, costs)]) path progstate1 
            else  
                (material ,location, visited, costs, path1, (List.tail shortlist), progstate1)
    
     
let rec getShortestPath (x,y) visited material costs currentpath (shortlist: (int * int64 * int64 list) list)  progstate =
    let visited1 = visited @ [((x,y), material ,costs)]
    let targets = [north; east; south; west]        
    let shortlist1 = 
        (shortlist @ (targets |> List.map (fun d -> (costs + 1, d, currentpath)))) |> 
        List.sortBy (fun (c,_,_) -> c )
    let (mat1, loc1, vis1, cost1, path1, shrtl1, prog1) = performstep shortlist1 visited1 currentpath progstate 
    if mat1 = ready then (vis1, (loc1, cost1, path1, shrtl1, prog1))
    else getShortestPath loc1 vis1 mat1 cost1 path1 shrtl1 prog1

let findOgygenSystem =
    getShortestPath (0,0) [] start 0 [] [] (initalprogstate (Array.copy prog1)) 
 
let answer1 = 
    (fst findOgygenSystem) |> 
    Seq.filter (fun (_,a,_) -> a = movedOnOxSys ) |> 
    Seq.map (fun (_,_,c) -> c) |>
    Seq.head

let printfield vis1 =
    Console.Clear()
    let xmin = vis1 |> List.map (fun ((x,_),_,_) -> x) |> List.min
    let ymin = vis1 |> List.map (fun ((_,y),_,_) -> y) |> List.min
    let ymax = vis1 |> List.map (fun ((_,y),_,_) -> y) |> List.max
    for ((x,y),material,_) in vis1 do 
        Console.CursorTop <- ymax - y
        Console.CursorLeft <- x - xmin
        Console.Write (
            if material = wall then "#" 
            elif material = moved then "." 
            elif material = movedOnOxSys then "O" 
            elif material = flooded then "~"
            elif material = start then "S"
            else " ")
    Console.SetCursorPosition( 0, (ymax + 1 - ymin))

printfield (fst findOgygenSystem)
printfn "Answer1: %d" answer1

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
    Seq.fold (fun (_,ps) d -> runNextStep d ps) ((-1L,false),progstate)

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

let rec performstep (shortlist: (int * int64 * int64 list) list)  (visited : ((int*int)*int64*int*int64 list) list) (currentpath: int64 list) progstate =
    if shortlist.Length = 0 then (ready, (locationOf currentpath), visited, 0, currentpath, shortlist, progstate) 
    else
        let (costs, direction, path) = Seq.head shortlist   
        let path1 = path @ [direction]
        let location = locationOf path1
        if (Seq.exists (fun ((x1, y1),_,_, _) -> x1 = fst location && y1 = snd location ) visited) then
            performstep (List.tail shortlist) visited currentpath progstate 
        else    
            let pathAtoB = getPathFromAtoB currentpath path1
            let ((material,_), progstate1) = navigateTo pathAtoB progstate
            if material = wall then 
                performstep (List.tail shortlist) (visited @ [(location, wall, costs, path1)]) path progstate1 
            else  
                (material ,location, visited, costs, path1, (List.tail shortlist), progstate1)
    
     
let rec getShortestPath (x,y) visited material costs currentpath (shortlist: (int * int64 * int64 list) list)  progstate =
    let visited1 = visited @ [((x,y), material ,costs, currentpath)]
    let targets = [north; east; south; west]        
    let shortlist1 = 
        (shortlist @ (targets |> List.map (fun d -> (costs + 1, d, currentpath)))) |> 
        List.sortBy (fun (c,_,_) -> c )
    let (mat1, loc1, vis1, cost1, path1, shrtl1, prog1) = performstep shortlist1 visited1 currentpath progstate 
    if mat1 = ready then (vis1, (loc1, cost1, path1, shrtl1, prog1))
    else getShortestPath loc1 vis1 mat1 cost1 path1 shrtl1 prog1

let findOgygenSystem =
    getShortestPath (0,0) [] start 0 [] [] (initalprogstate (Array.copy prog1)) 

let directtions = dict [(north,(0,1)); (south,(0,-1)); (east, (1,0)); (west, (-1,0))]

let winningpath = 
    dict ((fst findOgygenSystem) |> 
    List.filter (fun (_,a,_,_) -> a = movedOnOxSys ) |> 
    List.map (fun (_,_,_,d) -> d) |>
    List.head |>
    List.mapFold(fun (x,y) dir -> (((x + fst (directtions[dir]), y + snd directtions[dir]), dir),(x + fst (directtions[dir]), y + snd directtions[dir]))) (0,0) |>
    fst |>
    List.toArray)
    
let answer1 = 
    (fst findOgygenSystem) |> 
    Seq.filter (fun (_,a,_,_) -> a = movedOnOxSys ) |> 
    Seq.map (fun (_,_,c,_) -> c) |>
    Seq.head

    
let printfield vis1 =
    Console.Clear()
    let xmin = vis1 |> List.map (fun ((x,_),_,_,_) -> x) |> List.min
    let ymin = vis1 |> List.map (fun ((_,y),_,_,_) -> y) |> List.min
    let ymax = vis1 |> List.map (fun ((_,y),_,_,_) -> y) |> List.max
    for ((x,y),material,_,_) in vis1 do 
        let oldcolor = Console.ForegroundColor
        let direction = 
            if winningpath.ContainsKey (x,y) then 
                winningpath[(x,y)]
            else
                0L
        if direction > 0L && material = moved then Console.ForegroundColor <- ConsoleColor.Yellow
        elif material = flooded then Console.ForegroundColor <- ConsoleColor.Blue
        Console.CursorTop <- ymax - y
        Console.CursorLeft <- x - xmin
        Console.Write (
            if material = wall then "#" 
            elif material = moved then [|"."; "^";"v";"<";">"|][int direction]
            elif material = movedOnOxSys then "O" 
            elif material = flooded then "~"
            elif material = start then "S"
            else " ")
        Console.ForegroundColor <- oldcolor
    Console.SetCursorPosition( 0, (ymax + 1 - ymin))

printfield (fst findOgygenSystem)
printfn "Answer1: %d" answer1

let canBeFlooded visited (x,y) =
    Seq.exists (fun ((x1, y1),material,_,_) -> x1 = x && y1 = y && (material = moved || material = start)) visited

let rec getnewworklist worklist visited newwork =
    if List.isEmpty worklist then (visited, newwork)
    else
        let (x,y) = List.head worklist
        let up = if canBeFlooded visited (x, y+1) then [(x, y+1)] else []
        let down = if canBeFlooded visited (x, y-1) then [(x, y-1)] else []
        let right = if canBeFlooded visited (x+1, y) then [(x+1, y)] else []
        let left = if canBeFlooded visited (x-1, y) then [(x-1, y)] else []
        let sublist = up @ down @ right @ left 
        let newvisit = 
            visited |> 
            List.map (fun ((x1, y1),m,c,p) -> 
                if (set sublist).Contains (x1,y1) then ((x1,y1),flooded,c,p)
                else ((x1,y1),m,c,p))
        getnewworklist (List.tail worklist) newvisit (newwork @ sublist)

let rec flood visited worklist count =
    let (vis1, wl1) = getnewworklist worklist visited []
    if List.isEmpty wl1 then (count, vis1)
    else flood vis1 wl1 (count + 1) 

let oxygenlocation = 
    fst findOgygenSystem |>
    List.filter (fun (_,material,_,_) -> material = movedOnOxSys ) |>
    List.map (fun (loc,_,_,_) -> loc ) |>
    Seq.head

let part2 = flood (fst findOgygenSystem) [oxygenlocation] 0

printfield (snd part2)
printfn "Answer2: %d" (fst part2)

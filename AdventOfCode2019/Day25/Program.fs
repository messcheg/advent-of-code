open System.IO
open IntcodeMachine.Intcode

let filename = "E:\\develop\\advent-of-code-input\\2019\\Day25.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let makeInput (command:string) =
    command + "\n" |>
    Seq.map(fun a -> int64 (int a)) |>
    Seq.toArray

let rec nextLine built pst command=
    let pst1 = runNextStepArr (makeInput command) pst
    let nextChar = ((char (fst (fst pst1))).ToString())
    if nextChar = "\n" then (built, pst1)
    else nextLine (built + nextChar) (snd pst1) ""


let doors = "Doors here lead:"
let items = "Items here:"
let cmd = "Command?"

let rec collectItems collected pst =
    let (nextItem, pst1) = nextLine "" pst ""
    if nextItem.StartsWith('-') then collectItems (Array.append collected [|nextItem[2..]|]) (snd pst1)
    else (collected, pst1)
    

let rec goToRoom pst collected command = 
    let (line, pst1) = nextLine "" pst command 
    printfn "%s" line
    if line.StartsWith("=") then
        let map1 = collected |> Map.add "name" [|line|]
        goToRoom (snd pst1) map1 ""
    elif line = cmd then 
        (collected, pst1)
    elif line = doors then
        let (colDr, pst2) = collectItems [||] (snd pst1)
        let map1 = collected |> Map.add "doors" colDr
        goToRoom (snd pst2) map1 ""
    elif line = items then
        let (colIt, pst2) = collectItems [||] (snd pst1)
        let map1 = collected |> Map.add "items" colIt
        goToRoom (snd pst2) map1 ""
    else
        goToRoom (snd pst1) collected ""

let rec Take pst item =
    let command = 
        if item = "" then "" 
        else
            printfn "take %s" item
            "take " + item
    let (line, pst1) = nextLine "" pst command
    printfn "%s" line
    if line = cmd then pst1
    else Take (snd pst1) ""

let rec PlayGame pst command = 
    let (line, pst1) = nextLine "" pst command 
    printfn "%s" line
    if line = cmd then 
        let txt = System.Console.ReadLine()
        if txt = "exit" then pst1
        else PlayGame (snd pst1) txt
    else
        PlayGame (snd pst1) ""

     
// PlayGame (initalprogstate (Array.copy prog1)) "" |> ignore

let printallreadablecharacters (prog:int64 array) =
    prog |>
    Seq.filter (fun x -> x >= 32 && x < 255) |>
    Seq.map (fun x -> int x) |>
    Seq.map (fun x -> char x) |>
    Seq.iter (fun x -> printf "%s" (x.ToString()))

//printallreadablecharacters (Array.copy prog1)

let oposite a =
    if a = "north" then "south"
    elif a = "south" then "north"
    elif a = "left" then "right"
    elif a = "right" then "left"
    else ""

let rec AddItems items allItems currentroom = 
    if Array.isEmpty items then allItems
    else
        let currentitem = items[0]
        let restitems = items[1..]
        let allitems1 = Map.add currentitem currentroom allItems
        AddItems restitems allitems1 currentroom

//make a map of the rooms collecting
// - all roomnames
// - all itemnames
// - adistance table
// - inaccessable rooms

(*
let rec GetRoomMap pst allrooms allitems visited currentpath direction =
    let (currentRoom, pst1) = goToRoom pst Map.empty direction
    let roomName = currentRoom["name"]
    if visited.ContainsKey(roomName) then
        let contraDirection = oposite direction
        let (previousroom, pst2) = gotoRoom pst1 Map.empty contraDirection
        (allrooms, allitems, visited, currentpath[..^1], pst2)
    else
        let visited1 = Map.add  roomName currentpath visited
        let allrooms1 = Map.add allrooms roomName currentRoom
        let allitems1 = currentRoom["items"] |>
                        Array.iter
        let rec checkAllDoors doorsToGo chPst chAllrooms chAllitems chVisited chCurrentPath =
            if Array.length doorsToGo > 0 then
                let nextDoor = doorsToGo[0]
                let restDoors = doorsToGo[1..]
                let biggerPath = Array.append chCurrentPath [|nextDoor|]
                let (chAllrooms1,chAllitems1, chVisited1, chCurrentPath1, chPst1) = GetRoomMap chPst chAllrooms chAllitems chVisited biggerPath nextDoor
                checkAllDoors restDoors chPst1 chAllrooms1 chAllitems1 chVisited1 chCurrentPath1
            else
                chPst

*)
    
let instructions =
    [|  // Hull Breach
        "west"; 
        // Hot Chocolate Fountain
        // - infinite loop
        "south";
        // Storage 
        // - giant electromagnet
        "south";
        // Arcade
        "south";
        // Corridor
        // - asterisk
        "take asterisk";
        "north";
        // Arcade
        "north";
        // Storage 
        // - giant electromagnet
        "east";
        // Warp Drive Maintenance
        "south";
        // Gift Wrapping Center
        "north";
        // Warp Drive Maintenance
        "west";
        // Storage 
        // - giant electromagnet
        "north";
        // Hot Chocolate Fountain
        // - infinite loop
        "west";
        // Passages
        // - photons
        "take photons";
        // Hull Breach
        "west";
        // Hot Chocolate Fountain
        // - infinite loop
        "west";
        // Passages
        "west";
        // Stables 
        "west";
        // Holodeck 
        // - dark matter
        "take dark matter";
        "east";
        // Stables 
        "south";
        // Navigation
        // - fixed point
        "take fixed point";
        "west";
        // Hallway
        // - food ration
        "take food ration";
        "east";
        // Navigation
        "north";
        // Stables 
        "east";
        // Passages
        "south";
        // Science Lab
        // - astronaut ice cream
        "take astronaut ice cream";
        "west";
        // Sick Bay
        // - molten lava
        "take molten lava";
        "west"; "west"; "south";
        // Science Lab
        "south";
        // Crew Quarters
        // - polygon
        "take polygon";
        "east";
        // Observatory 
        // - easter egg
        "take easter egg";
        "north";
        // Kitchen 
        // - escape pod
        "take escape pod";
        "west"; "west"; "south"; "south"; "east";
        "east";
        // Engineering 
        // - weather machine
        "take weather machine";
        "north";
        "drop asterisk"; 
        "drop food ration";
        
        "drop polygon";
        "north";
        "take polygon";
        "drop fixed point";
        "north";
        "take fixed point";
        "drop molten lava";
        "north";
        "take molten lava";
        "west"; "west"; "south"; "south"; "east"; "east"; "north";
        "drop astronaut ice cream";
        "north";
        "take astronaut ice cream";
        "drop easter egg";
        "north";
        "take easter egg";
        "drop photons";
        "north";
        "take photons";
        "west"; "west"; "south"; "south"; "east"; "east"; "north";
        "drop escape pod";
        "north";
        "drop polygon";
        "north";
        "drop fixed point";
        "north";
        "drop molten lava";
        "north";
        "drop astronaut ice cream";
        "north";
        "drop easter egg";
        "north";
        "drop photons";
        "north";
    |]


let rec PlayInstr pst commands command =
    let (line, pst1) = nextLine "" pst command 
    printfn "%s" line
    if line = cmd then
        if Array.length commands > 0 then
            let first = commands |> Array.head
            let rest = commands |> Array.tail
            printfn "%s" first
            PlayInstr (snd pst1) rest first
        else PlayGame (snd pst1) "inv"
    else
        PlayInstr (snd pst1) commands ""

     
PlayInstr (initalprogstate (Array.copy prog1)) instructions "" |> ignore

open IntcodeMachine.Intcode
open System
open System.Threading

let filename = "..\\..\\..\\real_input.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let rec determineGamefield program (fields:Map<int64*int64,int>) arrExtra pc pbase =
        let (x, pc1, (_, pbase1), fin1, (prog1, extra1), _) = 
            doruntilout program arrExtra [| |] pc pbase
        if fin1 then fields
        else
            let (y, pc2, (_, pbase2), _, (prog2, extra2), _) = 
                doruntilout prog1 extra1 [| |] pc1 pbase1
            let (tileID, pc3, (_, pbase3), _, (prog3, extra3), _) = 
                doruntilout prog2 extra2 [| |] pc2 pbase2
            let containedKey = Map.containsKey (x,y) fields
            let fields1 = 
                if tileID = 0 && not containedKey then fields
                elif containedKey then Map.add (x,y) (int tileID) (Map.remove(x,y) fields)
                else Map.add (x,y) (int tileID) fields
            determineGamefield prog3 fields1 extra3 pc3 pbase3

let result1 = determineGamefield (Array.copy prog1) Map.empty [||] 0 0

let answer1 = result1 |> Map.values |> Seq.filter (fun x -> x = 2) |> Seq.length 

let printAnswer1 = 
    Console.Clear() |> ignore
    Console.WriteLine "" 
    for y in 0..25 do
       for x in 0..60 do
           if result1.ContainsKey (x,y) then 
                if result1[(x,y)] = 1 then printf "#"
                elif result1[(x,y)] = 2 then printf "*"
                elif result1[(x,y)] = 3 then printf "_"
                elif result1[(x,y)] = 4 then printf "O"
                else printf " "
           else printf " "
       printfn ""

    printfn "Answer1: %d" answer1

let insert2Quarters ar1 =
    Array.set ar1 0 2L
    ar1

let printTile x y tileid = 
    Console.CursorLeft <- int x
    Console.CursorTop <- int y + 1
    let prnt = " #*_O"
    Console.Write(sprintf "%c" prnt[int tileid])

let rec playGame program pdlpos blocks ballpos score (fields:Map<int64*int64,int>)arrExtra pc pbase =
    let joystick = (fun _ -> if pdlpos > ballpos then (true,-1L) elif pdlpos < ballpos then (true,1L) else (true,0L))
    let (x, pc1, (_, pbase1), fin1, (prog1, extra1), _) = 
        runFunInUntilOut program arrExtra joystick pc pbase
    if fin1 then score
    else
        let (y, pc2, (_, pbase2), _, (prog2, extra2), _) = 
            runFunInUntilOut prog1 extra1 joystick pc1 pbase1
        let (tileID, pc3, (_, pbase3), _, (prog3, extra3), _) = 
            runFunInUntilOut  prog2 extra2 joystick pc2 pbase2
        if x = -1 then
            Console.CursorLeft <- 0
            Console.CursorTop <- 0
            Console.Write(sprintf "Score: %d" tileID)
            playGame prog3 pdlpos blocks ballpos tileID fields extra3 pc3 pbase3
        else
            printTile x y tileID
            let containedKey = Map.containsKey (x,y) fields
            let fields1 = 
                if tileID = 0 && not containedKey then fields
                elif containedKey then Map.add (x,y) (int tileID) (Map.remove(x,y) fields)
                else Map.add (x,y) (int tileID) fields
            let pdlpos1 = if tileID = 3 then x else pdlpos
            let ballpos1 = if tileID = 4 then x else ballpos
            let (blocks1, less) = 
                if tileID = 2 && not containedKey then (blocks + 1, false)
                elif (tileID = 0 || tileID = 4)&& containedKey && fields[(x,y)] = 2 then (blocks - 1, true)
                else (blocks, false)
            if ballpos1 <> ballpos then Thread.Sleep 20
            playGame prog3 pdlpos1 blocks1 ballpos1 score fields1 extra3 pc3 pbase3

let answer2 =
    let prog2 = insert2Quarters (Array.copy prog1)
    playGame prog2 0 0 0 0 Map.empty [||] 0 0 

let printAnswer2 = 
    Console.Clear() |> ignore
    Console.CursorLeft <- 0
    Console.CursorTop <- 25
    Console.CursorVisible <- false
    printfn "Answer2: %d" answer2 
    Console.CursorVisible <- true
let runAll = 
    printAnswer1
    System.Threading.Thread.Sleep(2000)
    printAnswer2

runAll
open IntcodeMachine.Intcode

let filename = "..\\..\\..\\real_input.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let rec determineGamefield program (fields:Map<int64*int64,int>) arrExtra pc pbase =
        let (x, pc1, (_, pbase1), fin1, (prog1, extra1)) = 
            doruntilout program arrExtra [| |] pc pbase
        if fin1 then fields
        else
            let (y, pc2, (_, pbase2), _, (prog2, extra2)) = 
                doruntilout prog1 extra1 [| |] pc1 pbase1
            let (tileID, pc3, (_, pbase3), _, (prog3, extra3)) = 
                doruntilout prog2 extra2 [| |] pc2 pbase2
            let containedKey = Map.containsKey (x,y) fields
            let fields1 = 
                if tileID = 0 && not containedKey then fields
                elif containedKey then Map.add (x,y) (int tileID) (Map.remove(x,y) fields)
                else Map.add (x,y) (int tileID) fields
            determineGamefield prog3 fields1 extra3 pc3 pbase3

let result1 = determineGamefield (Array.copy prog1) Map.empty [||] 0 0

let answer1 = result1 |> Map.values |> Seq.filter (fun x -> x = 2) |> Seq.length 


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

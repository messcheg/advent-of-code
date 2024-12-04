open IntcodeMachine.Intcode

let filename = "..\\..\\..\\real_input.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let directions = [|(0,1);(1,0);(0,-1);(-1,0)|]

let rec determinePaintedTiles program tiles (x,y) dir arrExtra pc pbase=
        let containedkey = Map.containsKey (x,y) tiles 
        let color = if containedkey then tiles[(x,y)] else 0  
        let (color1, pc1, (_, pbase1), fin1, (prog1, extra1), _) = 
            doruntilout program arrExtra [| color |] pc pbase
        if fin1 then tiles
        else 
            let tiles1 = 
                if color = int color1 && containedkey then tiles
                elif containedkey then Map.add (x,y) (int color1) (Map.remove(x,y) tiles)
                else Map.add (x,y) (int color1) tiles
            let (direction, pc2, (_, pbase2), fin2, (prog2,extra2),_) =
                doruntilout prog1 extra1 [| |] pc1 pbase1
            if fin2 then tiles1
            else
                let dir1 =
                    if direction = 0 then 
                        if dir > 0 then (dir - 1) else 3
                    else
                        if dir < 3 then (dir + 1) else 0
                determinePaintedTiles prog2 tiles1 ( x + (fst directions[dir1]), y + (snd directions[dir1]) ) dir1 extra2 pc2 pbase2

let result1 = determinePaintedTiles (Array.copy prog1) Map.empty (0,0) 0 [||] 0 0

let answer1 = result1.Count

printfn "Answer1: %d" answer1

let result2 = determinePaintedTiles (Array.copy prog1) Map[((0,0),1)] (0,0) 0 [||] 0 0
let left2 = result2.Keys |> Seq.map(fun (x,_) -> x) |> Seq.min
let top2 = result2.Keys |> Seq.map(fun (_,y) -> y) |> Seq.max

printfn "Answer2:"
printfn ""

for j in 0..10 do
    for i in 0..50 do
        let x = left2 + i
        let y = top2 - j
        if result2.ContainsKey (x,y) && result2[(x,y)] = 1 then printf "#"
        else printf " "
    printfn ""

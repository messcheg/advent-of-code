open System.IO
open System

let ex1 = "..\\..\\..\\example_input3.txt"
let real1 = "..\\..\\..\\real_input.txt"

let inp fl = 
    File.ReadLines(fl) |>
    Seq.toArray

let rec getline i j x y w h c f =
    let posx = x + i * f
    let posy = y + j * f
    if posx < w && posy < h && posx >= 0 && posy >= 0 then
        Array2D.set c posy  posx  true
        [(posx, posy )] @ getline i j x y w h c (f + 1)
    else
        []

let linesofsight hight width y x =
    let check = Array2D.create hight width false
    let mutable lines = [||]
    for i in [0 .. max x (width - x)] do
        for j in [0 .. max y (hight - y)] do
            if (i > 0 || j > 0) && i + x < width && j + y < hight && not check[y+j, i+x] then
                lines <- Array.append lines [| getline i j x y width hight check 1|]
            if x - i > 0 && j + y < hight && not check[y+j, x-(i+1)] then
                lines <- Array.append lines [| getline -(i+1) j x y width hight check 1|]
            if i + x < width && y - j > 0 && not check[y-(j+1), x+i] then
                lines <- Array.append lines [| getline i -(j+1) x y width hight check 1|]
            if x - i > 0 && y - j > 0 && not check[y-(j+1), x-(i+1)] then
                lines <- Array.append lines [| getline -(i+1) -(j+1) x y width hight check 1|]
    lines

let printlines =
    for l in linesofsight (Array.length (inp ex1)) (String.length ((inp ex1)[0])) 2 2 do
        for ll in l do  
            printf "(%d,%d) " (fst ll) (snd ll)
        printfn ""



let maplines hight width y x =
    let lns = Array2D.create hight width 0
    let los = linesofsight hight width y x 
    let mutable a = 1
    for lo in los do
        for (i,j) in lo do
            lns[i,j] <- a
        a <- a + 1
    lns
 
let printmap mp = 
    for i in 0 .. Array2D.length1 mp - 1 do
        for j in 0 .. Array2D.length2 mp - 1 do
            printf " %d " mp[i,j]
        printfn ""
    
let countvisible (inpt : string[]) hight width y x =
    linesofsight hight width y x |>
    Seq.map (fun l -> 
        if l |> Seq.exists (fun (i , j) -> inpt[j][i] = '#') then 1 else 0) |>
    Seq.sum    

let losCounts fl =
    let inf = inp fl
    let hight = Array.length inf
    let width = String.length inf[0]
    let cnt = Array2D.create hight width 0
    for y in 0..(hight-1) do
        for x in 0..(width-1) do
            if inf[y][x] = '#' then
                cnt[y,x] <- countvisible inf hight width y x
                //printmap (maplines hight width y x )
                //printfn "---------------"
    cnt

//let loscEx1 = losCounts ex1 
let loscEx1 = losCounts real1 

for i in 0 .. Array2D.length1 loscEx1 - 1 do
    for j in 0 .. Array2D.length2 loscEx1 - 1 do
        printf "%d, " loscEx1[i,j]
    printfn ""
 
let d2max ar2 = 
    let mutable max = 0
    let mutable coordinates = (0,0) 
    for i in 0 .. Array2D.length1 ar2 - 1 do
        for j in 0 .. Array2D.length2 ar2 - 1 do
            if ar2[i,j] > max then
                max <- ar2[i,j]
                coordinates <- (i,j)
    (max, coordinates)

let answer1 = d2max loscEx1

printfn "answer1: %d" (fst answer1)

let vaporize ar3 =
    let (max, (y,x)) = d2max ar3
    let lines = linesofsight (Array2D.length1 ar3) (Array2D.length2 ar3) y x
    let astrs = 
        lines |> 
        Array.map (fun z -> z |> Seq.filter ( fun (i,j) -> ar3[j,i] > 0)) |> 
        Array.filter (fun z -> not (Seq.isEmpty z)) |>
        Array.sortBy (fun z -> 
                let (i,j) = Seq.head z  
                let angle = Math.Atan2( float (i - x), float (y - j))
                if angle >= 0 then angle
                else angle + Math.PI * 2.0) 
    let mutable cntr = 0
    let mutable lastone = (x,y)
    let mutable linestocheck = Array.toSeq astrs
    let mutable nextlines = List.empty
    while cntr < 200 do
        let current = Seq.head linestocheck
        lastone <- Seq.head current
        if not (Seq.isEmpty (Seq.tail current)) then nextlines <- nextlines @ [Seq.tail current]
        linestocheck <- Seq.tail linestocheck
        cntr <- cntr + 1
        if Seq.isEmpty linestocheck then
            linestocheck <- nextlines
            nextlines <- List.Empty
    lastone

let answer2 = vaporize loscEx1
    
printfn "answer2: %d"  (100 * (fst answer2) + (snd answer2))
    
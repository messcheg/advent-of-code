open IntcodeMachine.Intcode
open System
open System.Threading

let filename = "..\\..\\..\\IntcodeProg.txt"

let prog1 = IntcodeMachine.Intcode.inp filename

let rec initialfield ps = 
    let s = runNextStep 0 ps
    printf "%c" (char (fst(fst s)))
    if isFinished s then []
    else [fst (fst s)] @ initialfield (snd s)

let playfield = 
    System.Console.Clear()
    //System.Console.WindowHeight <- 60
    initialfield (initalprogstate (Array.copy prog1)) |>
    Seq.mapFold(fun s i -> ((char i, s), if i = 10L then s+1 else s)) 0 |>
    fst |>
    Seq.filter(fun (c,_) -> int c <> 10) |>
    Seq.groupBy(fun (_, s) -> s) |>
    Seq.sortBy(fun (k, _) -> k) |>
    Seq.map(fun (_, sq) -> sq |> Seq.map(fun (c,_)-> c) |> Seq.toArray) |>
    Seq.toArray

let getIntersections pf = 
    let lpf = Array.length pf
    let lr = Array.length pf[0]
    let mutable intersections = []
    for i = 1 to lpf - 2 do 
        for j = 1 to lr - 2 do
            if  pf[i][j] = '#' &&
                pf[i+1][j] = '#' &&
                pf[i-1][j] = '#' &&
                pf[i][j+1] = '#' &&
                pf[i][j-1] = '#' then intersections <- intersections @ [(i,j)]
    intersections
                                       
let printfield pf =
    System.Console.Clear()
    System.Console.ForegroundColor <- ConsoleColor.White
    //System.Console.WindowHeight <- 60
    let insec = getIntersections pf
    for i = 0 to pf.Length - 1 do
        for j = 0 to pf[i].Length - 1 do
            let c = if (insec |> List.contains (i,j)) then 'O' else pf[i][j]
            printf "%c" c
        printfn ""
    
printfield playfield

let answer1 = 
    getIntersections playfield |>
    Seq.map(fun (i,j) -> i*j) |>
    Seq.sum

printfn "Answer1 = %d" answer1

let numberofscaffolds pf = 
    pf |> 
    Seq.map(fun s -> 
        s |> 
        Seq.where(fun c -> c = '#' || c = '^') |> 
        Seq.length) |>
    Seq.sum 

let findVacuumcleaner pf = 
    let mutable ret = (0,0)
    for i = 0 to (pf |> Array.length) - 1 do
        let l = pf[i]
        for j = 0 to (l |> Array.length)- 1 do
            let c = l[j]
            if c = '^' || c = 'v' || c = '>' || c = '<' then ret <- (i,j)
    ret

let dirchar D = 
    if D = (-1,0) then '^'
    elif D = (0,1) then '>'
    elif D = (1,0) then 'v'
    elif D = (0,-1) then '<'
    else 'O' 
                    
let rec getpath1 pf (i,j) (path: int list) scafleft direction (cnt:int) =
    if scafleft = 0 then path @ [cnt]
    else
        System.Console.BackgroundColor <- ConsoleColor.Yellow
        System.Console.ForegroundColor <- ConsoleColor.DarkBlue
        Console.CursorLeft <- j
        Console.CursorTop <- i
        Console.Write (dirchar direction)
        let maxI = (Array.length pf) - 1
        let maxJ = (Array.length pf[0]) - 1
        let check (I,J) = J <= maxJ && J >= 0 && I <= maxI && I >= 0 && pf[I][J] = '#' 
        let dir (I,J) (dI,dJ) = (I+dI,J+dJ)
        let R (dI,dJ) = (dJ, 0 - dI)
        let L (dI,dJ) = (0 - dJ, dI)
        let newpos = dir (i,j) direction
        if check newpos then
            getpath1 pf newpos path (scafleft - 1) direction (cnt + 1) 
        else
            let posl = dir (i, j) (L direction)
            if check posl then
                getpath1 pf posl (path @ [cnt; (int 'L')]) (scafleft - 1) (L direction) 1
            else
                let posr = dir (i, j) (R direction)
                if check posr then
                    getpath1 pf posr (path @ [cnt; (int 'R')]) (scafleft - 1) (R direction) 1
                else
                    path @ [cnt]


let path pf = 
    getpath1 pf (findVacuumcleaner pf) [(int 'R')] ((numberofscaffolds pf) + (getIntersections pf |> Seq.length)) (0,1) 0
    
let isEqual (A:int[][]) (B:int[][]) =
    if (Array.length A) <> (Array.length B) then false
    else
        let mutable res = true
        for i = 0 to Array.length A - 1 do
            if A[i][0] <> B[i][0] || A[i][1] <> B[i][1] then res <- false
        res

let occurrences (sub:int[][]) (cmdlist:int[][]) =
    let mutable i = 0
    let mutable cnt = 0
    let lensub = Array.length sub
    while i <= Array.length cmdlist - lensub do
        if isEqual sub (cmdlist[i..i + lensub - 1]) then
            cnt <- cnt + 1
            i <- i + lensub
        else
            i <- i + 1
    cnt

let memoryproblem (funcs, used) =
    (List.length funcs) * 2 - 1 > 20 ||
    (List.length used) > 3 ||
    used |> List.exists(fun l -> (Seq.length l) * 2 - 1 > 20)

let rec getfit (lookup: Collections.Generic.IDictionary<(int*int),(int*int[][]*int*int)[]>) used funcs (cmdlist:int[][]) =
    if Array.isEmpty cmdlist then 
        [(funcs, used)]
    elif memoryproblem(funcs, used) then []
    else 
        let mutable results = []
        let cur = cmdlist[0]
        let matchlist = lookup[(cur[0],cur[1])] 
        for (_,l,_,id) in matchlist do  
            if isEqual l (cmdlist[0..(Array.length l) - 1]) then 
                let newused =
                    if used |> List.exists (fun x -> isEqual x l) then used
                    else used @ [l]
                results <- results @ (getfit lookup newused (funcs @ [id]) cmdlist[(Array.length l)..] )
        results

let rec getbestfit lookup cmdlist = 
    let fits = getfit lookup [] [] cmdlist 
    let best =
        fits |> List.filter(fun (_,R) -> Seq.length R = 3 )
    let verybest = List.head best
    verybest

let createprogram s = 
    let cmdlist:int[][] = 
        s |> 
        List.toArray |>
        Array.chunkBySize 2 
    let mutable patrns = []
    let arlen = cmdlist |> Array.length
    for i = 0 to arlen - 2 do
        for j = i + 1 to arlen - 1 do
            let sub:int[][] = cmdlist[i..j]
            if not (patrns |> List.exists (fun (a,_) -> isEqual a sub )) then
                let occ = occurrences sub cmdlist
                if (occ > 1) then  patrns <- patrns @ [(sub, occ)]
    let withId =
        patrns |>
        List.mapi(fun a (f,c) -> ((Array.length f) * (c - 1) - c , f, c, a))
 
    let lookup =
        dict(withId |>
        List.groupBy (fun (_,f,_,_) -> (f[0][0],f[0][1])) |>
        List.map(fun (k,fl) -> (k,fl |> List.sortByDescending(fun (gain,_,_,_) -> gain )|>List.toArray)))
    let bestfit = getbestfit lookup cmdlist 
    
    let progf = 
        bestfit |> fst |> 
        List.mapFold(fun (F, map) i -> 
            let num = map |> List.filter(fun (_,j) -> i = j) 
            if not (List.isEmpty num) then
                (num |> List.head |> fst, (F, map))
            else
                (F, (F+1, map @ [(F,i)]))
            ) (int 'A', []) |> fst
    let prog =
        ([progf |> List.toArray] @ 
            (bestfit |> 
                snd |> 
                List.map(fun ar -> ar |> Array.concat ))) |>
        List.map(fun ar -> 
            let nums c =
                let c1 = (c % 10L) + int64 '0'
                let c2 = (c / 10L) + int64 '0'
                if c > 9 then [|c2;c1|]
                else [|c1|]
            let ar1 = 
                ar |> 
                Array.mapFold(fun s c ->
                    if s = 0 then 
                        if c < 40 then
                            (nums c,int ',') 
                        else
                            ([|c|],int ',') 
                    else 
                        if c < 40 then
                            (Array.append [|s|] (nums c),int ',') 
                        else
                            ([|s;c|],s)) 0 |> fst |>
                Array.concat
            Array.append ar1 [|10|])
    prog


// A B A B C C B C B A
// R 12 L 8 R 12 
// R 8 R 6 R 6 R 8
// R 8 L 8 R 8 R 4 R 4


let answer2 =
    let s = path playfield
    Console.BackgroundColor <- ConsoleColor.Black
    Console.ForegroundColor <- ConsoleColor.Gray
    Console.CursorTop <- 52
    Console.CursorLeft <- 0
    s


printfn "A2 %d" (Seq.length answer2)
for progline in createprogram answer2 do
    Console.ForegroundColor <- ConsoleColor.Green
    for cmd in progline do
        if cmd >= int ',' then
            printf "%c " (char cmd)
        else printf "%d " cmd
    printfn ""
    Console.ForegroundColor <- ConsoleColor.DarkGreen
    for cmd in progline do
        printf "%d "  cmd
    printfn ""
Console.ForegroundColor <- ConsoleColor.Gray

let finalanswer =
    let mutable p1 = Array.copy prog1
    p1[0] <- 2L
    let inputcode = 
        (createprogram answer2 @ [[|int 'N'; 10|]])|>
        List.toArray |>
        Array.concat |>
        Array.map (fun i -> int64 i)
    let mutable finised = false
    let mutable inputvals = inputcode
    let mutable extr1 = [||]
    let mutable state = (0L, 0, (0,0),false,(p1,extr1), false)
    let mutable veryout = 0L
    while not finised  do
        state <- doruntilout p1 extr1 inputvals (gsnd state) (snd(gtrd state))
        let (outp, pc, (inpcnt, pbase), fini, arrs, _) = state
        finised <- fini
        p1 <- fst arrs
        extr1 <- snd arrs
        inputvals <- inputvals[inpcnt..]
        if outp < 255 then printf "%c" (char outp)
        else
            veryout <- outp
            printf " %d " outp
    veryout

printfn "Answer2: %d" finalanswer
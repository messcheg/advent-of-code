open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let ex2 = "..\\..\\..\\Example2.txt"
let ex3 = "..\\..\\..\\Example3.txt"
let day = "E:\\develop\\advent-of-code-input\\2019\\Day20.txt"


let inp = 
    File.ReadLines day |>
    Seq.toArray

let Ycnt = inp.Length
let Xcnt = inp[0].Length

let isPort (c:char) = 'A' <= c && c <= 'Z'

let isDot (c:char) = c = '.'

let containsDot (loca:int*int) = 
    let (lY,lX) = loca
    let c = inp[lY][lX]
    isDot c

let emptyPort = ('-','-')

let rec borders y x (found:Map<string,int>) = 
    if y = Ycnt || found.Count = 8 then found
    else
        let fnd2 = 
            if x < Xcnt-1 && y < Ycnt-1 then
                let blok = (inp[y][x],inp[y][x+1],inp[y+1][x],inp[y+1][x+1])
                if blok = (' ', ' ', ' ', '#') then found.Add("OutLeft", x+1).Add("OutUp", y+1)
                elif blok = (' ', ' ', '#', ' ') then found.Add("OutRight", x)
                elif blok = ('#', '#', '#', ' ') then found.Add("InLeft", x).Add("InUp",y)
                elif blok = ('#', '#', ' ', '#') then found.Add("InRight", x)
                elif blok = ('#', ' ', '#', '#') then found.Add("InDown", y+1)
                elif blok = (' ', '#', ' ', ' ') then found.Add("OutDown", y)
                else found
            else found
        let (nY, nX) = if x<Xcnt-1 then (y,x+1) else (y+1,0)
        borders nY nX fnd2

let GetBorders = borders 0 0 Map.empty
let (OutL, OutU, OutR, OutD, InL, InU, InR, InD) = 
        (
             GetBorders["OutLeft"],
             GetBorders["OutUp"],
             GetBorders["OutRight"],
             GetBorders["OutDown"],
             GetBorders["InLeft"],
             GetBorders["InUp"],
             GetBorders["InRight"],
             GetBorders["InDown"]
        )

let outSide (y,x) = y = OutU || y = OutD || x = OutL || x = OutR
let inSide (y,x) = y = InU || y = InD || x = InL || x = InR

printfn "borders: %d" GetBorders.Count


let rec allPorts (keys:Map<char*char*char,(int*int)>) (entries:Map<int*int,char*char*char>) (y:int) (x:int) =
    if y = Ycnt then (keys,entries)
    else
        let ((k1,k2), loc) =
            if isPort (inp[y][x]) then
                if y < Ycnt - 1 && isPort (inp[y+1][x]) then
                    if y < Ycnt - 2 && isDot (inp[y+2][x]) then ((inp[y][x],inp[y+1][x]), (y+2,x))
                    else ((inp[y][x], inp[y+1][x]), (y-1,x))
                elif x < Xcnt - 1 && isPort (inp[y][x+1]) then
                    if x < Xcnt - 2 && isDot (inp[y][x+2]) then ((inp[y][x], inp[y][x+1]), (y,x+2))
                    else ((inp[y][x], inp[y][x+1]), (y,x-1))
                else (emptyPort,(0,0))
            else (emptyPort,(0,0))
        let (newKeys, newEntries) = 
            if (k1,k2) = emptyPort then (keys,entries)
            else
                let k3 = if outSide loc then 'o' else 'i'
                let key = (k1,k2,k3)
                (keys.Add(key, loc), entries.Add(loc,key))
        let (newX, newY) = if x = Xcnt - 1 then (0,y+1) else (x+1,y)
        allPorts newKeys newEntries newY newX
                
let (pk, pe) = allPorts (Map.empty) (Map.empty) 0 0

let rec mpMrg (m1:Map<char*char*char,int>) (m2:Map<char*char*char,int>) =
    if m2.Count = 0 then m1
    else
        let h = m2.Keys |> Seq.head
        let t = m2.Remove(h)
        if m1.ContainsKey(h) then
            if m2[h] < m1[h] then
                mpMrg (m1.Remove(h).Add(h, m2[h])) t
            else mpMrg m1 t
        else
            mpMrg (m1.Add(h, m2[h])) t

let colMrg2 (c1:Map<char*char*char,Map<char*char*char,int>>) (c2:Map<char*char*char,Map<char*char*char,int>>) =
    if c2.Count = 0 then c1
    else
        let chk = c2.Keys |> Seq.head
        if c1.ContainsKey(chk) then
            let mp1 = c1[chk]
            let mp2 = c2[chk]
            let mp3 = mpMrg mp1 mp2
            c1.Remove(chk).Add(chk,mp3)
        else
            c1.Add(chk, c2[chk])

let rec colMerge (collected:Map<char*char*char,Map<char*char*char,int>>) (arr:Map<char*char*char,Map<char*char*char,int>> array) = 
    if arr.Length = 0 then collected
    else
        let col2 =  colMrg2 collected arr[0]
        colMerge col2 arr[1..]

let rec allConnections (from:char*char*char) (current:int*int) (visited:Set<int*int>) (count:int) (collected:Map<char*char*char,Map<char*char*char,int>>) =
    if visited.Contains(current) || (not (containsDot current)) then collected
    else
        if pe.ContainsKey(current) && not (pe[current] = from) then 
            if collected.ContainsKey(from) then
                let arr = collected[from]
                collected.Remove(from).Add(from, arr.Add(pe[current],count))
            else
                collected.Add(from, Map.empty.Add(pe[current],count))
        else
            let nVis = visited.Add(current)
            let nCnt = count+1
            let (cY,cX) = current
            let nC1 = allConnections from (cY+1,cX) nVis nCnt Map.empty
            let nC2 = allConnections from (cY-1,cX) nVis nCnt Map.empty
            let nC3 = allConnections from (cY,cX+1) nVis nCnt Map.empty
            let nC4 = allConnections from (cY,cX-1) nVis nCnt Map.empty
            colMerge collected [| nC1; nC2; nC3; nC4|]


let rec GetGraph (keys:Map<char*char*char,(int*int)>) (collected:Map<char*char*char,Map<char*char*char,int>>) =
    if keys.Count = 0 then collected
    else
        let key = keys.Keys |> Seq.head 
        let newKeys = keys.Remove(key)
        let loc = keys[key]
        let col1 = allConnections key loc Set.empty 0 collected
        let (k1,k2,k3) = key
        let k4 = if k3 = 'o' then 'i' else 'o'
        let hopKey = (k1,k2,k4)
        let col2 = 
            if not (pk.ContainsKey(hopKey)) then col1
            else
                let map = col1[key].Add(hopKey,1)
                col1.Remove(key).Add(key,map) 

        GetGraph newKeys col2
        
let grp = GetGraph pk Map.empty

let hop (a1,a2,a3) (b1,b2,_) = 
    if (a1,a2) <> (b1,b2) then 0
    elif a3 = 'i' then 1
    else -1

let rec AddWork (keys:Map<char*char*char,int>) (fromKey:char*char*char) (count:int) (level:int) (inspected:Map<char*char*char*int, int>) (visited:Set<char*char*char*int>) (multilevel:bool) =
    if keys.Count = 0 then inspected
    else
        let k = keys.Keys |> Seq.head
        let v = keys[k] + count
        let (k1,k2,k3) = k
        let lev1 = level + if multilevel then hop fromKey k else 0
        let newKey = (k1,k2,k3,lev1)
        let newInsp = 
            if visited.Contains(newKey) || lev1 < 0 then inspected
            elif inspected.ContainsKey(newKey) then
                if inspected[newKey] > v then
                    inspected.Remove(newKey).Add(newKey,v)
                else
                    inspected
            else inspected.Add(newKey,v)
        AddWork (keys.Remove(k)) fromKey count level newInsp visited multilevel

let rec shortedPath (target:char*char*char*int) (inspected:Map<char*char*char*int, int>) (visited:Set<char*char*char*int>) (multilevel:bool) = 
    let first = inspected |> Seq.minBy(fun k -> k.Value)
    let (k1,k2,k3,level) = first.Key
    let key = (k1,k2,k3)
    if first.Key = target then inspected[first.Key]
    else
        let newVis = visited.Add(first.Key)
        let newInsp = AddWork grp[key] key (inspected[first.Key]) level (inspected.Remove(first.Key)) visited  multilevel
        shortedPath target newInsp newVis multilevel
    
printfn "key: %d, entries: %d, graph: %d" pk.Count pe.Count grp.Count
printfn "shortest answer1: %d" (shortedPath ('Z','Z','o',0) (Map.empty.Add(('A','A','o',0),0)) (Set.empty) false)
printfn "shortest answer2: %d" (shortedPath ('Z','Z','o',0) (Map.empty.Add(('A','A','o',0),0)) (Set.empty) true)

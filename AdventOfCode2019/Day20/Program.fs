open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let ex2 = "..\\..\\..\\Example2.txt"
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

let rec allPorts (keys:Map<char*char,(int*int) array>) (entries:Map<int*int,char*char>) (y:int) (x:int) =
    if y = Ycnt then (keys,entries)
    else
        let (key, loc) =
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
            if key = emptyPort then (keys,entries)
            elif keys.ContainsKey(key) then
                let ar = keys[key]
                (keys.Remove(key).Add(key, Array.concat [| ar ; [|loc|] |]), entries.Add(loc,key))
            else (keys.Add(key, [|loc|]), entries.Add(loc,key))
        let (newX, newY) = if x = Xcnt - 1 then (0,y+1) else (x+1,y)
        allPorts newKeys newEntries newY newX
                
let (pk, pe) = allPorts (Map.empty) (Map.empty) 0 0

let rec mpMrg (m1:Map<char*char,int>) (m2:Map<char*char,int>) =
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

let colMrg2 (c1:Map<char*char,Map<char*char,int>>) (c2:Map<char*char,Map<char*char,int>>) =
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

let rec colMerge (collected:Map<char*char,Map<char*char,int>>) (arr:Map<char*char,Map<char*char,int>> array) = 
    if arr.Length = 0 then collected
    else
        let col2 =  colMrg2 collected arr[0]
        colMerge col2 arr[1..]

let rec allConnections (from:char*char) (current:int*int) (visited:Set<int*int>) (count:int) (collected:Map<char*char,Map<char*char,int>>) =
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


let rec GetGraph (keys:Map<char*char,(int*int) array>) (collected:Map<char*char,Map<char*char,int>>) =
    if keys.Count = 0 then collected
    else
        let key = keys.Keys |> Seq.head 
        let newKeys = keys.Remove(key)
        let arr = keys[key]
        let col1 = allConnections key arr[0] Set.empty 0 collected
        let col2 = if arr.Length = 1 then col1 else allConnections key arr[1] Set.empty 0 col1
        GetGraph newKeys col2
        
let grp = GetGraph pk Map.empty

let rec AddWork (keys:Map<char*char,int>) (count:int) (inspected:Map<char*char, int>) (visited:Set<char*char>) =
    if keys.Count = 0 then inspected
    else
        let k = keys.Keys |> Seq.head
        let v = keys[k] + count
        let newInsp = 
            if visited.Contains(k) then inspected
            elif inspected.ContainsKey(k) then
                if inspected[k] > v then
                    inspected.Remove(k).Add(k,v)
                else
                    inspected
            else inspected.Add(k,v)
        AddWork (keys.Remove(k)) count newInsp visited

let rec shortedPath (target:char*char) (inspected:Map<char*char, int>) (visited:Set<char*char>) = 
    let first = inspected |> Seq.minBy(fun k -> k.Value)
    if first.Key = target then inspected[first.Key]
    else
        let newVis = visited.Add(first.Key)
        let hop = if pk[first.Key].Length = 2 then 1 else 0
        let newInsp = AddWork grp[first.Key] (inspected[first.Key]+hop) (inspected.Remove(first.Key)) visited 
        shortedPath target newInsp newVis
    
printfn "key: %d, entries: %d, graph: %d" pk.Count pe.Count grp.Count
printfn "shortest: %d" (shortedPath ('Z','Z') (Map.empty.Add(('A','A'),0)) (Set.empty))
open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let ex2 = "..\\..\\..\\Example2.txt"
let ex3 = "..\\..\\..\\Example3.txt"
let rl = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines rl |>
    Seq.toArray

let rec allKeys i j (allkeys:Map<char,int*int>) (alldoors:Map<char,int*int>) (everything:Map<char,int*int>) inp= 
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    let (nextI, nextJ) =
        if j+1 < lenJ then (i, (j+1))
        else ((i+1), 0)
    if i >= lenI then (allkeys, alldoors, everything)
    else
        let k = inp[i][j]
        let p = (i,j)
        let (ak1, ev1) = if (k >= 'a' && k <= 'z') || "@+-*%".Contains(k)  then (allkeys.Add(k,p), everything.Add(k,p)) else (allkeys,everything)
        let (ad1, ev2) = if (k >= 'A' && k <= 'Z') then (alldoors.Add(k,p), ev1.Add(k,p)) else (alldoors,ev1)
        allKeys nextI nextJ ak1 ad1 ev2 inp

let GetAllKeys = allKeys 0 0 Map.empty Map.empty Map.empty inp
 
let rec reachable (loc:int*int) (visited: Set<int * int>) (from:char) (reaching:Map<char,Set<char>>) (inpt:string array) =
    let lenI = Array.length inpt
    let lenJ = String.length inpt[0]
    let (i,j) = loc
    let k = inpt[i][j]
    if k = '#' then reaching
    elif visited.Contains((i,j)) then reaching
    elif (k <> from) && ((k >= 'a' && k <= 'z') || (k >= 'A' && k <= 'Z' )) then
        let n4 = reaching
        if n4.ContainsKey(from) then 
            if not (n4[from].Contains(k)) then
                let newC = n4[from].Remove(k).Add(k)
                n4.Remove(from).Add(from, newC)
            else n4
        else n4.Add(from,Set.empty.Add(k))
    else
        let newVisit = visited.Add((i,j))
        let n1 = if i + 1 < lenI then reachable ((i+1), j) newVisit from reaching inpt else reaching
        let n2 = if i > 0 then reachable ((i-1), j) newVisit from n1 inpt else n1 
        let n3 = if j + 1 < lenJ then reachable (i, (j+1)) newVisit from n2 inpt else n2 
        if j > 0 then reachable (i, (j-1)) newVisit from n3 inpt else n3

let rec mapKeys (loc:int*int) (visited: Set<int * int>) (costs:int) (from:char) (map:Map<char*char,int>) (inpt:string array)=
    let lenI = Array.length inpt
    let lenJ = String.length inpt[0]
    let (i,j) = loc
    let k = inpt[i][j]
    if k = '#' then map
    elif visited.Contains((i,j)) then map
    else
        let n0 = 
            if (k >= 'a' && k <= 'z') then
                if map.ContainsKey((from,k)) then 
                    if map[(from,k)] > costs then
                        map.Remove((from,k)).Remove((k,from)).Add((from,k),costs).Add((k,from), costs)
                    else map
                else map.Add((from,k),costs).Add((k,from), costs)
            else map
        let newCosts = costs+1
        let newVisit = visited.Add((i,j))
        let n1 = if i + 1 < lenI then mapKeys ((i+1), j) newVisit newCosts from n0 inpt else n0
        let n2 = if i > 0 then mapKeys ((i-1), j) newVisit newCosts from n1 inpt else n1 
        let n3 = if j + 1 < lenJ then mapKeys (i, (j+1)) newVisit newCosts from n2 inpt else n2 
        let n4 = if j > 0 then mapKeys (i, (j-1)) newVisit newCosts from n3 inpt else n3
        n4
        
let rec reachableSet (allkeys:Map<char,int*int>) (reaching:Map<char,Set<char>>) (inpt:string array) =
    if allkeys.Count = 0 then reaching
    else
        let k = allkeys.Keys |> Seq.head
        let pos = allkeys[k]
        let newReach = reachable pos Set.empty k reaching inpt
        reachableSet (allkeys.Remove(k)) newReach inpt
        
let GetReachable (allkeys:Map<char,int*int>) inpt = reachableSet allkeys Map.empty inpt

let rec distanceMapToKey i j k (keys:Map<char,int*int>) (map:Map<char*char,int>) (inpt:string array)=
    mapKeys (i,j) Set.empty 0 k map inpt

let rec distanceMap (keys:Map<char,int*int>) (map:Map<char*char,int>) (inpt:string array)=
    let k = keys.Keys |> Seq.head
    let (i,j) = keys[k]
    let newKeys = keys.Remove(k)
    if newKeys.IsEmpty then 
        map
    else 
        let newMap = distanceMapToKey i j k newKeys map inpt
        distanceMap newKeys newMap inpt

        
let GetDistanceMap (keys:Map<char,int*int>) = distanceMap keys Map.empty inp

let (mk,md,me) = GetAllKeys
let KeyMap = GetReachable me inp 
let KeyDistMap = GetDistanceMap mk

printfn "Check: %d" KeyMap.Keys.Count
printfn "Check: %d" KeyDistMap.Keys.Count

let lower k = 
    if 'A' <= k && k <= 'Z' then k + 'a' - 'A'
    else k

let validkey (k:char) (collectedKeys:Set<char>) =
    let a = mk.ContainsKey(k)
    let b = (not (collectedKeys.Contains(lower(k))))
    a && b

let validdoor (k:char) (collectedKeys:Set<char>) (deeper:Set<char>) =
    let a = collectedKeys.Contains(lower k)
    let b = not (deeper.Contains(k))
    a && b

let rec GetNextSteps (key:char) (collectedKeys:Set<char>) (deeper:Set<char>) (keymap:Map<char,Set<char>>) =
    let kmk = keymap[key]
    let direct = kmk |> Seq.filter(fun k -> validkey k collectedKeys)
    let ports = kmk |> Seq.filter(fun k -> validdoor k collectedKeys deeper)
    let sub = ports |> Seq.map(fun k -> GetNextSteps k collectedKeys (kmk + deeper) keymap)  
    let sub1 = sub |> Seq.concat
    direct|> Seq.append(sub1) |> Set.ofSeq

let rec AddWork (lst:List<char>) (workset:Set<int*char*string>) (discovered:Map<char*string, int*string>) (visited:Set<char*string>) (fromKey:char*string) (fromVal:int*string) =
    if lst.Length = 0 then (workset, discovered)
    else 
        let (cnt, pth) = fromVal  
        let (key, collect) = fromKey
        let addK = lst.Head
        let nxt = lst.Tail
        let nCnt = cnt + KeyDistMap[(key, addK)]
        let nPth = ((pth |> Seq.toList) @ [addK] ) |> List.toArray  
        let nCol = nPth |> Array.sort |> System.String
        let nPthc = nPth |> System.String
        let nKey = (addK,nCol)
        if visited.Contains(nKey) then AddWork nxt workset discovered visited fromKey fromVal
        elif discovered.ContainsKey(nKey) then  
            let (dCnt, dpth) = discovered[nKey]
            let nDisc = if dCnt > nCnt then discovered.Remove(nKey).Add(nKey,(nCnt,nPthc)) else discovered
            let nWset = if dCnt > nCnt then workset.Remove((dCnt,addK,nCol)).Add((nCnt,addK,nCol)) else workset
            AddWork nxt nWset nDisc visited fromKey fromVal
        else 
            AddWork nxt (workset.Add((nCnt,addK,nCol))) (discovered.Add(nKey,(nCnt,nPthc))) visited fromKey fromVal
           

let rec shortest (workset:Set<int*char*string>) (discovered:Map<char*string, int*string>) (visited:Set<char*string>) = 
    let work = workset |> Seq.head
    let (count, curkey, collectedkeys) = work
    let dKey = (curkey, collectedkeys)
    if collectedkeys.Length = mk.Count - 1 then discovered[dKey]
    else
        let newV = visited.Add(dKey)
        let dVal = discovered[dKey]
        let newD = discovered.Remove(dKey)
        let newW = workset.Remove(work)

        let nxt = GetNextSteps curkey (collectedkeys |> Seq.toArray |> Set.ofArray)  Set.empty KeyMap |> Seq.toList
        let (newWorkSet, newDiscovered) = AddWork nxt newW newD newV dKey dVal 
        shortest newWorkSet newDiscovered newV  
  
let sp = 
    shortest (Set.empty.Add((0,'@' ,""))) (Map.empty.Add(('@',""),(0,""))) Set.empty

let (spCnt, spP) = sp 
printfn "Answer1 %d" spCnt
printfn "%s" spP

let newMap = 
   let (i,j) = mk['@']
   let l1 = inp[i-1][0..j-2] + "+#-" + inp[i-1][j+2..] 
   let l2 = inp[i][0..j-2] + "###" + inp[i][j+2..] 
   let l3 = inp[i+1][0..j-2] + "*#%" + inp[i+1][j+2..] 
   Array.concat [| inp[0..i-2] ; [| l1;l2;l3 |] ; inp[i+2..] |]

let GetAllKeys2 = allKeys 0 0 Map.empty Map.empty Map.empty newMap
let (mk2,md2,me2) = GetAllKeys2
let GetDistanceMap2 (keys:Map<char,int*int>) = distanceMap keys Map.empty newMap

let KeyMap2 = GetReachable me2 newMap 
let KeyDistMap2 = GetDistanceMap2 mk2

printfn "Keys: %d" KeyMap2.Keys.Count
printfn "Distmap: %d" KeyDistMap2.Keys.Count


let GetDistance (keys:string*string) = 
    let (a,b) = keys
    if a=b then 0
    else
        let res = a |> Seq.zip(b) |> Seq.where(fun (c,d) -> c <> d) |> Seq.map(fun (c,d) -> KeyDistMap2[(c,d)]) |> Seq.head
        res

let GetRoboDiffKey (keys:string*string) = 
    let (a,b) = keys
    let res = a |> Seq.zip(b) |> Seq.where(fun (c,d) -> c <> d) |> Seq.map(fun (c,d) -> c) |> Seq.head
    res

let rec AddWork2 (lst:List<string>) (workset:Set<int*string*string>) (discovered:Map<string*string, int*string>) (visited:Set<string*string>) (fromKey:string*string) (fromVal:int*string) =
    if lst.Length = 0 then (workset, discovered)
    else 
        let (cnt, pth) = fromVal  
        let (key, collect) = fromKey
        let addK = lst.Head
        let nxt = lst.Tail
        let nCnt = cnt + GetDistance (key, addK)
        let addToPath = GetRoboDiffKey (key, addK)
        let nPth = ((pth |> Seq.toList) @ [addToPath] ) |> List.toArray  
        let nCol = nPth |> Array.sort |> System.String
        let nPthc = nPth |> System.String
        let nKey = (addK,nCol)
        if visited.Contains(nKey) then AddWork2 nxt workset discovered visited fromKey fromVal
        elif discovered.ContainsKey(nKey) then  
            let (dCnt, dpth) = discovered[nKey]
            let nDisc = if dCnt > nCnt then discovered.Remove(nKey).Add(nKey,(nCnt,nPthc)) else discovered
            let nWset = if dCnt > nCnt then workset.Remove((dCnt,addK,nCol)).Add((nCnt,addK,nCol)) else workset
            AddWork2 nxt nWset nDisc visited fromKey fromVal
        else 
            AddWork2 nxt (workset.Add((nCnt,addK,nCol))) (discovered.Add(nKey,(nCnt,nPthc))) visited fromKey fromVal
        
let rec GetNextSteps2 (kcnt:int) (key:string) (collectedKeys:Set<char>) (keymap:Map<char,Set<char>>) =
    let newset = GetNextSteps key[kcnt] collectedKeys Set.empty keymap |> Set.map(fun t -> key[0..kcnt-1] + t.ToString() + key[kcnt+1..]) 
    if kcnt < key.Length - 1 then newset + (GetNextSteps2 (kcnt+1) key collectedKeys keymap)
    else newset
        
let rec shortest2 (workset:Set<int*string*string>) (discovered:Map<string*string, int*string>) (visited:Set<string*string>) = 
    let work = workset |> Seq.head
    let (count, curkey, collectedkeys) = work
    let dKey = (curkey, collectedkeys)
    if collectedkeys.Length = mk2.Count - curkey.Length then discovered[dKey]
    else
        let newV = visited.Add(dKey)
        let dVal = discovered[dKey]
        let newD = discovered.Remove(dKey)
        let newW = workset.Remove(work)

        let nxt = GetNextSteps2 0 curkey (collectedkeys |> Seq.toArray |> Set.ofArray) KeyMap2 |> Seq.toList
        let (newWorkSet, newDiscovered) = AddWork2 nxt newW newD newV dKey dVal 
        shortest2 newWorkSet newDiscovered newV  

let sp2 = 
    shortest2 (Set.empty.Add((0,"+-*%" ,""))) (Map.empty.Add(("+-*%",""),(0,""))) Set.empty

let (spCnt2, spP2) = sp2 
printfn "Answer1 %d" spCnt2
printfn "%s" spP2


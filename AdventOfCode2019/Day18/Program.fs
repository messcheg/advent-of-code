open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let ex2 = "..\\..\\..\\Example2.txt"
let rl = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines ex2 |>
    Seq.toArray

let rec allKeys i j (allkeys:Map<char,int*int>) (alldoors:Map<char,int*int>)= 
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    let (nextI, nextJ) =
        if j+1 < lenJ then (i, (j+1))
        else ((i+1), 0)
    if i >= lenI then (allkeys, alldoors)
    elif ( inp[i][j] >= 'a' && inp[i][j] <= 'z') || inp[i][j] = '@' then allKeys nextI nextJ (allkeys.Add(inp[i][j], (i,j))) alldoors 
    elif ( inp[i][j] >= 'A' && inp[i][j] <= 'Z') then allKeys nextI nextJ allkeys (alldoors.Add(inp[i][j], (i,j))) 
    else  allKeys nextI nextJ allkeys alldoors

let GetAllKeys = allKeys 0 0 Map.empty Map.empty
 
let rec reachable (loc:int*int) (visited: Set<int * int>) (from:char) (reaching:Map<char,Set<char>>)=
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    let (i,j) = loc
    let k = inp[i][j]
    if k = '#' then reaching
    elif visited.Contains((i,j)) then reaching
    else
        let newFrom = if (k >= 'a' && k <= 'z') || (k >= 'A' && k <= 'Z' ) then k else from 
    
        let newVisit = visited.Add((i,j))
        let n1 = if i + 1 < lenI then reachable ((i+1), j) newVisit newFrom reaching else reaching
        let n2 = if i > 0 then reachable ((i-1), j) newVisit newFrom n1  else n1 
        let n3 = if j + 1 < lenJ then reachable (i, (j+1)) newVisit newFrom n2  else n2 
        let n4 = if j > 0 then reachable (i, (j-1)) newVisit newFrom n3 else n3
        
        if (k >= 'a' && k <= 'z') || (k >= 'A' && k <= 'Z' ) then
            if n4.ContainsKey(from) then 
                if not (n4[from].Contains(k)) then
                    let newC = n4[from].Remove(k).Add(k)
                    n4.Remove(from).Add(from, newC)
                else n4
            else n4.Add(from,Set.empty.Add(k))
        else n4

let rec mapKeys (loc:int*int) (visited: Set<int * int>) (costs:int) (from:char) (map:Map<char*char,int>)=
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    let (i,j) = loc
    let k = inp[i][j]
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
        let n1 = if i + 1 < lenI then mapKeys ((i+1), j) newVisit newCosts from n0 else n0
        let n2 = if i > 0 then mapKeys ((i-1), j) newVisit newCosts from n1  else n1 
        let n3 = if j + 1 < lenJ then mapKeys (i, (j+1)) newVisit newCosts from n2  else n2 
        let n4 = if j > 0 then mapKeys (i, (j-1)) newVisit newCosts from n3 else n3
        n4
        

        
let GetReachable (allkeys:Map<char,int*int>) = reachable allkeys['@'] Set.empty '@' (Map.empty.Add('@', Set.empty))

let rec distanceMapToKey i j k (keys:Map<char,int*int>) (map:Map<char*char,int>) =
    mapKeys (i,j) Set.empty 0 k map

let rec distanceMap (keys:Map<char,int*int>) (map:Map<char*char,int>) =
    let k = keys.Keys |> Seq.head
    let (i,j) = keys[k]
    let newKeys = keys.Remove(k)
    if newKeys.IsEmpty then 
        map
    else 
        let newMap = distanceMapToKey i j k newKeys map
        distanceMap newKeys newMap

        
let GetDistanceMap (keys:Map<char,int*int>) = distanceMap keys Map.empty

let (mk,md) = GetAllKeys
let KeyTree = GetReachable mk
let KeyDistMap = GetDistanceMap mk

printfn "Check: %d" KeyTree.Keys.Count
printfn "Check: %d" KeyDistMap.Keys.Count

let rec husslekey (h1:string) (h2:string) (cnt:int) (result:List<string*string>) = 
    if cnt = h2.Length then
        result
    else
        let k =  h2[cnt]
        if 'A' <= k && k <= 'Z' && h2.Contains(k + 'a' - 'A') then
            husslekey h1 h2 (cnt+1) result
        else
            husslekey h1 h2 (cnt+1) (result @ [(h1 + h2[cnt..cnt], h2[0..cnt-1] + h2[cnt+1..])])

let rec doHussle (collected:List<string*string>)=
    let (h1,h2) = collected[0]
    if h2.Length = 0 then 
        collected
    else
        doHussle (collected[1..] @ (husslekey h1 h2 0 List.empty) ) 

let rec Usefull (s:string) startloc =
    let firstDoorIdx = s.IndexOfAny((KeyTree.Keys |> Seq.toArray), startloc)
    if firstDoorIdx = -1 then true
    else
        let k = s[firstDoorIdx] - 'A' + 'a'
        let pos =  s.IndexOf(k)
        if pos > firstDoorIdx then false
        elif pos = -1 then 
            if startloc = 0 then false
            else true
        else Usefull s (firstDoorIdx+1)

let rec FilterUseless (col:List<string*string>) (res:List<string>) =
    if col.Length = 0 then res
    else
        let (h,_) = col[0]
        if Usefull h  0 then FilterUseless (col[1..]) (res @ [h]) 
        else FilterUseless (col[1..]) res 

let substituteAndHussleString (path:string) =
    let firstDoorIdx = path.IndexOfAny(KeyTree.Keys |> Seq.toArray)
    if firstDoorIdx = -1 then List.empty
    else
        let firstDoor = path[firstDoorIdx]
        let reachable = KeyTree[firstDoor]
        let part1 = path[0..firstDoorIdx-1]
        let part2 = path[firstDoorIdx+1..] + (reachable |> Seq.toArray |> System.String)
        let col = doHussle [(part1,part2)]
        FilterUseless col List.empty

let rec substituteAndHussle (paths:List<string>) =
    let h = paths[0]
    let subs = substituteAndHussleString h
    if subs.IsEmpty then paths
    else 
        substituteAndHussle (paths[1..] @ subs)
(*
let husseldedlist = substituteAndHussle ["@"]

printfn "Check list: %d"  husseldedlist .Length
*)

let rec shortest (work:Map<int, Map<int,Set<string>>>) = 
    let cost = work.Keys |> Seq.head
    let bestones = work[cost]
    let longest = bestones.Keys |> Seq.last
    let str = bestones[longest] |> Seq.head

    (0,"")
    
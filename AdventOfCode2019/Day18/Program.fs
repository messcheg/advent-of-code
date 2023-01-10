open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let ex2 = "..\\..\\..\\Example2.txt"
let rl = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines rl |>
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
 
let rec reachable (loc:int*int) (visited: Set<int * int>) (from:char) costs (reaching:Map<char,Map<char,int>>)=
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    let (i,j) = loc
    let k = inp[i][j]
    if k = '#' then reaching
    elif visited.Contains((i,j)) then reaching
    else
        let (newFrom, newCosts) = if k >= 'A' && k <= 'Z' then (k, 1) else (from, costs + 1) 
    
        let newVisit = visited.Add((i,j))
        let n1 = if i + 1 < lenI then reachable ((i+1), j) newVisit newFrom newCosts reaching else reaching
        let n2 = if i > 0 then reachable ((i-1), j) newVisit newFrom newCosts n1  else n1 
        let n3 = if j + 1 < lenJ then reachable (i, (j+1)) newVisit newFrom newCosts n2  else n2 
        let n4 = if j > 0 then reachable (i, (j-1)) newVisit newFrom newCosts n3 else n3
        
        if (k >= 'a' && k <= 'z') || (k >= 'A' && k <= 'Z' ) then
            if n4.ContainsKey(from) then 
                if n4[from].ContainsKey(k) && n4[from][k] > costs then
                    let newC = n4[from].Remove(k).Add(k, costs)
                    n4.Remove(from).Add(from, newC)
                else n4
            else n4.Add(from,Map.empty.Add(k,costs))
        else n4
        
let GetReachable (allkeys:Map<char,int*int>) = reachable allkeys['@'] Set.empty '@' 0 (Map.empty.Add('@', Map.empty))
        
let MakeSet (path:string) =
       Set.ofArray (path |> Seq.toArray) 

let rec AddWork (reach:Map<char,int>) path cost (costs:Map<char*string,int*string>) (visited:Set<char*string>)=
    if reach.IsEmpty then costs
    else
        let k = reach.Keys |> Seq.head
        let c = reach[k]
        let t = reach.Remove(k)
        let path1 = (path + k.ToString())
        let newCost = cost + c
        let keys = (path1.ToCharArray()) |> Array.sort |> System.String
        if visited.Contains (k,keys) then
            AddWork t path cost costs visited
        else
            let newCosts = 
                if costs.ContainsKey (k,keys) then 
                    let (cst, _ ) = costs[(k,keys)]
                    if (cst > newCost) then costs.Remove((k,keys)).Add((k,keys), (newCost,path1))
                    else costs
                else costs.Add((k,keys), (newCost,path1))
            AddWork t path cost newCosts visited
            

//let rec findpath (costs:Map<char*string,int*string>) (visited:Set<char*string>) (allkeys:Map<char,int*int>)  =
//    let free = Map.keys costs |> Seq.filter(fun (k1,k2) -> not (visited.Contains((k1,k2))))
//    let (k,keys) = 
//        free |> 
//        Seq.minBy(fun k ->
//            let (c, _) = costs[k]
//            c)
//    let (x,y) = allkeys[k]
//    let (cost, path) = costs[(k,keys)]
//    if keys.Length = allkeys.Count - 1 then (cost, path)
//    else
//        let newVisit = visited.Add((k,keys))
//        let reach = GetReachable x y (MakeSet path)
//        let distnew = AddWork reach path cost costs newVisit
//        findpath distnew newVisit allkeys 
//    
//let GetBestPath =
//    findpath (Map.empty.Add(('@', ""),(0,""))) Set.empty GetAllKeys
//
//let (FinalCost, FinalPath) = GetBestPath
//
//printfn "Answer1: %d" FinalCost
//printfn "Path: %s" FinalPath
//

let (mk,md) = GetAllKeys
printfn "Check: %d" (GetReachable mk).Keys.Count
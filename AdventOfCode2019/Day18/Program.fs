open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let ex2 = "..\\..\\..\\Example2.txt"
let rl = "..\\..\\..\\real_input.txt"

let inp = 
    File.ReadLines ex1 |>
    Seq.toArray

let rec startlocation i j =
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    if inp[i][j] = '@' then (i,j)
    else
        if j+1 < lenJ then startlocation i (j+1)
        elif i+1 < lenI then startlocation (i+1) 0
        else (-1,-1)

let rec allKeys i j = 
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    let next =
        if j+1 < lenJ then allKeys i (j+1)
        elif i+1 < lenI then allKeys (i+1) 0
        else Set.empty
    if inp[i][j] >= 'a' && inp[i][j] <= 'z' then next.Add(inp[i][j]) else next

let GetAllKeys = allKeys 0 0
let GetStartLocation = startlocation 0 0
 
let rec reachable i j (collectedKeys: Set<char>) (visited: Set<int * int>) costs =
    let lenI = Array.length inp
    let lenJ = String.length inp[0]
    if inp[i][j] = '#' then Set.empty
    elif inp[i][j] >= 'A' && inp[i][j] <= 'Z' && not (collectedKeys.Contains( inp[i][j] + 'z' - 'a' )) then Set.empty 
    elif visited.Contains((i,j)) then Set.empty
    else    
        let newVisit = Set.empty.Add((i,j)) + visited
        let newCosts = costs + 1
        let n1 = if i + 1 < lenI then reachable (i+1) j collectedKeys newVisit newCosts else Set.empty 
        let n2 = if i > 0 then reachable (i-1) j collectedKeys newVisit newCosts  else Set.empty 
        let n3 = if j + 1 < lenJ then reachable i (j+1) collectedKeys newVisit newCosts  else Set.empty 
        let n4 = if j > 0 then reachable i (j-1) collectedKeys newVisit newCosts  else Set.empty 
        let nAll = n1 + n2 + n3 + n4
        if inp[i][j] >= 'a' && inp[i][j] <= 'z' && not (collectedKeys.Contains( inp[i][j])) then 
            nAll.Add((inp[i][j],(i , j), costs)) 
        else nAll
        
let GetReachable i j collectedKeys = reachable i j collectedKeys Set.empty 0
        

let rec findpath worklist visited =
    let next = List.sortBy (fun (cost, path) -> cost) worklist 
    let h = List.head next
    let t = List.tail next



    // mijn idee voor de oplossing: set-met-sleutels-leeg is "klaar"
    // sleutel pakken is uit-de-set kortstepad naar de sleutel
    // open deuren
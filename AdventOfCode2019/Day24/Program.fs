open System.IO

let ex1 = "..\\..\\..\\Example1.txt"
let day = "E:\\develop\\advent-of-code-input\\2019\\Day24.txt"

let inp1 = 
    File.ReadLines ex1 |>
    Seq.toArray

let inp2 = 
    File.ReadLines day |>
    Seq.toArray

let rec bioRating (eris:string array) (x,y) (count:int64) (pwr:int64)=
    let count1 = if eris[y][x] = '.' then count else count + pwr
    let pwr1 = pwr * 2L
    if x = eris[0].Length - 1 then 
        if y = eris.Length - 1 then count1
        else bioRating eris (0,y+1) count1 pwr1
    else
        bioRating eris (x+1, y) count1 pwr1

let rec nextminute (eris:string array) (x,y) (newris:string array) =
    let c1 = if x > 0 && eris[y][x-1] = '#' then 1 else 0
    let c2 = if x < eris[0].Length - 1 && eris[y][x+1] = '#' then 1 else 0
    let c3 = if y > 0 && eris[y-1][x] = '#' then 1 else 0
    let c4 = if y < eris.Length - 1 && eris[y+1][x] = '#' then 1 else 0
    let cnt = c1 + c2 + c3 + c4
    let newval =
        if eris[y][x] = '.' && (cnt = 1 || cnt = 2) then "#"
        elif eris[y][x] = '#' && cnt = 1 then "#"
        else "."
    let newline = newris[y].Remove(x,1).Insert(x,newval)
    let newris1 = newris |> Array.removeAt y |> Array.insertAt y newline
    if x = eris[0].Length - 1 then 
        if y = eris.Length - 1 then newris1
        else nextminute eris (0,y+1) newris1
    else
       nextminute eris (x+1, y) newris1
    

let printEris (eris:string array) =
    printfn "--------------------"
    for line in eris do printfn "%s" line


let rec findprev eris (visited:Set<int64>) =
    let rating = bioRating eris (0,0) 0 1
    if visited.Contains(rating) then rating
    else
        let eris1 = nextminute eris (0,0) eris
        let vis1 = visited.Add rating
        findprev eris1 vis1

printfn "Example: %d" (findprev inp1 Set.empty)
printfn "Answer1: %d" (findprev inp2 Set.empty)

let ovralChr a b = if a = "#" || b = "#" then "#" else "."

let rec nextMinuteRecursive (eris:string array array) level (x,y) (newris:string array array) =

    let newris1 = if x = 2 && y = 2 then
                     if level < eris.Length - 1 then
                        nextMinuteRecursive eris (level+1) (0,0) newris
                     else
                        let cleft = ((eris[level][2])[1]).ToString()
                        let cright = ((eris[level][2])[3]).ToString()
                        let cup = ((eris[level][1])[2]).ToString()
                        let cdown = ((eris[level][3])[2]).ToString()
                        if cleft = "#" || cright = "#"  || cup = "#" || cdown = "#" then
                            let (erisnew:string array) =  
                                [|  (ovralChr cleft cup) + cup + cup + cup + (ovralChr cright cup);
                                    cleft + "..." + cright;
                                    cleft + "..." + cright;
                                    cleft + "..." + cright;
                                    (ovralChr cleft cdown) + cdown + cdown + cdown + (ovralChr cright cdown)
                                 |]  
                            Array.append newris [|erisnew|]
                        else newris
                  else
                    let c1 = if x = 0 then
                                if level > 0 && (eris[level-1][2])[1] = '#' then 1 else 0
                             elif x = 3 && y = 2 then
                                if level < eris.Length - 1 then
                                    eris[level+1] |> Array.sumBy(fun a -> if a[4] = '#' then 1 else 0)
                                else 0
                             elif (eris[level][y])[x-1] = '#' then 1 
                             else 0
                    let c2 = if x = 4 then
                                if level > 0 && (eris[level-1][2])[3] = '#' then 1 else 0
                             elif x = 1 && y = 2 then
                                if level < eris.Length - 1 then
                                    eris[level+1] |> Array.sumBy(fun a -> if a[0] = '#' then 1 else 0)
                                else 0
                             elif (eris[level][y])[x+1] = '#' then 1 
                             else 0
                    let c3 = if y = 0 then
                                if level > 0 && (eris[level-1][1])[2] = '#' then 1 else 0
                             elif y = 3 && x = 2 then
                                if level < eris.Length - 1 then
                                    eris[level+1][4] |> Seq.sumBy(fun a -> if a = '#' then 1 else 0)
                                else 0
                             elif (eris[level][y-1])[x] = '#' then 1 
                             else 0
                    let c4 = if y = 4 then
                                if level > 0 && (eris[level-1][3])[2] = '#' then 1 else 0
                             elif y = 1 && x = 2 then
                                if level < eris.Length - 1 then
                                    eris[level+1][0] |> Seq.sumBy(fun a -> if a = '#' then 1 else 0)
                                else 0
                             elif (eris[level][y+1])[x] = '#' then 1 
                             else 0
                    
                    let cnt = c1 + c2 + c3 + c4
                    let newval =
                        if (eris[level][y])[x] = '.' && (cnt = 1 || cnt = 2) then "#"
                        elif (eris[level][y])[x] = '#' && cnt = 1 then "#"
                        else "."
                    let newline = (newris[level][y]).Remove(x,1).Insert(x,newval)
                    let newrislev = newris[level] |> Array.removeAt y |> Array.insertAt y newline
                    newris |> Array.removeAt level |> Array.insertAt level newrislev
    if x = 4 then 
        if y = 4 then
            if level = 0 then
                let sLeft = eris[0] |> Seq.sumBy(fun a -> if a[0] = '#' then 1 else 0)
                let sRight = eris[0] |> Seq.sumBy(fun a -> if a[4] = '#' then 1 else 0)
                let sUp = eris[0][0] |> Seq.sumBy(fun a -> if a = '#' then 1 else 0)
                let sDown = eris[0][0] |> Seq.sumBy(fun a -> if a = '#' then 1 else 0)
                let is12 a = a=1 || a=2
                if is12 sLeft || is12 sRight || is12 sUp || is12 sDown then
                    let chr12 a = if is12 a then "#" else "."
                    let leveldown = [| [| 
                                        ".....";
                                        ".." + chr12 sUp + "..";
                                        "." + chr12 sLeft + "?" + chr12 sRight + ".";
                                        ".." + chr12 sDown + "..";
                                        "....."
                                    |] |]
                    Array.append leveldown newris1
                else
                    newris1
            else
                newris1
        else nextMinuteRecursive eris level (0,y+1) newris1
    else
       nextMinuteRecursive eris level (x+1, y) newris1


let rec erisRecurse (erisArr:string array array) times =
    if times = 0 then erisArr
    else
        let eris1 = nextMinuteRecursive erisArr 0 (0,0) erisArr
        erisRecurse eris1 (times-1)

let printErisArr (erisArr:string array array) =
    for eris in erisArr do printEris eris

let countBugs (erisArr:string array array) =
    erisArr |>
    Array.sumBy(fun a -> 
        a |> Array.sumBy( fun b ->
            b |> Seq.sumBy (fun c ->
                if c = '#' then 1 else 0
            )))

printErisArr (erisRecurse [|inp1|] 10)

printfn "Examplebugs: %d, expected:99" (countBugs (erisRecurse [|inp1|] 10))
printfn "Answer2: %d" (countBugs (erisRecurse [|inp2|] 200))



    
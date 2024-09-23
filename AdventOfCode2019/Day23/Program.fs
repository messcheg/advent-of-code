open System.IO
open IntcodeMachine.Intcode

let filename = "E:\\develop\\advent-of-code-input\\2019\\Day23.txt"

let prog1 = IntcodeMachine.Intcode.inp filename


let bootup number =
    let pst = initalprogstate (Array.copy prog1)
    let pst1 = runNextStep number pst
    // runFunInUntilOut arrInp arrExtra (fun x -> getInput[int x]) pc pbase
    pst1

let startSet n = 
    [|0..n|] |>
    Array.map(fun a -> (a, bootup a))

let emptyQueus =
    [|0..49|] |> 
    Array.map(fun _ -> [||])

let rec newround node (queues:int64 array array) (collected:int64 array array) idle =
    let (nodeNum,((outval,wvi),pst)) = node
    if wvi then
        let curQ = queues[nodeNum]
        if Array.length curQ = 0 then
            let node1 = (nodeNum, runNextStep -1L pst)
            let (_,((_,wvi1),_)) = node1
            (node1, queues, collected, false, [||], idle && wvi1)
        else
            let qh = curQ |> Array.head
            let qt = curQ |> Array.tail
            let node1 = (nodeNum, runNextStep qh pst)
            let queues1 =  queues |> Array.removeAt nodeNum |> Array.insertAt nodeNum qt
            (node1, queues1, collected, false, [||], false)
    else
        // hier de output eruit lezen en in een queue stoppen
        let collectedoutput = collected[nodeNum]
        let colout1 =  Array.append collectedoutput [|outval|]
        let node1 = (nodeNum, runNextStepArr [||] pst)
        let len = colout1 |> Array.length
        let collected1 = if len < 3 then collected |> Array.removeAt nodeNum |> Array.insertAt nodeNum colout1
                         else collected |> Array.removeAt nodeNum |> Array.insertAt nodeNum [||]
        if len = 3 then
            let target = int colout1[0]
            if target = 255 then (node1,queues, collected1, true, colout1, false)
            else 
                let curQ = queues[target]
                let que =  Array.append curQ colout1[1..2]
                let queues1 =  queues |> Array.removeAt target |> Array.insertAt target que        
                (node1,queues1,  collected1, false, [||], false)
        else
           newround node1 queues collected1 false
           
let rec find255 nodes queues collected rest =
    if Array.isEmpty nodes then find255 rest queues collected [||]
    else    
        let node = nodes |> Array.head
        let (node1, queues1, collected1, found, result, _) = newround node queues collected false
        if found then result[2]
        else
            let tail = nodes |> Array.tail
            let rest1 = Array.append rest [|node1|]
            find255 tail queues1 collected1 rest1 
    

printfn "Answer1: %d" (find255 (startSet 49) emptyQueus emptyQueus [||]) 


let rec NAT255 nodes queues collected rest idle (last255:int64 array) (prevSend255:int64 array) =
    if Array.isEmpty nodes then
        if idle then
            if Array.length prevSend255 = 3 && prevSend255[2] = last255[2] then last255[2]
            else
                let queues1 = queues |> Array.removeAt 0 |> Array.insertAt 0 last255[1..2] 
                NAT255 rest queues1 collected [||] true last255 last255
        else 
            NAT255 rest queues collected [||] true last255 prevSend255 
    else    
        let node = nodes |> Array.head
        let (node1, queues1, collected1, found, result, idle1) = newround node queues collected idle
        let last255_1 = if found then result else last255 
        let tail = nodes |> Array.tail
        let rest1 = Array.append rest [|node1|]
        NAT255 tail queues1 collected1 rest1 (idle && idle1) last255_1 prevSend255
        
printfn "Answer2: %d" (NAT255 (startSet 49) emptyQueus emptyQueus [||] true [||] [||]) 


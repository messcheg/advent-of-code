let inputfrom = 206938 
let inputuntil = 679128

let rec checkeqpair a (b : string) =
    let l = String.length b
    if a = b[0] then true
    elif l > 1 then checkeqpair b[0] b[1.. l - 1]
    else false

let containseqpair (b:string) =
    checkeqpair b[0] b[1.. String.length b - 1]

let rec checkincreasing a (b : string) =
   let l = String.length b
   if a <= b[0] then 
        if l > 1 then checkincreasing b[0] b[1.. l - 1]
        else true
   else false  

let allincreasing (b:string) = 
    checkincreasing b[0] b[1.. String.length b - 1]

let meetscriteria num =
    let strnum = num.ToString()
    containseqpair strnum && allincreasing strnum
    
let numberofvalidkeys =
    let mutable cnt = 0
    for i = inputfrom to inputuntil do
        if meetscriteria i then cnt <- cnt + 1
    cnt

let answer1 = numberofvalidkeys  

printfn "Answer1: %d" answer1

let inputfrom = 206938 
let inputuntil = 679128

let rec checkeqpair a (b : string) a2 ign =
    let l = String.length b
    if a = b[0] then
        if not a2 then true
        else (a <> ign && (l < 2 || a <> b[1])) || (l > 2 && checkeqpair b[0] b[1.. l - 1] a2 a)
    elif l > 1 then checkeqpair b[0] b[1.. l - 1] a2 ign
    else false

let containseqpair (b:string) a2 =
    checkeqpair b[0] b[1.. String.length b - 1] a2

let rec checkincreasing a (b : string) =
   let l = String.length b
   if a <= b[0] then 
        if l > 1 then checkincreasing b[0] b[1.. l - 1]
        else true
   else false  

let allincreasing (b:string) = 
    checkincreasing b[0] b[1.. String.length b - 1]

let meetscriteria num a2 =
    let strnum = num.ToString()
    containseqpair strnum a2 ' '
    && allincreasing strnum 
  
    
let numberofvalidkeys a2 =
    let mutable cnt = 0
    for i = inputfrom to inputuntil do
        if meetscriteria i a2 then cnt <- cnt + 1
    cnt

let answer1 = numberofvalidkeys false
let answer2 = numberofvalidkeys true

printfn "Answer1: %d" answer1
printfn "Answer2: %d" answer2

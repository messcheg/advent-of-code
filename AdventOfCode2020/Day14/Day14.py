#f = open("example_input.txt")
#f = open("example2_input.txt")
f = open("real_input.txt")
l = f.readlines()

import math
from typing import overload

mem = {}
maskA = 0
maskO = 0
for ins in l:
    if ins.startswith("mask"):
       maskA = 0
       maskO = 0  
       for c in ins[7:43] :
            maskA = (maskA << 1) + int(c != '0')
            maskO = (maskO << 1) + int(c == '1')
    else :
        vals = ins[4:].split('] = ')
        mem[vals[0]] = (int(vals[1]) & maskA) | maskO
cnt = 0
for v in mem:
    cnt += mem[v]

print ("Answer1: ", cnt)

def getpermutations(addr, mask):
    perms = [0]
    start = round(math.pow(2,len(mask)-1))
    for c in mask :        
        if c == 'X' :
            for i in range(0, len(perms)):
                perms[i] = perms[i] * 2
                perms.append(perms[i] +1)
        else :
            add = int(start <= addr or c == '1')
            for i in range(0, len(perms)) : perms[i] = perms[i] * 2 + add
        if start <= addr : addr -= start            
        start = start // 2
    return perms

memnew = {}
for ins in l:
    if ins.startswith("mask"):
        mask = ins[7:43]
    else :
        vals = ins[4:].split('] = ')
        raddr = int(vals[0])
        for adr in getpermutations(raddr, mask):
            memnew[adr] = int(vals[1])
cnt = 0
for v in memnew:
    cnt += memnew[v]
print ("Answer2: ", cnt)
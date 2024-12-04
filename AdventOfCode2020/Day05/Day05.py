#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

def getRow(ticket):
    top = 128
    bottom = 0
    for i in range(0,7):
        mid = (top + bottom)//2
        if ticket[i] == 'F' : top = mid
        else : bottom = mid
    return bottom

def getCol(ticket):
    left = 0
    right = 8
    for i in range(0,3):
        mid = (left + right)//2
        if ticket[i+7] == 'L' : right = mid
        else : left = mid
    return left

allseats = []
highest = 0
for tick in l:
    row = getRow(tick)
    col = getCol(tick)
    sid = row * 8 + col
    allseats.append(sid)
    if highest < sid : highest = sid

print ("Answer1: ", highest)

allseats.sort()
miss = 0
for i in range(1,len(allseats)):
    if allseats[i] > allseats[i -1] + 1 :
       miss = allseats[i -1] + 1 ;

print ("Answer2: ", miss)
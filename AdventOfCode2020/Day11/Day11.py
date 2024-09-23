#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

l1 = l
lenX = len(l[0])
if l[0].endswith('\n'): lenX -= 1
lenY = len(l)

changed =True
while changed:
    changed = False
    newl = []
    for y in range (0,lenY):
        line = ""
        for x in range (0, lenX):
            if l[y][x] == '.' : line += '.'
            else :
                occupied = 0
                if x > 0 and y > 0 and l[y-1][x-1] == '#' : occupied += 1
                if x > 0 and l[y][x-1] == '#' : occupied += 1
                if x > 0 and y < lenY - 1 and l[y + 1][x-1] == '#' : occupied += 1
                if y > 0 and l[y-1][x] == '#' : occupied += 1
                if y < lenY - 1 and l[y + 1][x] == '#' : occupied += 1
                if x < lenX -1  and y > 0 and l[y-1][x+1] == '#' : occupied += 1
                if x < lenX -1  and l[y][x+1] == '#' : occupied += 1
                if x < lenX -1  and y < lenY - 1 and l[y + 1][x+1] == '#' : occupied += 1
                if l[y][x] == 'L' :
                    if occupied == 0: 
                        line += '#'
                        changed = True
                    else : line += 'L'
                elif l[y][x] == '#' :
                    if occupied >= 4 : 
                        line += 'L'
                        changed = True
                    else : line += '#'
        newl.append(line)
    l = newl
    #for a in l:
    #    print( a)
    #print ('--------------------------')

result = 0
for item in l:
    for c in item:
        if c == '#': result += 1

print ("Answer1: ", result)

def checkOccupied(x,y,a,b,l, lenX, lenY):
    x += a
    y += b
    while x >=0 and y >= 0 and x < lenX and y < lenY:
        if l[y][x] != '.' : return l[y][x] == '#'
        x += a
        y += b
    return False

l = l1
changed =True
while changed:
    changed = False
    newl = []
    for y in range (0,lenY):
        line = ""
        for x in range (0, lenX):
            if l[y][x] == '.' : line += '.'
            else :
                occupied = 0
                i = x
                j = y
                if checkOccupied(x,y,-1,-1,l,lenX,lenY) : occupied += 1 
                if checkOccupied(x,y,0,-1,l,lenX,lenY) : occupied += 1 
                if checkOccupied(x,y,1,-1,l,lenX,lenY) : occupied += 1 
                if checkOccupied(x,y,-1,0,l,lenX,lenY) : occupied += 1 
                if checkOccupied(x,y,1,0,l,lenX,lenY) : occupied += 1 
                if checkOccupied(x,y,-1,1,l,lenX,lenY) : occupied += 1 
                if checkOccupied(x,y,0,1,l,lenX,lenY) : occupied += 1 
                if checkOccupied(x,y,1,1,l,lenX,lenY) : occupied += 1 
                if l[y][x] == 'L' :
                    if occupied == 0: 
                        line += '#'
                        changed = True
                    else : line += 'L'
                elif l[y][x] == '#' :
                    if occupied >= 5 : 
                        line += 'L'
                        changed = True
                    else : line += '#'
        newl.append(line)
    l = newl


result = 0
for item in l:
    for c in item:
        if c == '#': result += 1

print ("Answer2: ", result)

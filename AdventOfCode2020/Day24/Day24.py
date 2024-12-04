#f = open("example_input.txt")
f = open("real_input.txt")
inp = f.readlines()

touched = {}

for l in inp:
    x = y = i = 0
    while i < len(l):
        c = l[i]
        if c == 'e':
            x += 1
        elif c == 'w':
            x -= 1
        elif c == 'n':
            i += 1
            c = l[i]
            y -= 1
            if c == 'e':
                x += 1
        elif c == 's':
            i += 1
            c = l[i]
            y += 1
            if c == 'w':
                x -= 1
        i += 1
    if (x , y) in touched:
        touched.pop((x, y))
    else :
        touched[(x, y)] = True

answer1 = 0
for v in list(touched.values()):
    if v : answer1 += 1

print ("Answer1: ", answer1)


def neighbours(x,y):
    return [(x-1, y), (x+1, y), (x, y -1), (x, y +1), (x-1, y+1), (x+1, y-1) ]

floor = touched
for day in range(0,100):
    newfloor = {}
    for (x,y) in floor:
        blackneigbours = 0
        for t in neighbours(x,y):
            if t in floor: blackneigbours += 1
            elif t not in newfloor :
                b2 = 0
                for t2 in neighbours(t[0],t[1]):
                    if t2 in floor: b2 += 1
                if b2 == 2 : newfloor[t] = True
        if blackneigbours in [1,2]: newfloor[(x,y)] = True
    floor = newfloor

print ("Answer2 : " , len(list(floor.keys())))
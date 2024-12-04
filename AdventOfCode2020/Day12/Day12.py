#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()


x=0
y=0
d = 1;

for instr in l:
    c = instr[0]
    by = int(instr[1:])
    if c == 'E' or ( c == 'F' and d == 1) : x += by
    elif c == 'N' or ( c == 'F' and d == 0) : y += by
    elif c == 'S' or ( c == 'F' and d == 2) : y -= by
    elif c == 'W' or ( c == 'F' and d == 3): x -= by
    elif c == 'R' :
        if by == 90 : d += 1
        elif by == 180 : d += 2
        elif by == 270 : d += 3
    elif c == 'L' :
        if by == 90 : d -= 1
        elif by == 180 : d -= 2
        elif by == 270 : d -= 3
    if d > 3 : d -= 4
    if d < 0 : d += 4
print ("Answer1: ", x, y, abs(x)+abs(y))

x=0
y=0
wpx = 10
wpy = 1
for instr in l:
    c = instr[0]
    by = int(instr[1:])
    if c == 'F' : 
        x += by * wpx
        y += by * wpy
    elif c == 'E' : wpx += by
    elif c == 'N' : wpy += by
    elif c == 'S' : wpy -= by
    elif c == 'W' : wpx -= by
    elif (c == 'R' and by == 90) or (c == 'L' and by == 270) : (wpx, wpy) = (wpy, -wpx)
    elif (c == 'R' or c == 'L') and by == 180 : (wpx, wpy) = (-wpx, -wpy)
    elif (c == 'L' and by == 90) or (c == 'R' and by == 270) : (wpx, wpy) = (-wpy, wpx)
print ("Answer2: ", x, y, abs(x)+abs(y))





#f = open("example_input.txt")
f = open("real_input.txt")

def checktrees(down, right, l, lenlx):
    x = 0;
    y = 0;
    trees = 0
    while x < len(l) - 1:
        x += down
        y += right
        if y >= lenlx : y = y - lenlx
        a = l[x][y]
        if a == '#' : trees += 1
    return trees 

l = f.readlines()

lenlx = len(l[0]) - int(l[0][len(l[0])-1] == "\n")
a = checktrees(1,1,l,lenlx)
b = checktrees(1,3,l,lenlx) 
c = checktrees(1,5,l,lenlx) 
d = checktrees(1,7,l,lenlx) 
e = checktrees(2,1,l,lenlx)
print("answer1 " , b)
print ("answer2 ", a * b * c * d * e)
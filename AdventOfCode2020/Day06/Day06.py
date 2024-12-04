
#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()
for i in range(0,len(l)):
    if l[i].endswith('\n') : l[i] = l[i][:-1]

collected = []
counted = 0
for s in l:
    if s == "" : collected = []
    else :
        for c in s:
            if c not in collected :
                collected.append(c)
                counted += 1

print ("Answer1: ", counted)

collected = []
counted = 0
first = True
for s in l:
    if s == "" :
        counted += len (collected)
        collected = []
        first = True
    else :
        collected1 = []
        for c in s:
            if first or c in collected:
                if c not in collected1 : collected1.append(c)
        collected = collected1
        first = False
counted += len (collected)
        
print ("Answer2: ", counted)

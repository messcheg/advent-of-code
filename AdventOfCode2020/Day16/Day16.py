#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

rules = {}
i = 0
while l[i] != "\n":
    rl = l[i].split(": ")
    cond = rl[1].split(" or ")
    cdi = []
    for c in cond:
        cd = c.split('-')
        cdi.append((int(cd[0]), int(cd[1])))
    rules[rl[0]] = cdi
    i += 1

myticket = []
for n in l[i+2].split(',') : myticket.append(int(n)) 
i += 5
tickets = []
while i < len(l):
    newt = []
    for n in l[i].split(',') : newt.append(int(n))
    tickets.append(newt)
    i += 1

inv = 0
validtickets = []
for t in tickets:
    discard = False
    for a in t:
        valid = False
        for r in rules:
            for (fr, to) in rules[r] :
                if fr <= a and to >= a : valid = True
        if not valid : 
            inv += a
            discard = True
    if not discard : validtickets.append(t)

print ("Answer1: ", inv)

possible = {}
for rl in rules: possible[rl] = list(range(0,len(myticket)))
for t in validtickets:
    for i, a in enumerate(t):
        for rl in rules:
            if i in possible[rl] :
                valid = False
                for (fr, to) in rules[rl]:
                    if fr <= a and to >= a : valid = True
                if not valid : 
                    possible[rl].remove(i)
    changed = True
    while changed:
        changed = False
        for rl in possible:
            if len(possible[rl]) == 1:
                for rl1 in possible:
                    if rl1 != rl and possible[rl][0] in possible[rl1]:
                        changed = True
                        possible[rl1].remove(possible[rl][0])
cnt = 1
for rl in possible:
    if rl.startswith("departure"):
        cnt *= myticket[possible[rl][0]]
print("Answer2: ", cnt)


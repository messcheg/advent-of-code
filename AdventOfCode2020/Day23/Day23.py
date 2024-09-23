#cups = [3,8,9,1,2,5,4,6,7]
cups=[8,5,3,1,9,2,6,4,7]

def playgame(currentCup, nextCups, iterations, maxi):
    for i in range(0, iterations):
        skipuntil = skipto = nextCups[currentCup]
        skip = [currentCup]
        for j in range(0, 3):
            skip.append(skipuntil)
            skipto =skipuntil
            skipuntil = nextCups[skipuntil]
        search = currentCup
        while search in skip:
            search -= 1
            if search < 1 : search = maxi
        skipfrom = nextCups[currentCup]
        nextCups[currentCup] = skipuntil
        nextCups[skipto] = nextCups[search]
        nextCups[search] = skipfrom
        a = skipfrom
        currentCup = skipuntil


nextCups = {}
for i in range(0,len(cups)-1):
    nextCups[cups[i]] = cups[i+1]
nextCups[cups[-1]] = cups[0]
currentCup = cups[0]

playgame(currentCup, nextCups, 100, 9)

cur = nextCups[1]
answer1 = ""
for i in range(0, 8) :
    answer1 += str(cur)
    cur = nextCups[cur]

print("Answer1: ", answer1)

nextCups = {}
for i in range(0,len(cups)-1):
    nextCups[cups[i]] = cups[i+1]
nextCups[cups[-1]] = 10
for i in range(10,1000000):
    nextCups[i] = i + 1
nextCups[1000000] = cups[0]
currentCup = cups[0]

playgame(currentCup, nextCups, 10000000, 1000000)

p1 = nextCups[1]
p2 = nextCups[p1]
answer1 = p1 * p2

print("Answer2: ", p1, ' * ', p2,' = ', answer1)
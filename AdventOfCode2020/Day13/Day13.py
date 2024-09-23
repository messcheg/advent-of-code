
#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

arrive = int(l[0])
notes = l[1].split(',')
buslines = []
delays = []
delay = 0
for n in notes:
    if n.isnumeric() : 
        buslines.append(int(n))
        delays.append(delay)
        delay = 0
    delay += 1

closest = -1
best = arrive
for b in buslines:
    time = ((arrive // b) + 1) * b - arrive
    if time == b : time = 0
    if time < best:
        best = time
        closest = b

print ("Answer1: ", closest, best, closest * best)

def findMatch(first, increment, diff, second):
    result1 = first + diff
    result2 = second
    if (result1 + increment < second) : result1 = ((second - diff - first) // increment ) * increment + first + diff
    if (second < result1) : result2 = (first // second) * second
    while result1 != result2:
        while result1 < result2:
            if (result2 - result1) % increment == 0 : result1 = result2
            else : result1 += ((result2 - result1) // increment + 1) * increment
        while result1 > result2: 
            if (result1 % second == 0) : result2 = result1; 
            else : result2 += ((result1 - result2 ) // second + 1 ) * second
    return result1

def findSmallestCommonProduct(first ,second):
    return findMatch(first, first, 0, second)        

scp = buslines[0];
result = buslines[0]
for i in range(1, len(buslines)):
    first = result
    second = buslines[i]
    diff = delays[i]
    incr = scp
    result = findMatch(first, incr, diff, second)
    scp = findSmallestCommonProduct(incr, second)
for d in delays : result -= d
print("Answer2: ", result)

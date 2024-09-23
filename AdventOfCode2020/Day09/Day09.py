#f = open("example_input.txt")
#preamble = 5
f = open("real_input.txt")
preamble = 25
l = f.readlines()

i= preamble
invalid = False
while not invalid and i <len(l):
    invalid = True
    for j in range( i - preamble, i - 1):
        for k in range( j + 1, i):
            if int(l[i]) == int(l[j]) + int(l[k]): invalid = False
    i += 1
print ("Answer1: ", l[i-1])

lower = 0
upper = 0
count = 0
found = False
while not found and lower < len(l):
    count = 0
    upper = lower
    while count < int(l[i-1]) and upper < len(l):
        count += int(l[upper])
        upper += 1
    found = count == int(l[i-1]) 
    lower += 1

smallest = int(l[lower - 1]) 
largest = smallest
for x in range(lower, upper):
    if int(l[x]) < smallest : smallest = int(l[x])
    if int(l[x]) > largest : largest = int(l[x])

print ("Answer2: ", smallest , largest, smallest + largest)



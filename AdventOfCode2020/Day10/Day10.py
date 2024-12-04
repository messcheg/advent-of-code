#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()
il = []
for i in l: il.append(int(i))
il.sort()

last = 0
jolt1cnt = 0
jolt3cnt = 1 
for i in il:
    if i - last == 1: jolt1cnt += 1
    elif i - last == 3: jolt3cnt += 1
    last = i
print ("Answer1: ", jolt1cnt, jolt3cnt, jolt1cnt * jolt3cnt)

il1 = [0]
il1.extend(il)
il1.append(il[-1] + 3)
pos = [1]
for x in range(1, len(il1)):
    y = x - 1
    cnt = 0
    while y >= 0 and il1[x] - il1[y] <= 3:
        cnt += pos[y]
        y -= 1
    pos.append(cnt)

print ("Answer2: ", pos[-1])
        
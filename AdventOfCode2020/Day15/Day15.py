#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()
game = []
for n in l[0].split(','):
    game.append(int(n))

for i in range(len(game), 2020):
    ago = game[i-1]
    j = i-2
    while j >= 0 and game[j] != ago:
        j -= 1
    if j >= 0 : game.append(i - 1 - j) 
    else : game.append(0)

print ("Answer1: ", game[2019])

game1 = {}

k = 1
for n in l[0].split(','):
    last = int(n)
    game1[last] = k
    lastdif = 0
    k += 1
while k < 30000000:
    if lastdif not in game1:
        newdiff = 0
    else :
        newdiff = k - game1[lastdif] 
    game1[lastdif] = k
    lastdif = newdiff
    k += 1

print ("Answer2: ", lastdif)

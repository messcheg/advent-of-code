
#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

def playgame(player1, player2):
    while len(player1) > 0 and len(player2) > 0:
        if (player1[0] > player2[0]) :
            player1.append(player1[0])
            player1.append(player2[0])
        else :
            player2.append(player2[0])
            player2.append(player1[0])
        player1 = player1[1:]
        player2 = player2[1:]
    if len(player1) > 0 : return player1
    return player2

def seenbefore(pp1, p1, p2):
    for i in range(1, min(len(p1),len(p2))):
        if p1[i] in pp1 :
            pp2 = pp1[p1[i]]
        else :
            return False
        if p2[i] in pp2 :
            pp1 = pp2[p2[i]]
        else :
            return False 
    for i in range(min(len(p1),len(p2)), max(len(p1),len(p2))):
        if (i < len(p1)) : x1 = p1[i]
        else : x1 = -1
        if (i < len(p2)) : x2 = p2[i]
        else : x2 = -1       
        if x1 in pp1 :
            pp2 = pp1[x1]
        else :
            return False
        if x2 in pp2 :
            pp1 = pp2[x2]
        else :
            return False
    return True

def addsee(pp1, p1, p2):
    for i in range(1, min(len(p1),len(p2))):
        if p1[i] in pp1 :
            pp2 = pp1[p1[i]]
        else :
            pp2 = pp1[p1[i]] = {}
        if p2[i] in pp2 :
            pp1 = pp2[p2[i]]
        else :
            pp1 = pp2[p2[i]] = {}
    for i in range( min(len(p1),len(p2)), max(len(p1),len(p2))):
        if (i < len(p1)) : x1 = p1[i]
        else : x1 = -1
        if (i < len(p2)) : x2 = p2[i]
        else : x2 = -1       
        if x1 in pp1 :
            pp2 = pp1[x1]
        else :
            pp2 = pp1[x1] = {}
        if x2 in pp2 :
            pp1 = pp2[x2]
        else :
            pp1 = pp2[x2] = {}

def playrecursivegame(player1, player2):
    pp = {} 
    while len(player1) > 0 and len(player2) > 0:
        if seenbefore(pp, player1, player2) :
            return (1, player1)
        else :
            addsee(pp, player1, player2)
        p1 = player1[0]
        p2 = player2[0]
        if (p1 < len(player1) and p2 < len(player2)):
            (winner, winlist) = playrecursivegame(player1[1:p1 + 1], player2[1:p2 + 1])
        elif p1 > p2:
            winner = 1
        else :
            winner = 2

        if winner == 1 :
            player1.append(p1)
            player1.append(p2)
        else :
            player2.append(p2)
            player2.append(p1)
        player1 = player1[1:]
        player2 = player2[1:]
    if len(player1) > 0 : return (1, player1)
    return (2, player2)


players = []
player = []
for li in l:
    if li.endswith('\n') : li = li[:-1]
    if li.isnumeric() : player.append(int(li))
    elif li == '' :
        players.append(player)
        player = []
players.append(player)

winnerlist = playgame(players[0].copy(), players[1].copy())

answer1 = 0
for i in range(1, len(winnerlist) + 1) :
    answer1 += winnerlist[-i] * i

print("Answer1: ", answer1)

(winner, winnerlist) = playrecursivegame(players[0].copy(), players[1].copy())

answer2 = 0
for i in range(1, len(winnerlist) + 1) :
    answer2 += winnerlist[-i] * i

print("Answer2: ", answer2)

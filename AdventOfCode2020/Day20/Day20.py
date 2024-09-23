import math
from os import sep

#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

tile = []
tiles = {}
tilenr = 0
for li in l:
    if li.endswith('\n') : li = li[:-1]
    if li.startswith("Tile") : 
        tilenr = int(li[5:-1])
        tile = []
    elif li == "" : tiles[tilenr] = tile
    else : tile.append(li)
tiles[tilenr] = tile

def doprint(tile):
    for line in tile: print(line)
    print("-------------------------")

def rotate(tile):
    result = []
    for j in range(0, len(tile)):
        line = ""
        for i in range(0, len(tile[0])):
            line = line + tile[i][-1-j]
        result.append(line)
    return result

def fliptile(tile):
    result = []
    for i in range(0, len(tile)):
        line = ""
        for j in range(0, len(tile)):
            line = line + tile[i][-1-j]
        result.append(line)
    return result

def match(tile1, tile2, side) :
    matches = True
    i = 0
    while i < len(tile1) and matches:
        if side == 'r' :
            c1 = tile1[i][-1]
            c2 = tile2[i][0] 
        elif side == 'l' :
            c1 = tile1[i][0]
            c2 = tile2[i][-1]
        elif side == 't' :
            c1 = tile1[0][i]
            c2 = tile2[-1][i]
        else :
            c1 = tile1[-1][i]
            c2 = tile2[0][i]
        matches = c1 == c2
        i += 1
    return matches
            

def trytomatch (tile1, tile2, side):
    matches = False
    flip = 0
    while flip < 2 and not matches:
        rot = 0
        while rot < 4 and not matches:
            #doprint(tile2)
            matches = match(tile1, tile2, side)
            if matches :
               return (True, rot, flip, tile2)
            rot += 1
            tile2 = rotate(tile2)
        flip += 1
        tile2 = fliptile(tile2)
    return (False, 0, 0, tile2)


def findmatch(tile1, keys, tiles, rm, side) :
    k = 0
    matched = False
    while not matched and k < len(keys) : 
        t = keys[k]
        if t not in rm :
            tile2 = tiles[t]
            (matched, rot, flip, tile2) = trytomatch(tile1, tile2, side)
        k += 1
    return (matched, rot, flip, t, tile2)

 
rm = []
keys = []
for t in tiles.keys() : keys.append(t)

#for i1, t1 in enumerate(tiles.keys()):
#    for t2 in keys[i1 + 1:]:
#        (match, rot, flip) = trytomatch(t1, t2, tiles, 'r')
#        if match : rm.append((t1, t2, 'r', rot, flip))

size = int(math.sqrt(len(keys)))
puzzle = []
had = {}
for i in range(0, size) :
    strip = []
    ref = ' '
    for j in range(0, size) :
        if len(puzzle) == 0 and len(strip) == 0 : 
            strip.append((keys[0],tiles[keys[0]]))
            had[keys[0]] = True
        else :
            if len(strip) > 0 : 
                (number1, tile1) = strip[-1] 
                (matched, rot, flip, number2, tile2) = findmatch(tile1,keys,tiles, had, 'r')
                if matched :
                    had[number2] = True
                    strip.append((number2, tile2))
                else:
                    (number1, tile1) = strip[0]
                    (matched, rot, flip, number2, tile2) = findmatch(tile1,keys,tiles, had, 'l')
                    if matched :
                        had[number2] = True
                        strip.insert(0,(number2, tile2)) 
            else :
                (number1, tile1) = puzzle[-1][0]
                (matched, rot, flip, number2, tile2) = findmatch(tile1,keys,tiles, had,  'b')
                if matched :
                    had[number2] = True
                    strip = [(number2, tile2)]
                    ref = 'b'
                else :
                    (number1, tile1) = puzzle[0][0]
                    (matched, rot, flip, number2, tile2) = findmatch(tile1,keys,tiles, had,  't')
                    if matched :
                        had[number2] = True
                        strip = [(number2, tile2)]
                        ref = 't'
    if ref == 't':
        puzzle.insert(0, strip)
    else :
        puzzle.append(strip)           

(topl, tile) = puzzle[0][0]
(topr, tile) = puzzle[0][-1]
(botl, tile) = puzzle[-1][0]
(botr, tile) = puzzle[-1][-1]

print("Answer1: " , topl , " * ", topr , " * ", botl , " * ", botr , " = " , (topl*topr*botl*botr))

sea = []
for k, tilerow in enumerate(puzzle):
    for i, (number, tile) in enumerate(tilerow):
        for j in range(1, len(tile[0]) - 1):
            if i == 0 :
                sea.append(tile[j][1:-1])
            else :
                idx = k * (len(tile[0])-2)+ j -1 
                sea[idx] = sea[idx] + tile[j][1:-1]

doprint(sea)

seasnake = ["                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   "]            

def checksnakepart(sealine, snake):
    for i in range(0,len(snake)):
        if snake[i] == '#' and sealine[i] != '#' : return False
    return True

def paintsnake(sealine, start, snake):
    result = sealine[0:start]
    for i in range(0,len(snake)):
        if snake[i] == '#' : result += 'O'
        else : result += sealine[start + i]
    result += sealine[start+len(snake):]
    return result


def findsnakes(sea,seasnake): 
    snakes = 0
    snakelen = len(seasnake[0])
    snakehigh = len (seasnake)
    for i in range(snakehigh - 1, len(sea)):
        for j in range(snakelen, len(sea[0]) + 1):
            match = True                
            for k in range(0, snakehigh):
                if match :
                   match = checksnakepart(sea[i-k][j - snakelen : j], seasnake[-1-k])
            if match : 
                for k in range(0, snakehigh):
                    sea[i-k] = paintsnake(sea[i-k], j-snakelen, seasnake[-1-k])
                snakes += 1
    return (snakes, sea)

def tryfindsnakes (sea, seasnake):
    flip = 0
    while flip < 2:
        rot = 0
        while rot < 4:
            (snakes, newsea) = findsnakes(sea, seasnake)
            if snakes > 0:
               return (snakes, newsea)
            rot += 1
            sea = rotate(sea)
        flip += 1
        sea = fliptile(sea)
    return 0

(snakes, newsea) = tryfindsnakes(sea,seasnake)
answer2 = 0
for l in newsea:
    for c in l:
        if c == '#' : answer2 += 1

doprint(newsea)
print ("Answer2: ", answer2 )
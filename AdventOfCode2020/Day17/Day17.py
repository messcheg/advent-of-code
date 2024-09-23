f = open("example_input.txt")
#f = open("real_input.txt")
l = f.readlines()

def checkfield(x,y,z, w, hcubes):
    if x>0 and y>0 and z>0 and w>0 and w <= len(hcubes) and z <= len(hcubes[0]) and y <= len(hcubes[0][0]) and x <= len(hcubes[0][0][0]):
        return hcubes[w-1][z-1][y-1][x-1]
    return '.'

for turn in range(1,3) :
    sizeW = 1
    sizeZ = 1
    sizeY = len(l)
    sizeX = len(l[0])
    if l[0].endswith('\n') : sizeX -= 1

    hcubes = []
    cubes = []
    plate = []
    for li in l:
        plate.append(li[0:sizeX])
    cubes.append(plate)
    hcubes.append(cubes)

    for i in range(0,6):
        sizeZ += 2
        sizeY += 2
        sizeX += 2
        if turn == 2 : sizeW += 2
        newhcubes =[]
        for w in range(0, sizeW):
            newcubes = []
            for z in range(0, sizeZ):
                newplate = []
                for y in range(0,sizeY):
                    newrow = ""
                    for x in range (0,sizeX):
                        cnt = 0
                        x1 = x - 1
                        while x1 < x+2 and cnt < 4:
                            y1 = y -1
                            while y1 < y+2 and cnt < 4:
                                z1 = z -1
                                while z1 < z+2 and cnt < 4:
                                    w1 = w - 1
                                    while w1 < w + 2 and cnt < 4:
                                        if not (x1 == x and y1 == y and z1== z and w1 == w) and checkfield(x1, y1, z1, w1, hcubes) == '#' : cnt += 1
                                        w1 += 1
                                    z1 += 1
                                y1 += 1
                            x1 += 1
                        cur = checkfield(x,y,z, w, hcubes)
                        if cur == '#' and not (cnt == 2 or cnt == 3) : newrow += '.'
                        elif cur == '.' and cnt == 3 : newrow += '#'
                        else : newrow += cur
                    newplate.append(newrow)
                newcubes.append(newplate)
            newhcubes.append(newcubes)
        hcubes = newhcubes
        #print ("-------[Cube", i,"]----------")
        #for q, p in enumerate(cubes):
        #    print ("-------[Plate " , q, "]----------")
        #    for r in p:
        #        print(r)

    cnt = 0
    for cb in hcubes:
        for p in cb:
            for r in p:
                for c in r: 
                    if c == '#' : cnt += 1

    print ("Answer" turn, ": ", cnt)


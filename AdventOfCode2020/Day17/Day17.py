#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

def checkfield(x,y,z,cubes):
    if x>0 and y>0 and z>0 and z <= len(cubes) and y <= len(cubes[0]) and x <= len(cubes[0][0]):
        return cubes[z-1][y-1][x-1]
    return '.'

sizeZ = 1
sizeY = len(l)
sizeX = len(l[0])
if l[0].endswith('\n') : sizeX -= 1

cubes = []
plate = []
for li in l:
    plate.append(li[0:sizeX])
cubes.append(plate)

for i in range(0,6):
    newcubes = []
    sizeZ += 2
    sizeY += 2
    sizeX += 2
    for z in range(0, sizeZ):
        newplate = []
        for y in range(0,sizeY):
            newrow = ""
            for x in range (0,sizeX):
                cnt = 0
                for x1 in range(x-1, x+2):
                    for y1 in range(y-1,y+2):
                        for z1 in range(z-1, z+2):
                            if not (x1 == x and y1 == y and z1== z) and checkfield(x1, y1, z1, cubes) == '#' : cnt += 1
                cur = checkfield(x,y,z,cubes)
                if cur == '#' and not (cnt == 2 or cnt == 3) : newrow += '.'
                elif cur == '.' and cnt == 3 : newrow += '#'
                else : newrow += cur
            newplate.append(newrow)
        newcubes.append(newplate)
    cubes = newcubes
    #print ("-------[Cube", i,"]----------")
    #for q, p in enumerate(cubes):
    #    print ("-------[Plate " , q, "]----------")
    #    for r in p:
    #        print(r)

    cnt = 0
for p in cubes:
    for r in p:
        for c in r: 
            if c == '#' : cnt += 1

print ("Answer1: ", cnt)


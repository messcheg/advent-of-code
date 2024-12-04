
#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()
for i in range(0,len(l)):
    if l[i].endswith('\n') : l[i] = l[i][:-2] 
    else : l[i] = l[i][:-1] 

rules = {}
for s in l:
    rule = s.split(" bags contain ")
    content = rule[1].replace(" bags", '').replace(" bag", '').split(', ')
    contents = {}
    for co in content:
        cont = co.split(' ')
        if cont[0] == 'no' :
            contents[co] = 0
        else :
            contents[cont[1] + ' ' + cont[2] ] = int(cont[0])
    rules[rule[0]] = contents

goldbags = {}
bagsinside = {}
while len(goldbags) < len(rules):
    for key in rules:
        if key not in goldbags:
            val = rules[key]
            if len(val) == 1 and "no other" in val:
                goldbags[key] = 0
                bagsinside[key] = 0
            else :
                countgold = 0
                countinside = 0
                allfound = True
                for valkey in val:
                    countinside += val[valkey]
                    if valkey == "shiny gold" : countgold += val["shiny gold"]
                    if valkey in goldbags:
                        countgold += val[valkey] * goldbags[valkey]
                        countinside += val[valkey] * bagsinside[valkey]
                    else :
                        allfound = False
                if allfound:
                    goldbags[key] = countgold
                    bagsinside[key] = countinside
countpossible = 0
for val1 in goldbags.values():
    if val1 > 0 : countpossible += 1

print ("Answer1: " , countpossible)
print ("Answer2: ", bagsinside["shiny gold"])



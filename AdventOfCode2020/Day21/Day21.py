#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

allergycard = {}
dishes = []
for lin in l:
    s = lin[:-1-int(lin.endswith('\n'))].split(" (contains ")
    dishes.append([s[0].split(' '), s[1].split(', ')])
    for a in dishes[-1][1] :
        if a in allergycard:
            allergycard[a] = list(set(allergycard[a]) & set(dishes[-1][0]))
        else :
            allergycard[a] = dishes[-1][0]
ingredients = {}

changed = True
while changed:
    changed = False
    for al in allergycard :
        ln = len(allergycard[al]) 
        if ln == 1 : 
            ing = allergycard[al][0]
            if ing not in ingredients:
               ingredients[ing] = al
               changed = True
        elif ln > 1 :
            newing = []
            for ing in allergycard[al]:
                if ing not in ingredients:
                    newing.append(ing)
                else : changed = True
            allergycard[al] = newing
 
answer1 = 0
for di in dishes:
    for p in di[0]:
        if p not in ingredients : answer1 += 1

print ("Answer1: " , answer1)

comma = ""
result = ""
liAlg = list(allergycard.keys())
liAlg.sort()
for alg in liAlg :
    result += comma + allergycard[alg][0]
    comma = ","
print ("Answer2: ", result)
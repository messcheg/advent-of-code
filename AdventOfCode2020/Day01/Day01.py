#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()
i = 0
while i < len(l):
    j = i + 1
    while j < len(l):
        if (int(l[i]) + int(l[j]) == 2020):
            x = l[i]
            y = l[j] 
        k = j + 1
        while k < len(l):
            if (int(l[i]) + int(l[j]) + int(l[k]) == 2020):
                p = l[i]
                q = l[j]
                r = l[k]
            k += 1
        j += 1
    i += 1
print ("x", x)
print ("y", y)
print (int(x) * int(y))
print ("p", p)
print ("q", q)
print ("r", r)
print (int(p) * int(q) * int(r))

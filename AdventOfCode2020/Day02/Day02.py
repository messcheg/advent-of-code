#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()
valid = 0
for l1 in l:
    l2 = l1.split(' ')
    pwd = l2[2]
    rule = l2[1][0]
    pol = l2[0].split('-')
    cnt = 0 
    for c in pwd: 
        if c == rule : cnt += 1
    if int(pol[0]) <= cnt and int(pol[1] ) >= cnt: valid += 1
print ("answer1 ", valid)
    
valid = 0
for l1 in l:
    l2 = l1.split(' ')
    pwd = l2[2]
    rule = l2[1][0]
    pol = l2[0].split('-')
    a = pwd[int(pol[0])-1] == rule  
    b = pwd[int(pol[1])-1] == rule
    if (a and not b) or ( b and not a): valid += 1
print ("answer2 ", valid)

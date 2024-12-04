#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

def readnumber(i, line, version):
    number = 0
    if line[i] in '0123456789':
        number = int(line[i])
        i += 1
    elif line[i] == '(':
        sub = 1
        k = i + 1
        while sub > 0:
            if line[k] == '(' : sub += 1
            elif line[k] == ')' : sub -= 1
            k += 1
        if version == 1 : number = calculate(line[i+1:k-1]) 
        elif version == 2 : number = calculate2(line[i+1:k-1]) 
        i = k
    return (i, number)

def calculate(line):
    i = 0
    (i, subresult) = readnumber(i, line, 1)     
    while i < len(line) and line[i] != '\n':
        operator = line[i+1]
        (i, number2) = readnumber(i + 3,line ,1)
        if (operator == '*') : subresult *= number2
        else : subresult += number2
    return subresult

def readaddgroups(i, line):
    (i, subresult) = readnumber(i, line, 2)     
    while i < len(line) and line[i] != '\n':
        operator = line[i+1]
        if operator == '+':
            (i, number2) = readnumber(i+3, line, 2)
            subresult += number2
        else : return (i, subresult)
    return (i, subresult)
            

def calculate2(line):
    i = 0 
    (i, subresult) = readnumber(i, line, 2)     
    while i < len(line) and line[i] != '\n':
        operator = line[i+1]
        if (operator == '*') : 
            (i, number2) = readaddgroups(i+3, line)
            subresult *= number2
        else : 
            (i, number2) = readnumber(i + 3,line, 2)
            subresult += number2
    return subresult


cnt = 0
for line in l:
    r = calculate(line)
    cnt += r
    print (r)

print ("Answer1 = " , cnt)

cnt = 0
for line in l:
    r = calculate2(line)
    cnt += r
    print (r)

print ("Answer2 = " , cnt)
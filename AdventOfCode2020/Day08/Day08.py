#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

#errorcodes: 0 - ok, 1 - invalidjump, 2 - infinite
def runprogram(program):
    ranbefore = {}
    acc = 0
    pc = 0
    infiniteloop = False
    invalidjump = False
    finished = False
    while not (infiniteloop or invalidjump or finished):
        if pc in ranbefore : infiniteloop = True
        elif pc == len(program) : finished = True
        elif pc < 0 or pc > len(program) : invalidjump = True
        else :
            ranbefore[pc] = 1
            instruction = program[pc].split(' ')
            opcode = instruction[0]
            parameter = int(instruction[1])
            if opcode == "nop":
                pc += 1
            elif opcode == "acc":
                acc += parameter
                pc += 1
            elif opcode == "jmp":
                pc += parameter
            
    if infiniteloop : return [acc, 2]
    if invalidjump : return [acc, 1]
    return [acc,0]

result = runprogram(l)
print ("Answer1: ", result[0])

i = 0
while result[1] > 0 and i < len(l):
    if l[i].startswith('nop') :
        l[i] = l[i].replace('nop', 'jmp')
        result = runprogram(l)
        l[i] = l[i].replace('jmp','nop')
    elif l[i].startswith('jmp') :
        l[i] = l[i].replace('jmp','nop')
        result = runprogram(l)
        l[i] = l[i].replace('nop', 'jmp')
    i += 1

print ("Answer2: ", result[0])

        
        

    

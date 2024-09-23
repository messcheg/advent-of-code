#f = open("example_input.txt")
#f = open("example_input2.txt")
f = open("real_input.txt")

def checkrules(m, rules, rule, path):
    if (rule, m) in path : return [[False, '']] # loop situation
    else : 
        path1 = path.copy()
        path1.append((rule,m))
    currentrule = rules[rule]
    if len(currentrule) == 1 and currentrule[0][0].startswith('"'):
        if len(m) == 0 : return [[False, '']]
        return [[m[0] == currentrule[0][0][1] ,m[1:]]]
    i = 0
    finalresults = []
    while i < len(currentrule):
        j = 0
        currentsubrule = currentrule[i]
        results = [[True, m]]        
        while len(results) > 0 and j < len(currentsubrule):
            results1 = []
            for sr in results : 
                if sr[0] :
                    results1.extend(checkrules(sr[1], rules, currentsubrule[j], path1))
            results = results1
            j += 1
        for r in results :
            if r[0] : finalresults.append(r)
        i += 1
    if len(finalresults) > 0 : return finalresults
    else : return [[False, m]]
        

def checkrule(m, rules, rule):
    result = checkrules(m, rules, rule, [])
    for r in result:
        if r[0] and r[1] == '' : return True
    return False

l = f.readlines()
nl = []
for s in l :
    if s.endswith('\n') : nl.append(s[:-1])
    else : nl.append(s)

rules = {}
i = 0
while nl[i] != '' : 
    rule = []
    snl = nl[i].split(': ') 
    for rl in snl[1].split(' | ') :
        rule.append(rl.split(' '))
    rules[snl[0]] = rule
    i += 1
i += 1
msgs = nl[i:] 

correct = []
for m in msgs:
    if checkrule(m, rules, '0'): correct.append(m)

print ("Answer1: ", len(correct))

rules['8'] = [['42'],['42','8']]
rules['11'] = [['42', '31'],['42','11','31']] 
correct = []
for m in msgs:
    if checkrule(m, rules, '0'): correct.append(m)

print ("Answer2: ", len(correct))

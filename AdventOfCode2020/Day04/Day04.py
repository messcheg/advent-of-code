#f = open("example_input.txt")
f = open("real_input.txt")
l = f.readlines()

def IsValidPassport(pp):
    checker = 0;
    if len(pp) < 7 : return False
    for field in pp:
        fld = field.split(':')[0]
        val = field.split(':')[1]
        if fld == 'byr'  : 
            if len(val) == 4 and int(val) >= 1920 and int(val) <= 2002 :
                checker = checker | 1
        elif fld == 'iyr' :
            if len(val) == 4 and int(val) >= 2010 and int(val) <= 2020 :
                 checker = checker | 2
        elif fld == 'eyr'  :
            if len(val) == 4 and int(val) >= 2020 and int(val) <= 2030 :
                 checker = checker | 4
        elif fld == 'hgt'  : 
            if (val.endswith('cm') and int(val[:-2]) >= 150 and int(val[:-2]) <= 193) or (val.endswith('in') and int(val[:-2]) >= 59 and int(val[:-2]) <= 76) :
                checker = checker | 8
        elif fld == 'hcl'  : 
            if len(val) == 7 and val[0] == '#':
                isok = True
                for i in range(1,6): 
                    if not val[i] in "0123456789abcdef" : isok = False
                if isok :
                    checker = checker | 16
        elif fld == 'ecl'  : 
            if val in ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"]:
                checker = checker | 32
        elif fld == 'pid'  : 
            if len(val) == 9 and val.isdigit() :
                checker = checker | 64
        elif fld == 'cid'  : checker = checker | 128
    return checker == 255 or checker == 127   

passports = []
currentpp = []
for lin in l:
    if lin.endswith('\n') : lin = lin[:-1]
    if lin == "":
        passports.append(currentpp);
        currentpp = []
    else :
        currentpp.extend(lin.split(' '))
passports.append(currentpp);

validcnt = 0
for pp in passports:
    if IsValidPassport(pp): validcnt += 1
print ("Answer1: ", validcnt)


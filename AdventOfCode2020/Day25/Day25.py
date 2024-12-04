#cardKey = 5764801
#doorKey = 17807724

cardKey = 5099500
doorKey = 7648211

def calculateloopsize(key):
    value = 1
    loopsize = 0
    while value != key:
        value *= 7
        value %= 20201227
        loopsize += 1
    return loopsize

cardloopsize = calculateloopsize(cardKey)
doorloopsize = calculateloopsize(doorKey)

print("Card loop size: " , cardloopsize)
print("Door loop size: " , doorloopsize)

privatkey = 1
for i in range(0, cardloopsize) :
    privatkey = (privatkey * doorKey) % 20201227

print ("Private key: ", privatkey)
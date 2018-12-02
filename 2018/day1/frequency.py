#!/usr/bin/env python3
with open('frequency.data', 'r') as f:
    values = f.read().splitlines()


result = 0
for line in values:
    result = result + int(line)

print(f"Puzzle 1: {result}")

result = 0
freq_seen = set()

while True:
    solved = False
    for line in values:
        result = result + int(line)
        if(result in freq_seen):
            print(f"Puzzle 2: {result}")
            solved = True
            break
        freq_seen.add(result)
    if(solved):
        break
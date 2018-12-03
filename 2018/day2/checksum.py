#!/usr/bin/env python3
from collections import Counter

with open('checksum.data', 'r') as f:
    values = f.read().splitlines()


seen_twice = 0
seen_thrice = 0

for row in values:
    twice = False
    thrice = False
    counter = Counter(row)
    for count in counter.most_common():
        if(count[1] == 3):
            thrice = True
        if(count[1] == 2):
            twice = True
    if twice:
        seen_twice += 1
    if thrice:
        seen_thrice += 1
print(f"Puzzle 1: {seen_twice * seen_thrice}")

min_differing = None
differing_pair = None
for row1 in values:
    for row2 in values:
        if(row1 == row2):
            continue

        differing = 0
        for i in range(0, len(row1)):
            c1 = row1[i]
            c2 = row2[i]

            if(c1 != c2):
                differing += 1

        if(min_differing == None or differing < min_differing):
            min_differing = differing
            differing_pair = (row1, row2)

string_match = ""
for i in range(0, len(differing_pair[0])):
    diff1 = differing_pair[0][i]
    diff2 = differing_pair[1][i]
    if(diff1 == diff2):
        string_match += diff1

print(f"Puzzle 2: {string_match}")
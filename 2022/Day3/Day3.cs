using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day3 : Solver {

    Dictionary<char, int> priorities = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
            .Select((l, i) => (l, i: i + 1))
            .ToDictionary(p => p.l, p => p.i);

    public override void PartOne() {
        var sumPriority = 0;

        foreach (var rucksack in Input) {
            var left = rucksack.Take(rucksack.Length / 2);
            var right = rucksack.TakeLast(rucksack.Length / 2);

            sumPriority += left.Where(l => right.Contains(l)).Distinct().Select(l => priorities[l]).Sum();
        }

        Console.WriteLine($"Sum of priorities: {sumPriority}");
    }

    public override void PartTwo() {
        var sumPriority = 0;
        var index = 0;

        while (index < Input.Length) {
            var group = Input.Skip(index).Take(3).ToArray();

            var badge = group
                .Skip(1)
                .Aggregate(
                    new HashSet<char>(group.First()),
                    (h, e) => { h.IntersectWith(e); return h; }
                ).First();
            
            sumPriority += priorities[badge];
            index += 3;
        }

        Console.WriteLine($"Sum of priorities: {sumPriority}");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021;

class Day8 : Solver {
    public override void PartOne() {
        var wireCount = new int[] {2, 4, 3, 7};
        var count = Input
            .Select(row => row.Split("|")[1].Trim().Split(" "))
            .Select(output => output.Where(d => wireCount.Contains(d.Length)))
            .SelectMany(d => d)
            .Count();

        Console.WriteLine($"Count: {count}");
    }

    public override void PartTwo() {
        var sum = 0;

        foreach (var entry in Input) {
            var puzzleDigits = entry.Replace(" | ", " ").Split(" ")
                .Select(d => new String(d.Order().ToArray()))
                .Distinct()
                .ToArray();

            var digitWires = puzzleDigits
                .Select(d => d.Length switch {
                    2 => (1, d),
                    4 => (4, d),
                    3 => (7, d),
                    7 => (8, d),
                    _ => (0, d)
                })
                .Where(d => d.Item1 != 0)
                .ToDictionary(d => d.Item1, d => d.d);
            
            var wireMappings = new Dictionary<char, char>(); // Standard -> Puzzle

            wireMappings['a'] = digitWires[7].Except(digitWires[1]).First();

            digitWires[9] = puzzleDigits
                .Where(d => !digitWires.ContainsValue(d))
                .Where(d => digitWires[4].All(d.Contains))
                .First();

            wireMappings['g'] = digitWires[9]
                .Except(digitWires[4]) // Left with wires a and g
                .Where(w => w != wireMappings['a'])
                .First();

            digitWires[0] = puzzleDigits
                .Where(d => !digitWires.ContainsValue(d))
                .Where(d => digitWires[1].All(d.Contains)) // 0, 3
                .Where(d => d.Length == 6)
                .First();

            digitWires[3] = puzzleDigits
                .Where(d => !digitWires.ContainsValue(d))
                .Where(d => digitWires[1].All(d.Contains))
                .First();
            
            digitWires[6] = puzzleDigits
                .Where(d => !digitWires.ContainsValue(d))
                .Where(d => d.Length == 6)
                .First();
            
            wireMappings['f'] = digitWires[1]
                .Intersect(digitWires[6])
                .First();

            digitWires[5] = puzzleDigits
                .Where(d => !digitWires.ContainsValue(d))
                .Where(d => d.Contains(wireMappings['f']))
                .First();
            
            digitWires[2] = puzzleDigits
                .Where(d => !digitWires.ContainsValue(d))
                .First();

            var puzzleMapping = digitWires.ToDictionary(d => d.Value, d => d.Key); // Invert mapping

            var output = entry
                .Split(" | ")[1].Split(" ")
                .Select(d => puzzleMapping[new String(d.Order().ToArray())])
                .Select(d => d.ToString())
                .Aggregate((s, d) => s += d);

            sum += Int32.Parse(output);
        }

        Console.WriteLine($"Sum of output: {sum}");
    }
}
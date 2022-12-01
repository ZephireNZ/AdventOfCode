using System;
using System.Linq;

namespace AdventOfCode.Y2021.Day1;

class Day1 : Solver {
    public override int Year { get; } = 2021;
    public override int Day { get; } = 1;

    public override void PartOne() {
        var input = Input;

        Console.WriteLine($"Read {Input.Length} lines of input");

        int currentDepth = Convert.ToInt32(input[0]);
        int increaseCount = 0;

        foreach (var depthStr in input.Skip(1))
        {
            var depth = Convert.ToInt32(depthStr);

            if (depth > currentDepth) {
                increaseCount += 1;
            }

            currentDepth = depth;
        }

        Console.WriteLine($"Total Depth Increases: {increaseCount}");
    }

    public override void PartTwo() {
        var input = Input.Select(i => Convert.ToInt32(i)).ToArray();

        Console.WriteLine($"Read {Input.Length} lines of input");

        var prevAvg = (input[0] + input[1] + input[2]);
        int increaseCount = 0;

        for (var x = 1; x < input.Length - 3 + 1; x++) {
            var windowAvg = (input[x] + input[x+1] + input[x+2]);

            if (windowAvg >  prevAvg) {
                increaseCount += 1;
            }

            prevAvg = windowAvg;
        }

        Console.WriteLine($"Total Depth Increases: {increaseCount}");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021;

class Day7 : Solver {
    public override void PartOne() {
        var input = InputRaw.Split(",").Select(Int32.Parse).ToArray();

        var max = input.Max();
        var min = input.Min();
        
        var minFuel = Enumerable
            .Range(min, (max-min))
            .Select(x => input.Select(c => Math.Abs(x - c)).Sum())
            .Min();

        Console.WriteLine($"Minimum fuel: {minFuel}");
    }

    public override void PartTwo() {
        var input = InputRaw.Split(",").Select(Int32.Parse).ToArray();

        var max = input.Max();
        var min = input.Min();
        
        var minFuel = Enumerable
            .Range(min, (max-min))
            .Select(x => {
                return input
                    .Select(c => Math.Abs(x - c))
                    .Select(d => Enumerable.Range(1, d).Sum())
                    .Sum();
            })
            .Min();

        Console.WriteLine($"Minimum fuel: {minFuel}");
    }
}
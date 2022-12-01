using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day1;

class Day1 : Solver {
    public override int Year { get; } = 2022;
    public override int Day { get; } = 1;

    public override void PartOne() {
        var input = Input
            .Select(i => Int32.TryParse(i, out var tempVal) ? tempVal : (int?) null).ToList();

        Console.WriteLine($"Read {Input.Length} lines of input");

        var elves = new List<int>();
        var currentElf = 0;

        foreach (var item in input) {
            if (item is null) {
                elves.Add(currentElf);
                currentElf = 0;
            } else {
                currentElf += (int) item;
            }
        }

        Console.WriteLine($"Most calories held: {elves.Max()}");
    }

    public override void PartTwo() {
        var input = Input
            .Select(i => Int32.TryParse(i, out var tempVal) ? tempVal : (int?) null).ToList();

        Console.WriteLine($"Read {Input.Length} lines of input");

        var elves = new List<int>();
        var currentElf = 0;

        foreach (var item in input) {
            if (item is null) {
                elves.Add(currentElf);
                currentElf = 0;
            } else {
                currentElf += (int) item;
            }
        }

        var topThree = elves.OrderDescending().Take(3).Sum();

        Console.WriteLine($"Top 3 elves calories: {topThree}");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021;

class Day2 : Solver {
    public override void PartOne() {
        var input = Input;
        Console.WriteLine($"Read {input.Length} lines of input");

        var distance = 0;
        var depth = 0;

        foreach (var commandStr in input) {
            switch (commandStr.Split(" ")) {
                case ["forward", var value]:
                    distance += Convert.ToInt32(value);
                    break;
                case ["up", var value]:
                    depth -= Convert.ToInt32(value);
                    break;
                case ["down", var value]:
                    depth += Convert.ToInt32(value);
                    break;
            }
        }

        Console.WriteLine($"Distance: {distance}, Depth: {depth}, Multiplied: {distance*depth}");
    }

    public override void PartTwo() {
        var input = Input;
        Console.WriteLine($"Read {input.Length} lines of input");

        var distance = 0;
        var depth = 0;
        var aim = 0;

        foreach (var commandStr in input) {
            switch (commandStr.Split(" ")) {
                case ["forward", var value]:
                    distance += Convert.ToInt32(value);
                    depth += aim*Convert.ToInt32(value);
                    break;
                case ["up", var value]:
                    aim -= Convert.ToInt32(value);
                    break;
                case ["down", var value]:
                    aim += Convert.ToInt32(value);
                    break;
            }
        }

        Console.WriteLine($"Distance: {distance}, Depth: {depth}, Multiplied: {distance*depth}");
    }
}
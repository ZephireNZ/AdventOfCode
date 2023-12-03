using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023;

class Day2 : Solver {
    public override void PartOne()
    {
        var result = 0;

        foreach (var game in Input)
        {
            var (id, (rounds, _)) = game.Split(": ");

            var isPossible = !rounds
                .Split("; ")
                .SelectMany(round => round.Split(", "))
                .Any(pull => pull.Split(" ") switch
                {
                    [var red, "red"] => int.Parse(red) > 12,
                    [var green, "green"] => int.Parse(green) > 13,
                    [var blue, "blue"] => int.Parse(blue) > 14,
                    _ => throw new InvalidOperationException(),
                });

            if (isPossible) result += int.Parse(id.Split(" ").Last());
        }

        Console.WriteLine($"Part 1: {result}");
    }

    public override void PartTwo()
    {
        var result = 0;

        foreach (var game in Input)
        {
            var (_, (rounds, _)) = game.Split(": ");

            var minBag = rounds
                .Split("; ")
                .SelectMany(round => round.Split(", "))
                .Aggregate(new { red = 0, green = 0, blue = 0 },
                    (minBag, pull) => pull.Split(" ") switch
                    {
                        [var red, "red"] => minBag with { red = Math.Max(int.Parse(red), minBag.red) },
                        [var green, "green"] => minBag with { green = Math.Max(int.Parse(green), minBag.green) },
                        [var blue, "blue"] => minBag with { blue = Math.Max(int.Parse(blue), minBag.blue) },
                        _ => throw new InvalidOperationException(),
                    });

            result += minBag.red * minBag.green * minBag.blue;
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
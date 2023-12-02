using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023;

class Day1 : Solver {
    public override void PartOne()
    {
        var result = Input.Select(line =>
        {
            var matches = Regex.Matches(line, @"\d").Select(m => m.Value).ToList();

            return int.Parse(matches.First() + matches.Last());
        }).Sum();

        Console.WriteLine($"Part One: {result}");
    }

    public override void PartTwo()
    {
        var matchString = new Regex(@"(?=(\d|one|two|three|four|five|six|seven|eight|nine))",
            RegexOptions.Compiled);

        var result = Input.Select((line, n) =>
        {
            var matches = matchString
                .Matches(line)
                .OrderBy(m => m.Index)
                .Select(m => int.TryParse(m.Groups[1].Value, out var intValue)
                    ? intValue
                    : m.Groups[1].Value switch
                    {
                        "one" => 1,
                        "two" => 2,
                        "three" => 3,
                        "four" => 4,
                        "five" => 5,
                        "six" => 6,
                        "seven" => 7,
                        "eight" => 8,
                        "nine" => 9,
                        _ => throw new InvalidOperationException(),
                    })
                .ToList();

            Console.WriteLine($"{n}: [{string.Join(",", matches)}] {int.Parse(matches.First().ToString() + matches.Last())}");

            return int.Parse(matches.First().ToString() + matches.Last());
        }).Sum();

        Console.WriteLine($"Part Two: {result}");
    }
}
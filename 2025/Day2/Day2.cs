using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2025;

class Day2 : Solver
{

    private static readonly Regex DoubleNumberRegex = new(@"^(\d+)\1$");

    private static readonly Regex ManyNumberRegex = new(@"^(\d+)\1+$");

    public override void PartOne()
    {
        var ranges = InputRaw.Split(',').Select(r =>
        {
            r.Split("-").Deconstruct(out var start, out var end, out var _);

            return (Start: long.Parse(start), End: long.Parse(end));
        });

        var numbers = ranges.SelectMany(range => Range(range.Start, range.End));

        var sum = numbers.Select(number =>
        {
            var match = DoubleNumberRegex.Match(number.ToString());

            return match.Success ? number : 0;
        });

        Logger.LogInformation("Result: {Sum}", sum.Sum());
    }

    public override void PartTwo()
    {
        var ranges = InputRaw.Split(',').Select(r =>
        {
            r.Split("-").Deconstruct(out var start, out var end, out var _);

            return (Start: long.Parse(start), End: long.Parse(end));
        });

        var numbers = ranges.SelectMany(range => Range(range.Start, range.End));

        var sum = numbers.Select(number =>
        {
            var match = ManyNumberRegex.Match(number.ToString());

            return match.Success ? number : 0;
        });

        Logger.LogInformation("Result: {Sum}", sum.Sum());
    }

    private static IEnumerable<long> Range(long start, long end)
    {
        var num = start;
        while (num <= end)
        {
            yield return num;
            num += 1;
        }
    }
}
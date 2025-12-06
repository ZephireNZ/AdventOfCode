using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2025;

class Day5 : Solver {
    public override void PartOne()
    {
        var (freshRanges, ingredients) = GetData();

        var count = 0;

        foreach (var ingredient in ingredients)
        {
            foreach (var fresh in freshRanges)
            {
                if (ingredient < fresh.Start) continue;
                if (ingredient > fresh.End) continue;

                count += 1;
                break;
            }
        }

        Logger.LogInformation("Fresh ingredients: {Count}", count);
    }

    public override void PartTwo()
    {
        var (freshRange, _) = GetData();

        var ranges = new HashSet<FreshRange>(freshRange);

        while (FindOverlapping(ranges, out var range, out var overlaps))
        {
            ranges.Remove(range);

            foreach (var overlap in overlaps)
            {
                if (overlap.Covers(range)) continue;
                if (range.Covers(overlap))
                {
                    ranges.Remove(overlap);
                    ranges.Add(range);
                    continue;
                }

                if (range.Start < overlap.Start)
                    ranges.Add(range with { End = overlap.Start - 1 });
                if (range.End > overlap.End)
                    ranges.Add(range with { Start = overlap.End + 1 });
            }
        }

        var totalNumbers = ranges.Sum(r => r.End - r.Start + 1);
        Logger.LogDebug("Total fresh: {Numbers}", totalNumbers);
    }

    private bool FindOverlapping(HashSet<FreshRange> freshRanges, out FreshRange range, out List<FreshRange> overlaps)
    {
        foreach (var l in freshRanges)
        {
            overlaps = freshRanges.Where(r => l != r && l.Overlaps(r)).ToList();
            if (overlaps.Count == 0) continue;
            range = l;
            return true;
        }

        range = default;
        overlaps = null;
        return false;
    }

    private (FreshRange[] fresh, long[] ingredients) GetData()
    {
        var fresh = Input
            .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
            .Select(f =>
            {
                f.Split('-').Deconstruct(out var startStr, out var endStr, out _);

                return new FreshRange(long.Parse(startStr), long.Parse(endStr));
            })
            .OrderByDescending(f => f.End)
            .ToArray();

        var ingredients = Input.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).Select(long.Parse).ToArray();

        return (fresh, ingredients);
    }

    private readonly record struct FreshRange(long Start, long End)
    {
        public bool Overlaps(FreshRange other) =>
            (Start >= other.Start && Start <= other.End)
            || (End >= other.Start && End <= other.End);

        public bool Covers(FreshRange other) => Start <= other.Start && End >= other.End;
    }
}
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2025;

class Day1 : Solver {
    public override void PartOne()
    {
        var dial = 50;
        var zeroes = 0;

        foreach (var cmd in Input)
        {
            var count = int.Parse(cmd[1..]);
            if (cmd[0] == 'L')
                count = -count;

            dial = (dial + count) % 100;

            Logger.LogDebug("Dial is now at {Dial}", dial);

            if (dial == 0)
                zeroes += 1;
        }

        Console.WriteLine(zeroes);
    }

    public override void PartTwo()
    {
        var dial = 50;
        var zeroes = 0;

        foreach (var cmd in Input)
        {
            var count = int.Parse(cmd[1..]);

            foreach (var _ in Enumerable.Range(0, count))
            {
                dial += cmd[0] == 'L' ? -1 : 1;
                dial %= 100;

                if (dial == 0)
                    zeroes += 1;
            }
        }

        Logger.LogInformation("Part Two Result: {Result}", zeroes);
    }
}
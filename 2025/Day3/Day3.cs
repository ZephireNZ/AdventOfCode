using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2025;

class Day3 : Solver {
    public override void PartOne()
    {
        var banks = Input.Select(b =>
            b.Select(c => int.Parse(c.ToString())).ToArray());

        var sum = 0;

        foreach (var bank in banks)
        {
            var maxDigit = bank[..^1].Max();
            var maxIndex = bank.IndexOf(maxDigit);

            var maxJoltage = bank[(maxIndex+1)..].Max(v => int.Parse($"{maxDigit}{v}"));

            sum += maxJoltage;
        }

        Logger.LogInformation("Joltage Sum: {Sum}", sum);
    }

    public override void PartTwo()
    {
        var banks = Input.Select(b =>
            b.Select(c => int.Parse(c.ToString())).ToArray());


        var sum = 0L;

        foreach (var bank in banks)
        {
            var digitsRemaining = 12;

            var joltage = new StringBuilder();
            var remainingBank = bank;

            while (digitsRemaining > 0)
            {
                var largestDigit = remainingBank[..^(digitsRemaining - 1)].Max();
                var index = remainingBank.IndexOf(largestDigit);

                joltage.Append(largestDigit);
                digitsRemaining -= 1;
                remainingBank = remainingBank[(index+1)..];
            }

            sum += long.Parse(joltage.ToString());
        }

        Logger.LogInformation("Joltage Sum: {Sum}", sum);
    }
}
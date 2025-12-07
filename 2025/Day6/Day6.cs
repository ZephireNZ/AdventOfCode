using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace AdventOfCode.Y2025;

class Day6 : Solver
{

    private Regex RowRegex = new(@"([\d*+]+)");

    public override void PartOne()
    {
        var data = Input
            .Select(row =>
                RowRegex.Matches(row)
                    .Select(m => m.Value)
            )
            .Transpose();

        var sum = 0L;

        foreach (var problem in data)
        {
            var numbers = problem.SkipLast(1).Select(long.Parse).ToArray();
            switch (problem.Last())
            {
                case "+":
                    sum += numbers.Sum();
                    break;
                case "*":
                    sum += numbers.Aggregate((a, b) => a * b);
                    break;
            }
        }

        Logger.LogInformation("Total sum: {Sum}", sum);
    }

    public override void PartTwo()
    {
        var data = Input.Select(row => new Stack<char>(row.ToCharArray())).ToArray();
        var numberStacks = data.SkipLast(1).ToArray();
        var operationStack = data.Last();

        var numbers = new List<long>();
        var sum = 0L;

        while (operationStack.TryPop(out var operation))
        {
            var numberStr = numberStacks.Select(s => s.Pop()).ToCharString().Trim();
            numbers.Add(long.Parse(numberStr));

            switch (operation)
            {
                case '+':
                    sum += numbers.Sum();
                    numbers.Clear();
                    break;
                case '*':
                    sum += numbers.Aggregate((a, b) => a * b);
                    numbers.Clear();
                    break;
                case ' ':
                    continue;
            }

            // Skip over the gap
            operationStack.TryPop(out _);
            foreach (var stack in numberStacks)
            {
                stack.TryPop(out _);
            }
        }

        Logger.LogInformation("Total sum: {Sum}", sum);
    }
}
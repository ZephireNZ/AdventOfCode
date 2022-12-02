using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021;

class Day5 : Solver {

    public static IEnumerable<int> Sequence(int start, int stop, int step) {
        long current = start;

        while (step >= 0 ? stop >= current : stop <= current) {
            yield return (int)current;
            current += step;
        }
    }

    public override void PartOne() {
        var grid = new Dictionary<(int x, int y), int>();

        foreach (var line in Input) {
            var (from, (to, _)) = line.Split(" -> ");

            var (from_x, (from_y, _)) = from.Split(",").Select(Int32.Parse).ToArray();
            var (to_x, (to_y, _)) = to.Split(",").Select(Int32.Parse).ToArray();

            if (from_x == to_x) {
                var start = Math.Min(from_y, to_y);
                var count = Math.Abs(to_y - from_y) + 1;
                foreach (var y in Enumerable.Range(start, count)) {
                    grid[(from_x, y)] = grid.TryGetValue((from_x, y), out var value) ? value + 1 : 1;
                }
            } else if (from_y == to_y) {
                var start = Math.Min(from_x, to_x);
                var count = Math.Abs(to_x - from_x) + 1;
                foreach (var x in Enumerable.Range(start, count)) {
                    grid[(x, from_y)] = grid.TryGetValue((x, from_y), out var value) ? value + 1 : 1;
                }
            }
        }

        var result = grid.Where(p => p.Value >= 2).Count();
        Console.WriteLine($"Total overlapping: {result}");
    }

    public override void PartTwo() {
        var grid = new Dictionary<(int x, int y), int>();

        foreach (var line in Input) {
            var (from, (to, _)) = line.Split(" -> ");

            var (from_x, (from_y, _)) = from.Split(",").Select(Int32.Parse).ToArray();
            var (to_x, (to_y, _)) = to.Split(",").Select(Int32.Parse).ToArray();

            if (from_x == to_x) {
                foreach (var y in Sequence(from_y, to_y, to_y >= from_y ? 1 : -1)) {
                    grid[(from_x, y)] = grid.TryGetValue((from_x, y), out var value) ? value + 1 : 1;
                }
            } else if (from_y == to_y) {
                foreach (var x in Sequence(from_x, to_x, to_x >= from_x ? 1 : -1)) {
                    grid[(x, from_y)] = grid.TryGetValue((x, from_y), out var value) ? value + 1 : 1;
                }
            } else if (Math.Abs(to_y-from_y) == Math.Abs(to_x-from_x)) { // Check to ensure line is perfectly diagonal
                var x_enum = Sequence(from_x, to_x, to_x >= from_x ? 1 : -1);
                var y_enum = Sequence(from_y, to_y, to_y >= from_y ? 1 : -1);

                foreach ((var x, var y) in Enumerable.Zip(x_enum, y_enum)) {
                    grid[(x, y)] = grid.TryGetValue((x, y), out var value) ? value + 1 : 1;
                }
            }
        }

        var result = grid.Where(p => p.Value >= 2).Count();
        Console.WriteLine($"Total overlapping: {result}");
    }
}
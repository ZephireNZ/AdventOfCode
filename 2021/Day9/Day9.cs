using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021;

class Day9 : Solver {
    public override void PartOne() {
        var input = Input
            .Select(row => row.Select(c => Int32.Parse(c.ToString())).ToArray())
            .ToArray();

        var x = input.Length;
        var y = input[0].Length;

        var sum = 0;
        
        for (var i = 0; i < x; i++) {
            for (var j = 0; j < y; j++) {
                var value = input[i][j];

                var lowest = true;

                lowest = lowest && (i == 0 || input[i - 1][j] > value); // Left
                lowest = lowest && (i == x - 1 || input[i + 1][j] > value); // Right
                lowest = lowest && (j == 0 || input[i][j - 1] > value); // Down
                lowest = lowest && (j == y - 1 || input[i][j + 1] > value); // Up

                if (lowest) sum += 1 + value;
            }
        }

        Console.WriteLine($"Risk Level sum: {sum}");
    }

    public override void PartTwo() {
        var input = Input
            .Select(row => row.Select(c => Int32.Parse(c.ToString())).ToArray())
            .ToArray();

        var w = input.Length;
        var h = input[0].Length;

        var visited = new HashSet<(int x, int y)>();
        var visitQueue = new Queue<(int x, int y)>();

        var basins = new List<int>();

        var currentBasin = 0;
        
        for (var x = 0; x < w; x++) {
            for (var y = 0; y < h; y++) {
                visitQueue.Enqueue((x,y));

                while (visitQueue.TryDequeue(out var v)) {
                    var value = input[v.x][v.y];
                    if (visited.Contains((v.x, v.y))) {
                        continue;
                    }

                    if (value == 9) {
                        visited.Append((v.x, v.y));
                        continue;
                    }

                    var toVisit = new List<(int x, int y)>();

                    if (v.x != 0) toVisit.Add((v.x - 1, v.y)); // Left
                    if (v.x != w - 1) toVisit.Add((v.x + 1, v.y)); // Right
                    if (v.y != 0) toVisit.Add((v.x, v.y - 1)); // Down
                    if (v.y != h - 1) toVisit.Add((v.x, v.y + 1)); // Up

                    toVisit.Where(l => !visited.Contains(l)).ToList().ForEach(visitQueue.Enqueue);

                    visited.Add(v);
                    currentBasin += 1;
                }

                if(currentBasin > 0) basins.Add(currentBasin);
                currentBasin = 0;
            }
        }

        var sum = basins.OrderDescending().Take(3).Aggregate((x, b) => x * b);

        Console.WriteLine($"3 Largest Sum: {sum}");
    }
}
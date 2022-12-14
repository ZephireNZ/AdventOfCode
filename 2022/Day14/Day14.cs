using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day14 : Solver {

    public void Render(Dictionary<(int x, int y), char> points) {

        var rows = points.Keys.Select(i => i.y);
        var cols = points.Keys.Select(i => i.x);

        for (int y = rows.Min(); y <= rows.Max(); y++) {
            for (int x = cols.Min(); x <= cols.Max(); x++) {
                Console.Write(points.ContainsKey((x, y)) ? points[(x, y)] : ' ');
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public Dictionary<(int x, int y), char> CreateMap() {
        var input = Input
            .Select(l => l
                .Split(" -> ")
                .Select(p => p.Split(","))
                .Select(p => (x: Int32.Parse(p[0]), y: Int32.Parse(p[1])))
            );

        var points = new Dictionary<(int x, int y), char>();
        foreach (var line in input) {
            foreach (var (first, second) in line.Zip(line.Skip(1))) {
                if(first.x == second.x) {
                    Enumerable
                        .Range(Math.Min(first.y, second.y), Math.Abs(first.y - second.y) + 1)
                        .Select(y => (first.x, y))
                        .ToList()
                        .ForEach(i => points[i] = '#');
                } else if (first.y == second.y) {
                    Enumerable
                        .Range(Math.Min(first.x, second.x), Math.Abs(first.x - second.x) + 1)
                        .Select(x => (x, first.y))
                        .ToList()
                        .ForEach(i => points[i] = '#');
                }
            }
        }
        

        return points;
    }

    public override void PartOne() {
        var points = CreateMap();
        // Render(points);

        var maxY = points.Keys.Select(i => i.y).Max();

        var s = (x: 500, y: 0);
        while (true) {
            if (s.y > maxY) break; // Fallen off the map
            if (!points.ContainsKey((s.x, s.y + 1))) {
                s.y += 1; // Move down one
                continue;
            }

            // Something underneath us, try go diagonal
            if(!points.ContainsKey((s.x - 1, s.y + 1))) {
                s.x -= 1; // Move diagonal left
                s.y += 1;
                continue;
            }

            if(!points.ContainsKey((s.x + 1, s.y + 1))) {
                s.x += 1; // Move diagonal right
                s.y += 1;
                continue;
            }

            // Settled, add to point map
            points[s] = 'o';
            // Render(points);

            s = (x: 500, y: 0);
        }

        var sand = points.Values.Where(v => v == 'o').Count();
        Render(points);

        Console.WriteLine($"Sand: {sand}");
    }

    public override void PartTwo() {
        var points = CreateMap();

        var maxY = points.Keys.Select(i => i.y).Max();

        var s = (x: 500, y: 0);
        while (true) {
            if (points.ContainsKey((500, 0))) break; // Filled up
            if (s.y > maxY) { // Reached the floor, don't go any further
                points[s] = 'o';
                s = (x: 500, y: 0);
            }

            if (!points.ContainsKey((s.x, s.y + 1))) {
                s.y += 1; // Move down one
                continue;
            }

            // Something underneath us, try go diagonal
            if(!points.ContainsKey((s.x - 1, s.y + 1))) {
                s.x -= 1; // Move diagonal left
                s.y += 1;
                continue;
            }

            if(!points.ContainsKey((s.x + 1, s.y + 1))) {
                s.x += 1; // Move diagonal right
                s.y += 1;
                continue;
            }

            // Settled, add to point map
            points[s] = 'o';
            // Render(points);

            s = (x: 500, y: 0);
        }

        var sand = points.Values.Where(v => v == 'o').Count();
        // Render(points);

        Console.WriteLine($"Sand: {sand}");
    }
}
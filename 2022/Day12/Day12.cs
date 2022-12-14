using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day12 : Solver {

    public List<(int x, int y)> GetNeighbours(int x, int y, int w, int h) {
        var ret = new List<(int x, int y)>();

        if (x != 0) ret.Add((x - 1, y));
        if (x != w - 1) ret.Add((x + 1, y));
        if (y != 0) ret.Add((x, y - 1));
        if (y != h - 1) ret.Add((x, y + 1));

        return ret;
    }

    public int ShortestPath((int x, int y) start, (int x, int y) end, char[][] input) {
        var (_, _, dist) = ShortestPath(end, input, p => p.x == start.x && p.y == start.y);

        return dist;
    }

    public (int x, int y, int dist) ShortestPath((int x, int y) start, char[][] input, Func<(int x, int y), bool> stopFunction) {
        var h = input.Length;
        var w = input[0].Length;

        var visitQueue = new Queue<(int x, int y, int d)>();
        var distance = new Dictionary<(int x, int y), int>();

        visitQueue.Enqueue((start.x, start.y, 0));
        distance.Add((start.x, start.y), 0);

        while (visitQueue.TryDequeue(out var v)) {
            var value = input[v.y][v.x];
            value = value == 'E' ? 'z' : value;

            if (stopFunction((v.x, v.y))) {
                return v;
            }

            foreach (var n in GetNeighbours(v.x, v.y, w, h)) {
                if (distance.ContainsKey(n)) continue;

                var nValue = input[n.y][n.x];
                nValue = nValue == 'S' ? 'a' : nValue;

                if (value - nValue == 1 || value - nValue <= 0) {
                    visitQueue.Enqueue((n.x, n.y, v.d + 1));
                    distance.Add((n.x, n.y), v.d + 1);
                }
            }

            // Render(distance, w, h, start);
        }

        throw new Exception("No path found!");
    }

    public void Render(Dictionary<(int x, int y), int> distances, int w, int h, (int x, int y) start) {
        for (var y = 0; y < h; y++) {
            for (var x = 0; x < w ; x++) {
                var d = distances.ContainsKey((x, y)) ? distances[(x, y)].ToString("00") : "..";
                if (start == (x, y)) d = "SS";
                Console.Write(d);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public override void PartOne() {
        var input = Input.Select(s => s.ToArray()).ToArray();
        var points = input.Select((v, y) => v.Select((v, x) => (x, y, v))).SelectMany(x => x);

        var start = points.Where(v => v.v == 'S').Select(v => (v.x, v.y)).First();
        var end = points.Where(v => v.v == 'E').Select(v => (v.x, v.y)).First();

        var shortest = ShortestPath(start, end, input);

        Console.WriteLine($"Shortest Path: {shortest}");
    }

    public override void PartTwo() {
        var input = Input.Select(s => s.ToArray()).ToArray();
        var points = input.Select((v, y) => v.Select((v, x) => (x, y, v))).SelectMany(x => x);

        var end = points.Where(v => v.v == 'E').Select(v => (v.x, v.y)).First();

        var (_,_,shortest) = ShortestPath(end, input, p => input[p.y][p.x] == 'a');

        Console.WriteLine($"Shortest of all paths: {shortest}");
    }
}
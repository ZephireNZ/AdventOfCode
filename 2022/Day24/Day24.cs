using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day24 : Solver {

    public enum Direction {
        LEFT = '<',
        RIGHT = '>',
        UP = '^',
        DOWN = 'v'
    }

    public static int Manhattan((int x, int y) p1, (int x, int y) p2) {
        return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
    }

    public class Blizzard {
        private (int x, int y) start;
        private Direction d;
        private int h;
        private int w;

        public Blizzard((int x, int y) start, Direction d, char[][] map) {
            this.start = start;
            this.d = d;
            this.h = map.Length - 2;
            this.w = map[0].Length - 2;
        }

        public (int x, int y) GetRowColRef() {
            switch (d) {
                case Direction.LEFT:
                case Direction.RIGHT:
                    return (0, this.start.y);
                case Direction.UP:
                case Direction.DOWN:
                    return (this.start.x, 0);
                default:
                    throw new ArgumentException();
            }
        }

        public bool IsAtLocation(int t, (int x, int y) loc) {
            switch (d) {
                case Direction.LEFT:
                case Direction.RIGHT:
                    if (start.y != loc.y) return false;

                    var x = start.x - 1; // Makes calculations easier
                    var offsetX = d == Direction.RIGHT ? t : -t;
                    x = (x + offsetX) % w;
                    x = x < 0 ? w + x : x;

                    return x + 1 == loc.x;
                case Direction.UP:
                case Direction.DOWN:
                    if(start.x != loc.x) return false;
                    
                    var y = start.y - 1; // Makes calculations easier
                    var offsetY = d == Direction.DOWN ? t : -t;
                    y = (y + offsetY) % h;
                    y = y < 0 ? h + y : y;

                    return y + 1 == loc.y;
                default:
                    throw new ArgumentException();
            }


        }

    }

    public int FindShortestBFS(
        (int x, int y) start,
        (int x, int y) end,
        Dictionary<int, List<(int x, int y, int t)>> positions,
        int cycleTime
    ) {
        var visited = new HashSet<(int x, int y, int t)>();
        var visitQueue = new PriorityQueue<(int x, int y, int t), int>();

        visitQueue.Enqueue((start.x, start.y, 0), Manhattan(start, end));
        visited.Add((start.x, start.y, 0));

        while (visitQueue.TryDequeue(out var pos, out var _)) {
            var t = (pos.t + 1);

            var newPos = new List<(int x, int y, int t)> {
                (pos.x + 1, pos.y, t),
                (pos.x - 1, pos.y, t),
                (pos.x, pos.y + 1, t),
                (pos.x, pos.y - 1, t),
                (pos.x, pos.y, t),
            };

            foreach (var p in newPos) {
                if(p.x == end.x && p.y == end.y) return p.t; // We've found the target

                var pNorm = (p.x, p.y, p.t % cycleTime);
                
                if(visited.Contains(pNorm)) continue;

                if(positions[t % cycleTime].Contains(pNorm)) {
                    visitQueue.Enqueue(p, Manhattan((p.x, p.y), end));
                    visited.Add(pNorm);
                }
            }
        }

        return -1; // Unable to find a path
    }

    public override void PartOne() {
        var input = Input;

        var map = input
            .Select(l => l.ToArray())
            .ToArray();

        var blizzards = input
            .Select((l, y) => l.Select((d, x) => (x, y, d: (Direction) d)))
            .SelectMany(b => b)
            .Where(b => Enum.GetValues<Direction>().Contains(b.d))
            .Select(b => new Blizzard((b.x, b.y), b.d, map))
            .GroupBy(b => b.GetRowColRef())
            .ToDictionary(g => g.Key, g => g.ToList());

        var start = (x: input.First().IndexOf('.'), y: 0);
        var end = (x: input.Last().IndexOf('.'), y: input.Length - 1);

        var cycleTime = MathHelpers.LeastCommonMultiple<int>(map.Length - 2, map[0].Length - 2);

        var positions = new Dictionary<int, List<(int x, int y, int t)>>();

        for(var t = 0; t < cycleTime; t++) {
            positions[t] = new List<(int x, int y, int t)>();
            positions[t].Add((1, 0, t));
            
            for (var x = 1; x < map[0].Length - 1; x++) {
                for (var y = 1; y < map.Length - 1; y++) {
                    List<Blizzard> bliz;
                    if (blizzards.TryGetValue((x, 0), out bliz) ? bliz.Any(b => b.IsAtLocation(t, (x, y))) : false) continue;
                    if (blizzards.TryGetValue((0, y), out bliz) ? bliz.Any(b => b.IsAtLocation(t, (x, y))) : false) continue;


                    positions[t].Add((x, y, t));
                } 
            }
        }


        var shortest = FindShortestBFS(start, end, positions, cycleTime);

        Console.WriteLine($"Shortest Path: {shortest}");


    }
}
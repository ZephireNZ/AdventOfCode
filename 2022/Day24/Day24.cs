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

    public List<(int x, int y, Direction d)> MoveBlizzards(
        List<(int x, int y, Direction d)> blizzards, 
        char[][] map)
    {
        return blizzards.Select(b => {
            (int x, int y) newPos = b.d switch {
                Direction.LEFT => (b.x - 1, b.y),
                Direction.RIGHT => (b.x + 1, b.y),
                Direction.UP => (b.x, b.y - 1),
                Direction.DOWN => (b.x, b.y + 1),
                _ => throw new ArgumentException()
            };

            if (map[newPos.y][newPos.x] == '#') {
                newPos = b.d switch {
                    Direction.LEFT => (map[b.y].Length - 2, b.y),
                    Direction.RIGHT => (1, b.y),
                    Direction.UP => (b.x, map.Length - 2),
                    Direction.DOWN => (b.x, 1),
                    _ => throw new ArgumentException()
                };
            }

            return (newPos.x, newPos.y, b.d);
        })
        .ToList();

        
    }

    public int FindShortestPath(
        (int x, int y) pos, 
        List<(int x, int y, Direction d)> blizzards,
        char[][] map,
        (int x, int y) target,
        int shortest)
    {
        if (Manhattan(pos, target) > shortest) return shortest; // Impossible

        var newPos = new List<(int x, int y)> {
            (pos.x + 1, pos.y),
            (pos.x - 1, pos.y),
            (pos.x, pos.y + 1),
            (pos.x, pos.y - 1),
            pos
        };

        blizzards = MoveBlizzards(blizzards, map);

        foreach (var p in newPos) {
            if(blizzards.Any(b => b.x == p.x && b.y == p.y)) continue; // not possible
            
            if(p == target) return 1; // We've found the target

            if(p.y < 0) continue; // Edge case for start of map

            if (map[p.y][p.x] == '#') continue; // can't move into a wall

            var res = FindShortestPath(p, blizzards, map, target, shortest - 1) + 1;

            if (res < shortest) shortest = res;
        }

        return shortest;
        
    }

    public override void PartOne() {
        var input = Input;

        var map = input
            .Select(l => l.Select(c => c switch {
                '#' => '#',
                '.' => '.',
                _ => '.'
            }).ToArray())
            .ToArray();

        var blizzards = input
            .Select((l, y) => l.Select((d, x) => (x, y, d: (Direction) d)))
            .SelectMany(b => b)
            .Where(b => Enum.GetValues<Direction>().Contains(b.d))
            .ToList();

        var start = (x: input.First().IndexOf('.'), y: 0);
        var end = (x: input.Last().IndexOf('.'), y: input.Length - 1);

        var shortest = FindShortestPath(start, blizzards, map, end, 200);

        Console.WriteLine($"Shortest Path: {shortest}");


    }
}
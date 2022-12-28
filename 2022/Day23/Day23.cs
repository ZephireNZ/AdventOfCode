using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day23 : Solver {

    public void Render(ICollection<(int x, int y)> elves) {
        var maxX = elves.Max(e => e.x);
        var maxY = elves.Max(e => e.y);

        for (var y = elves.Min(e => e.y); y <= maxY; y++) {
            for (var x = elves.Min(e => e.x); x <= maxX; x++) {
                Console.Write(elves.Contains((x, y)) ? "#" : ".");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public (int count, Dictionary<char, HashSet<(int x, int y)>> adj) Adjacent((int x, int y) elf, ICollection<(int x, int y)> elves) {

        var adj = Enumerable.Range(elf.x - 1, 3)
            .Select(x => Enumerable.Range(elf.y - 1, 3).Select(y => (x, y)))
            .SelectMany(n => n)
            .Where(n => n != elf && elves.Contains(n));

        return (adj.Count(), new Dictionary<char, HashSet<(int x, int y)>>() {
            {'N', adj.Where(n => n.y == elf.y - 1).ToHashSet()},
            {'S', adj.Where(n => n.y == elf.y + 1).ToHashSet()},
            {'E', adj.Where(n => n.x == elf.x + 1).ToHashSet()},
            {'W', adj.Where(n => n.x == elf.x - 1).ToHashSet()},
        });
    }

    public HashSet<(int x, int y)> Move(HashSet<(int x, int y)> elves, List<char> directions) {
        return elves.Select(elf => {
                var (count, adj) = Adjacent(elf, elves);
                if (count == 0) return (from: elf, to: elf);

                return (from: elf, to: directions
                    .Where(dir => adj[dir].Count == 0)
                    .Select(dir => dir switch {
                        'N' => (x: elf.x, y: elf.y - 1),
                        'S' => (x: elf.x, y: elf.y + 1),
                        'E' => (x: elf.x + 1, y: elf.y),
                        'W' => (x: elf.x - 1, y: elf.y),
                        _ => throw new ArgumentException()
                    })
                    .FirstOrDefault(elf)
                );
            })
            .GroupBy(e => e.to)
            .SelectMany(g => {
                if (g.Count() == 1) return g.Select(e => e.to);

                return g.Select(e => e.from);
            })
            .ToHashSet();
    }

    public override void PartOne() {
        var elves = Input
            .Select((l, y) => l
                .Select((v, x) => (v, x))
                .Where(v => v.v == '#')
                .Select(v => (x: v.x, y))
            )
            .SelectMany(x => x)
            .ToHashSet();

        var directions = new List<char>() {
            'N',
            'S',
            'W',
            'E'
        };

        foreach (var round in Enumerable.Range(1, 10)) {
            elves = Move(elves, directions);
            
            directions = directions.Skip(1).Append(directions.First()).ToList();
            //Render(elves);
        }

        var width = elves.Max(e => e.x) - elves.Min(e => e.x) + 1;
        var height = elves.Max(e => e.y) - elves.Min(e => e.y) + 1;

        var empty = (width * height) - elves.Count;

        Console.WriteLine($"Empty spaces: {empty}");
    }

    public override void PartTwo() {
        var elves = Input
            .Select((l, y) => l
                .Select((v, x) => (v, x))
                .Where(v => v.v == '#')
                .Select(v => (x: v.x, y))
            )
            .SelectMany(x => x)
            .ToHashSet();

        var directions = new List<char>() {
            'N',
            'S',
            'W',
            'E'
        };

        var round = 1;

        while(true) {
            var newElves = Move(elves, directions);
            if(newElves.SetEquals(elves)) {
                Console.WriteLine($"Halted at round {round}");
                return;
            }

            if(round % 100 == 0) {
                Console.WriteLine($"Round {round}");
            }

            elves = newElves;
            directions = directions.Skip(1).Append(directions.First()).ToList(); 
            round += 1;           
        }
    }
}
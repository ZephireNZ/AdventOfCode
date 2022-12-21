using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day19 : Solver {

    public enum Material {
        GEODE,
        OBSIDIAN,
        CLAY,
        ORE
    }

    public record Schema(Material mat, Dictionary<Material, int> cost);

    public Dictionary<Material, int> ProcessRobots(Dictionary<Material, int> materials, Dictionary<Material, int> robots) {
        // Returns a copy of the materials, preventing recursions from changing top level
        return materials
            .ToDictionary(
                m => m.Key,
                m => m.Value + robots[m.Key]
            );
    }

    public int MaxGeodeHeuristic(Dictionary<Material, int> robots, Dictionary<Material, int> materials, int remainingMinutes) {
        var start = robots[Material.GEODE];
        var end = remainingMinutes + robots[Material.GEODE] - 1;

        // Simulates buying a new geode robot for every following turn
        return materials[Material.GEODE] + (end * (end + 1) / 2) - (start * (start - 1) / 2);
    }

    public int FindMaxGeodes(
        List<Schema> schemas,
        int remainingMinutes,
        Dictionary<Material, int> materials,
        Dictionary<Material, int> robots,
        int best)
    {
        if(remainingMinutes == 0) return materials[Material.GEODE];

        // Short circuit if less than current best
        if(best >= MaxGeodeHeuristic(robots, materials, remainingMinutes)) return 0;

        if(remainingMinutes == 1) return materials[Material.GEODE] + robots[Material.GEODE];

        var newMaterials = ProcessRobots(materials, robots);

        
        var maxGeodes = 0;
        foreach (var (mat, cost) in schemas) {
            if (cost.All(c => materials[c.Key] >= c.Value)) {
                // We have enough for a new robot
                var newRobots = new Dictionary<Material, int>(robots);
                var minusMaterials = new Dictionary<Material, int>(newMaterials);
                newRobots[mat] += 1;
                foreach (var rem in cost) {
                    minusMaterials[rem.Key] -= rem.Value;
                }

                maxGeodes = Math.Max(
                    maxGeodes, 
                    FindMaxGeodes(schemas, remainingMinutes - 1, minusMaterials, newRobots, best)
                );
                best = Math.Max(best, maxGeodes);
            }
        }

        maxGeodes = Math.Max(
            maxGeodes, 
            FindMaxGeodes(schemas, remainingMinutes - 1, newMaterials, robots, best)
        );

        return maxGeodes;
    }
    
    public override void PartOne() {
        var blueprints = Input
            .Select(row => Regex.Matches(row, @"Each (?<MAT>\w+) robot costs ((?<COST>\d+ \w+)( and |))+"))
            .Select(matches => matches.Select(row => (
                        mat: Enum.Parse<Material>(row.Groups["MAT"].Value.ToUpper()),
                        cost: row.Groups["COST"]
                            .Captures
                            .Select(c => c.Value.Split(" "))
                            .ToDictionary(c=> Enum.Parse<Material>(c[1].ToUpper()), c => Int32.Parse(c[0]))
                    )
                )
                .Select(r => new Schema(r.mat, r.cost))
            )
            .Select((b, i) => (i: i + 1, schemas: b.ToList()))
            .ToList();
        
        var result = 0;

        foreach (var (i, schemas) in blueprints) {
            var robots = Enum.GetValues<Material>().ToDictionary(e => e, _ => 0);
            robots[Material.ORE] += 1;

            var materials = Enum.GetValues<Material>().ToDictionary(e => e, _ => 0);

            var remainingMinutes = 24;

            var maxGeodes = FindMaxGeodes(schemas, remainingMinutes, materials, robots, 0);
            Console.WriteLine($"Max Geodes {i}: {maxGeodes}");

            result += i * maxGeodes;
        }

        Console.WriteLine($"Result: {result}");
    }
}
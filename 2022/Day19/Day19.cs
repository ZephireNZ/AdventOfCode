using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Y2022;

class Day19 : Solver {

    public enum Material {
        GEODE,
        OBSIDIAN,
        CLAY,
        ORE
    }

    public record Schema(Material mat, Dictionary<Material, int> cost);

    public int FindMaxGeodes(
        Dictionary<Material, Schema> schemas,
        int remainingMinutes,
        Dictionary<Material, int> materials,
        Dictionary<Material, int> robots)
    {
        if(remainingMinutes == 0) return materials[Material.GEODE];

        if(remainingMinutes == 1) return materials[Material.GEODE] + robots[Material.GEODE];

        if (schemas[Material.GEODE].cost.All(c => materials[c.Key] >= c.Value)) {
            // Always craft a geode robot
            var newMaterials = materials
                    .ToDictionary(
                        m => m.Key,
                        m => m.Value - schemas[Material.GEODE].cost.GetValueOrDefault(m.Key)
                    );
                
            var newRobots = new Dictionary<Material, int>(robots);
            newRobots[Material.GEODE] += 1;

            return FindMaxGeodes(schemas, remainingMinutes - 1, newMaterials, newRobots);
        }

        var maxGeodes = schemas
            .Values
            .Select(schema => {
                var (mat, cost) = schema;

                if (cost.Keys.Any(c => robots[c] == 0)) return 0; // Not generating

                var timeDiff = cost
                .Select(c =>(c.Value - materials[c.Key]) / (double)robots[c.Key])
                .Select(c =>  (int) Math.Ceiling(c))
                .Append(1)
                .Max();

                if(remainingMinutes < timeDiff) return 0; // Handled by Append

                var newMaterials = materials
                    .ToDictionary(
                        m => m.Key,
                        m => m.Value - cost.GetValueOrDefault(m.Key) + (robots[m.Key] * timeDiff)
                    );

                var newRobots = new Dictionary<Material, int>(robots);
                newRobots[mat] += 1;

                return FindMaxGeodes(schemas, remainingMinutes - timeDiff, newMaterials, newRobots);         
            })
            .Append(materials[Material.GEODE] + (robots[Material.GEODE] * remainingMinutes))
            //.AsParallel()
            //.WithDegreeOfParallelism(4)
            .Max();
        
        if (maxGeodes == 20) {
            Console.WriteLine("20");
        }

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

            var maxGeodes = FindMaxGeodes(schemas.ToDictionary(s => s.mat, s => s), remainingMinutes, materials, robots);
            Console.WriteLine($"Max Geodes {i}: {maxGeodes}");

            result += i * maxGeodes;
        }

        Console.WriteLine($"Result: {result}");
    }
}
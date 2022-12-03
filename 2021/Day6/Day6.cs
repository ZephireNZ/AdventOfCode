using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021;

class Day6 : Solver {
    public override void PartOne() {
        var fish = Input[0].Split(',').Select(Int32.Parse).ToList();

        var iteration = 0;

        while (iteration < 80) {
            fish = fish.Select(f => f - 1).ToList();

            fish.AddRange(Enumerable.Repeat(8, fish.Where(f => f < 0).Count())); // Gave birth

            fish = fish.Select(f => f < 0 ? 6 : f).ToList();

            iteration += 1;
        }

        Console.WriteLine($"Total fish: {fish.Count}");
    }

    public override void PartTwo() {
        var fish = Enumerable.Range(-1, 10).ToDictionary(x => x, x => 0L); // Give each age starting count of 0

        foreach (var f in Input[0].Split(',').Select(Int32.Parse)) {
            fish[f] += 1;
        }

        var iteration = 0;

        while (iteration < 256) {
            Console.WriteLine($"Processing iteration {iteration}");

            foreach (var f in Enumerable.Range(0, 9)) {
                fish[f - 1] = fish[f];
                fish[f] = 0;
            }

            fish[8] += fish[-1]; // Spawn new fish
            fish[6] += fish[-1]; // Reset birthed fish
            fish[-1] = 0; // Remove birthed fish

            iteration += 1;
        }

        Console.WriteLine($"Total fish: {fish.Values.Sum()}");
    }
}
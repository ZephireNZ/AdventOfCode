using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day2 : Solver {
    public override void PartOne() {
        var score = 0;

        foreach (var round in Input)
        {
            var (elf, (you, _)) = round.Split(' ');

            score += (elf, you) switch {
                (_, "X") => 1 + elf switch { "A" => 3, "B" => 0, "C" => 6, _ => 0 }, // You play Rock
                (_, "Y") => 2 + elf switch { "A" => 6, "B" => 3, "C" => 0, _ => 0 }, // You play Paper
                (_, "Z") => 3 + elf switch { "A" => 0, "B" => 6, "C" => 3, _ => 0 }, // You play Scissors
                _ => 0,
            };
        }

        Console.WriteLine($"Final score: {score}");
    }

    public override void PartTwo() {
        var score = 0;

        foreach (var round in Input)
        {
            var (elf, (strat, _)) = round.Split(' ');

            score += (elf, strat) switch {
                (_, "X") => 0 + elf switch { "A" => 3, "B" => 1, "C" => 2, _ => 0 }, // You lose
                (_, "Y") => 3 + elf switch { "A" => 1, "B" => 2, "C" => 3, _ => 0 }, // You draw
                (_, "Z") => 6 + elf switch { "A" => 2, "B" => 3, "C" => 1, _ => 0 }, // You win
                _ => 0,
            };
        }
    
        Console.WriteLine($"Final part two score: {score}");
    }
}
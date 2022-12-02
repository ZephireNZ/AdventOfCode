using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021;

class Day3 : Solver {
    public override void PartOne() {
        var input = Input;

        var bitCount = new int[input[0].Length];

        Array.Fill(bitCount, 0);

        foreach (var row in input) {
            for (var i = 0; i < bitCount.Length; i++) {
                bitCount[i] += row[i] == '1' ? 1 : 0;
            }
        }

        var gammaStr = String.Concat(bitCount.Select(c => c >= input.Length / 2 ? '1' : '0'));
        var epsilonStr = String.Concat(gammaStr.Select(c => c == '1' ? '0' : '1'));

        Console.WriteLine($"Gamma {gammaStr}, Epsilon {epsilonStr}");

        var gamma = Convert.ToInt32(gammaStr, 2);
        var epsilon = Convert.ToInt32(epsilonStr, 2);

        Console.WriteLine($"Gamma {gamma}, Epsilon {epsilon}, Combined {gamma*epsilon}");
    }

    public override void PartTwo() {
        var input = Input;

        var bits = input[0].Length;

        var oxygenOptions = input.ToArray();

        for (var bit = 0; bit < bits; bit++) {
            var mostBit = oxygenOptions.Where(x => x[bit] == '1').Count() >= oxygenOptions.Length / 2.0 ? '1' : '0';

            oxygenOptions = oxygenOptions.Where(x => x[bit] == mostBit).ToArray();
            if (oxygenOptions.Count() == 1) {
                break;
            }
        }

        var scrubberOptions = input.ToArray();

        for (var bit = 0; bit < bits; bit++) {
            var leastBit = scrubberOptions.Where(x => x[bit] == '1').Count() >= scrubberOptions.Length / 2.0 ? '0' : '1';

            scrubberOptions = scrubberOptions.Where(x => x[bit] == leastBit).ToArray();
            if (scrubberOptions.Count() == 1) {
                break;
            }
        }

        Console.WriteLine($"Oxygen {oxygenOptions.First()}, Scrubber {scrubberOptions.First()}");

        var oxygen = Convert.ToInt32(oxygenOptions.First(), 2);
        var scrubber = Convert.ToInt32(scrubberOptions.First(), 2);

        Console.WriteLine($"Oxygen {oxygen}, Scrubber {scrubber}, Combined {oxygen*scrubber}");
    }
}
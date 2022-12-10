using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day10 : Solver {

    public override void PartOne() {
        var x = 1;
        var cycle = 0;
        var nextCheck = 20;
        var sum = 0;

        foreach (var instr in Input) {
            int startingX = x;
            if (instr == "noop") {
                cycle += 1;
            } else {
                x += Int32.Parse(instr.Split(' ')[1]);
                cycle += 2;
            }

            if (cycle >= nextCheck) {
                sum += nextCheck * startingX;
                nextCheck += 40;
            }
        }

        Console.WriteLine($"Signal Strength Sum {sum}");
    }

    public override void PartTwo() {
        var input = new Queue<string>(Input);
        var sprite = 1;
        var cycle = 1;

        var screen = new char[6, 40];

        int? pending = null;

        while (true) {
            var x = (cycle - 1) % 40;
            var y = (cycle - 1) / 40;
            screen[y, x] = (x >= sprite - 1 && x <= sprite + 1) ? '#' : ' ';

            if (pending != null) {
                sprite += (int)pending;
                pending = null;
            } else {
                var instr = input.Dequeue();

                if (instr == "noop") {
                    // Nothing
                } else {
                    pending = Int32.Parse(instr.Split(' ')[1]);
                }
            }

            cycle += 1;

            for (var i = 0; i < screen.GetLength(0); i++) {
                for (var j = 0; j < screen.GetLength(1); j++) {
                    Console.Write(screen[i,j]);
                }
                Console.Write(Environment.NewLine);
            }
            Console.Write(Environment.NewLine);

            if (input.Count == 0) break;
        }
    }
}
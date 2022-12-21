using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day20 : Solver {
    public override void PartOne() {
        var input = Input.Select(c => Int32.Parse(c)).ToList();
        var count = input.Count;

        var inputIdx = input.Select((_, i) => i).ToList();

        for (var i = 0; i < count; i++) {
            var pos = inputIdx.IndexOf(i);
            var val = input[pos];

            var newPos = (pos + val) % (count - 1);
            newPos = newPos < 0 ? newPos + count - 1 : newPos;
            newPos = newPos == 0 ? count - 1 : newPos;

            if (newPos > pos) {
                input.RemoveAt(pos);
                input.Insert(newPos, val);
                inputIdx.RemoveAt(pos);
                inputIdx.Insert(newPos, i);
            } else {
                input.Insert(newPos, val);
                input.RemoveAt(pos);
                inputIdx.Insert(newPos, i);
                inputIdx.RemoveAt(pos);
            }
        }

        var zero = input.IndexOf(0);

        var sum = input[(zero + 1_000) % count]
            + input[(zero + 2_000) % count]
            + input[(zero + 3_000) % count];

        Console.WriteLine($"Result: {sum}");
    }

    public override void PartTwo() {
        var input = Input.Select(c => Int32.Parse(c) * 811589153L).ToList();
        var count = input.Count;

        var inputIdx = input.Select((_, i) => i).ToList();

        foreach (var _ in Enumerable.Range(0, 10)) {
            for (var i = 0; i < count; i++) {
                var pos = inputIdx.IndexOf(i);
                var val = input[pos];

                var newPos = (int) ((pos + val) % (count - 1));
                newPos = newPos < 0 ? newPos + count - 1 : newPos;
                newPos = newPos == 0 ? count - 1 : newPos;

                if (newPos > pos) {
                    input.RemoveAt(pos);
                    input.Insert(newPos, val);
                    inputIdx.RemoveAt(pos);
                    inputIdx.Insert(newPos, i);
                } else {
                    input.Insert(newPos, val);
                    input.RemoveAt(pos + 1);
                    inputIdx.Insert(newPos, i);
                    inputIdx.RemoveAt(pos + 1);
                }
            }
        }

        var zero = input.IndexOf(0);

        var sum = input[(zero + 1_000) % count]
            + input[(zero + 2_000) % count]
            + input[(zero + 3_000) % count];

        Console.WriteLine($"Result: {sum}");
    }
}
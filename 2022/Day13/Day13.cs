using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AdventOfCode.Y2022;

class Day13 : Solver {

    public object DeepConvert(JsonElement input) {
        return input.ValueKind switch {
            JsonValueKind.Number => input.GetInt64(),
            JsonValueKind.Array => input.EnumerateArray().Select(i => DeepConvert(i)).ToArray(),
            _ => input
        };
    }

    public class Comparer : Comparer<object>
    {
        public override int Compare(object left, object right) {
            if (left is long && right is long) {
                if ((long) left < (long) right) return -1;
                if ((long) left > (long) right) return 1;
                return 0;
            }

            if (left is object[] && right is long) right = new object[] {right};
            if (right is object[] && left is long) left = new object[] {left};

            var i = 0;
            var leftA = (object[]) left;
            var rightA = (object[]) right;
            while (true) {
                if (i >= leftA.Length && i >= rightA.Length) return 0;
                if (i >= leftA.Length) return -1;
                if (i >= rightA.Length) return 1;

                var result = Compare(leftA[i], rightA[i]);
                if (result != 0) return result;

                i += 1;
            }
        }
    }

    public override void PartOne() {
        var compare = new Comparer();
        var result = InputRaw
            .Split(Environment.NewLine + Environment.NewLine)
            .Select((p, i) => p.Split(Environment.NewLine))
            .Select((pair, i) => {
                var one = DeepConvert(JsonSerializer.Deserialize<JsonElement>(pair[0]));
                var two = DeepConvert(JsonSerializer.Deserialize<JsonElement>(pair[1]));

                return (res: compare.Compare(one, two), i);
            })
            .Where(res => res.res == -1)
            .Select(res => res.i + 1)
            .Sum();
        
        Console.WriteLine($"Sum of right pairs: {result}");
    }

    public override void PartTwo() {
        var result = InputRaw
            .Split(Environment.NewLine + Environment.NewLine)
            .Select((pair, i) => pair.Split(Environment.NewLine))
            .Append(new string[] {"[[2]]", "[[6]]"})
            .SelectMany(p => p)
            .Select(packet => DeepConvert(JsonSerializer.Deserialize<JsonElement>(packet)))
            .ToList();

        result.Sort(new Comparer());

        var decoder = result
            .Select((packet, x) => (packet: JsonSerializer.Serialize(packet), x: x + 1))
            .Where(p => p.packet == "[[2]]" || p.packet == "[[6]]")
            .Select(p => p.x)
            .Aggregate((a, x) => a * x);
        
        Console.WriteLine($"Decoder: {decoder}");
    }
}
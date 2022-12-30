using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day25 : Solver {

    public long ParseSNAFU(string snafu) {
        return snafu
            .Reverse()
            .Select((c, i) => {
                var mult = (long) Math.Pow(5, i);

                return mult * c switch {
                    '2' => 2,
                    '1' => 1,
                    '0' => 0,
                    '-' => -1,
                    '=' => -2,
                    _ => throw new ArgumentException()
                };
            })
            .Sum();
    }

    public string ToSNAFU(long value) {
        var snafuStr = ""; // String is inverted, so i == 0 is 5^0
        var snafuVal = 0L;

        long mult = 1;
        while (value > snafuVal) {
            snafuVal += mult * 2;
            mult = mult * 5;
            snafuStr += "2";
        }

        mult = mult / 5; // We overshot by one
        var snafu = snafuStr.ToCharArray();
        var idx = snafu.Length - 1;

        while (value != snafuVal) {
            if (idx == -1) throw new InvalidOperationException();
            var remainder = snafuVal - value;

            if (remainder < mult) {
                mult /= 5;
                idx -= 1;
                continue;
            }

            snafu[idx] = snafu[idx] switch {
                '2' => '1',
                '1' => '0',
                '0' => '-',
                '-' => '=',
                _ => throw new InvalidOperationException()
            };
            snafuVal -= mult;
        }

        Array.Reverse(snafu);

        return new String(snafu);
    }

    public override void PartOne() {
        var sum = Input
            .Select(s => ParseSNAFU(s))
            .Sum();
        
        var sumSnafu = ToSNAFU(sum);
        
        Console.WriteLine($"Sum: {sum}, {sumSnafu}");
    }
}
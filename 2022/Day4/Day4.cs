using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day4 : Solver {

    public override void PartOne() {
        var result = Input
            .Select(pair => {
                var res = Regex.Match(pair, @"(\d+)-(\d+),(\d+)-(\d+)").Groups;
                return (
                    min1: Int32.Parse(res[1].Value),
                    max1: Int32.Parse(res[2].Value),
                    min2: Int32.Parse(res[3].Value),
                    max2: Int32.Parse(res[4].Value)
                );
            })
            .Where(p => 
                (p.min1 >= p.min2 && p.max1 <= p.max2) || (p.min2 >= p.min1 && p.max2 <= p.max1)
            ).Count();

        Console.WriteLine($"Total ranges: {result}");
    }

    public override void PartTwo() {
        var result = Input
            .Select(pair => {
                var res = Regex.Match(pair, @"(\d+)-(\d+),(\d+)-(\d+)").Groups;
                return (
                    min1: Int32.Parse(res[1].Value),
                    max1: Int32.Parse(res[2].Value),
                    min2: Int32.Parse(res[3].Value),
                    max2: Int32.Parse(res[4].Value)
                );
            })
            .Where(p => 
                (p.min1 >= p.min2 && p.max1 <= p.max2) || // 1 contained in 2
                (p.min2 >= p.min1 && p.max2 <= p.max1) || // 2 contained in 1
                (p.min1 <= p.min2 && p.max1 >= p.min2) || // 1 overlap 2 on the left
                (p.min2 <= p.min1 && p.max2 >= p.min1)    // 1 overlap 2 on the right
            ).Count();

        Console.WriteLine($"Total ranges: {result}");
    }
}
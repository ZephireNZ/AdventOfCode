using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day5 : Solver {

    public override void PartOne() {
        var (cratesStr, (actionsStr, _)) = InputRaw.Split(Environment.NewLine + Environment.NewLine);

        var rows = cratesStr
            .Split(Environment.NewLine)
            .Select(row => row
                    .Chunk(4)
                    .Select(x => Regex.Replace(new String(x), @"(\[|\]| )", ""))
                    .Select(x => String.IsNullOrEmpty(x) ? '_' : x.First())
                    .ToArray()
            )
            .Reverse()
            .Skip(1) // Ignore numbers
            .ToArray(); 

        // Transpose for easier use
        var stacks = Enumerable.Range(0, rows[0].Length).Select(x => new Stack<char>()).ToArray();
        
        for (var i = 0; i < rows.Length; i++) {
            for (var j = 0; j < rows[0].Length; j++) {
                var value = rows[i][j];
                if(value != '_') {
                    stacks[j].Push(value);
                }
            }
        }
        
        var actions = actionsStr
            .Split(Environment.NewLine)
            .Select(a => Regex.Match(a, @"move (\d+) from (\d+) to (\d+)").Groups)
            .Select(a => (
                count: Int32.Parse(a[1].Value),
                from: Int32.Parse(a[2].Value) - 1,
                to: Int32.Parse(a[3].Value) -1 // Ordinal to index
            )
        );

        foreach (var (count, from, to) in actions) {
            foreach (var i in Enumerable.Range(0, count)) {
                // Take from one stack, give to the other
                stacks[to].Push(stacks[from].Pop());
            }
           
        }

        var top = String.Concat(stacks.Select(s => s.Peek()));

        Console.WriteLine($"Top crates: {top}");
    }

    public override void PartTwo() {
        var (cratesStr, (actionsStr, _)) = InputRaw.Split(Environment.NewLine + Environment.NewLine);

        var rows = cratesStr
            .Split(Environment.NewLine)
            .Select(row => row
                    .Chunk(4)
                    .Select(x => Regex.Replace(new String(x), @"(\[|\]| )", ""))
                    .Select(x => String.IsNullOrEmpty(x) ? '_' : x.First())
                    .ToArray()
            )
            .Reverse()
            .Skip(1) // Ignore numbers
            .ToArray(); 

        // Transpose for easier use
        var stacks = Enumerable.Range(0, rows[0].Length).Select(x => new Stack<char>()).ToArray();
        
        for (var i = 0; i < rows.Length; i++) {
            for (var j = 0; j < rows[0].Length; j++) {
                var value = rows[i][j];
                if(value != '_') {
                    stacks[j].Push(value);
                }
            }
        }
        
        var actions = actionsStr
            .Split(Environment.NewLine)
            .Select(a => Regex.Match(a, @"move (\d+) from (\d+) to (\d+)").Groups)
            .Select(a => (
                count: Int32.Parse(a[1].Value),
                from: Int32.Parse(a[2].Value) - 1,
                to: Int32.Parse(a[3].Value) -1 // Ordinal to index
            )
        );

        foreach (var (count, from, to) in actions) {
            var taken = Enumerable
                .Range(0, count)
                .Select(i => stacks[from].Pop())
                .Reverse() // Imitate taking all at once
                .ToList(); 
            foreach (var i in taken) {
                stacks[to].Push(i);
            }
           
        }

        var top = String.Concat(stacks.Select(s => s.Peek()));

        Console.WriteLine($"Top crates 9001: {top}");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day8 : Solver {

    public override void PartOne() {
        var input = Input;
        var xSize = input[0].Length;
        var ySize = input.Length;

        var trees = new int[xSize][];
        for (var i = 0; i < xSize; i++) {
            trees[i] = new int[ySize];
            for (var j = 0; j < ySize; j++) {
                trees[i][j] = Int32.Parse(input[i][j].ToString());
            }
        }

        // Transposed Array for easy iteration
        var treesT = new int[ySize][];
        for (var j = 0; j < ySize; j++) {
            treesT[j] = new int[xSize];
            for (var i = 0; i < xSize; i++) {
                treesT[j][i] = trees[i][j];
            }
        }

        var visibleTrees = 0;

        for (var x = 0; x < xSize; x++) {
            for (var y = 0; y < ySize ; y++) {
                var tree = trees[x][y];

                var visible = false;

                // Edges
                visible = visible || x == 0 || y == 0 || x == xSize - 1 || y == ySize - 1;
                // LTR
                visible = visible || trees[x].Take(y).All(t => t < tree);
                // RTL
                visible = visible || trees[x].Skip(y + 1).All(t => t < tree);
                // TTB
                visible = visible || treesT[y].Take(x).All(t => t < tree);
                // BTT
                visible = visible || treesT[y].Skip(x + 1).All(t => t < tree);

                visibleTrees += visible ? 1 : 0;
            }
        }

        Console.WriteLine($"Visible trees: {visibleTrees}");

    }

    public override void PartTwo() {
        var input = Input;
        var xSize = input[0].Length;
        var ySize = input.Length;

        var trees = new int[xSize][];
        for (var i = 0; i < xSize; i++) {
            trees[i] = new int[ySize];
            for (var j = 0; j < ySize; j++) {
                trees[i][j] = Int32.Parse(input[i][j].ToString());
            }
        }

        // Transposed Array for easy iteration
        var treesT = new int[ySize][];
        for (var j = 0; j < ySize; j++) {
            treesT[j] = new int[xSize];
            for (var i = 0; i < xSize; i++) {
                treesT[j][i] = trees[i][j];
            }
        }

        var maxScore = 0;

        for (var x = 0; x < xSize; x++) {
            for (var y = 0; y < ySize ; y++) {
                var tree = trees[x][y];

                var scores = new int[4];

                // Left
                scores[0] = trees[x].Take(y).Reverse().Aggregate(
                    (seen: 0, blocked: false ),
                    (a, t) => a.blocked ? a : (a.seen + 1, t >= tree)
                ).seen;
                // Right
                scores[1] = trees[x].Skip(y + 1).Aggregate(
                    (seen: 0, blocked: false ),
                    (a, t) => a.blocked ? a : (a.seen + 1, t >= tree)
                ).seen;
                // Top
                scores[2] = treesT[y].Take(x).Reverse().Aggregate(
                    (seen: 0, blocked: false ),
                    (a, t) => a.blocked ? a : (a.seen + 1, t >= tree)
                ).seen;
                // Bottom
                scores[3] = treesT[y].Skip(x + 1).Aggregate(
                    (seen: 0, blocked: false ),
                    (a, t) => a.blocked ? a : (a.seen + 1, t >= tree)
                ).seen;

                maxScore = Math.Max(maxScore, scores.Aggregate((a, x) => a * x));
            }
        }

        Console.WriteLine($"Max score: {maxScore}");

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Combinatorics.Collections;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2025;

class Day9 : Solver {
    public override void PartOne() {
        var points = GetPoints();

        var combinations = new Combinations<Vector2>(points, 2, GenerateOption.WithoutRepetition);

        var largestArea = combinations
            .Select(g => (Corner1: g[0],Corner2: g[1], Area: GetAreaOfRectangle(g[0], g[1])))
            .OrderByDescending(g => g.Area)
            .First();

        Logger.LogInformation("Largest Area, coords {Vec1}, {Vec2}: {Area:F0}", largestArea.Corner1, largestArea.Corner2, largestArea.Area);
    }

    private long GetAreaOfRectangle(Vector2 corner1, Vector2 corner2)
    {
        var xLength = Math.Abs(corner1.X - corner2.X) + 1;
        var yLength = Math.Abs(corner1.Y - corner2.Y) + 1;
        return (long)xLength * (long)yLength;
    }

    private List<Vector2> GetPoints() {
        return Input
            .Select(x => new Vector2(x.Split(",").Select(float.Parse).ToArray()))
            .ToList();
    }
}

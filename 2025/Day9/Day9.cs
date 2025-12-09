using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Combinatorics.Collections;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
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

    public override void PartTwo()
    {
        var points = GetPoints();
        var polygon = new Polygon2D(points.Select(p => p.Point2D));
        var combinations = new Combinations<Vector2>(points, 2, GenerateOption.WithoutRepetition);

        var largestArea = combinations
            .Select(g => (Corner1: g[0], Corner2: g[1]))
            .Where(g => (long)g.Corner1.X != (long)g.Corner2.X && (long)g.Corner1.Y != (long)g.Corner2.Y)
            .Where(g => IsRectContainedInPolygon(polygon, g.Corner1, g.Corner2))
            .Select(g => (g.Corner1, g.Corner2, Area: GetAreaOfRectangle(g.Corner1, g.Corner2)))
            .OrderByDescending(g => g.Area)
            .ToList();

        // Too low: 1640247570
        // Too high: 4620810700

        Logger.LogInformation("Largest area, {Area:F0}", largestArea.First().Area);
    }

    private bool IsRectContainedInPolygon(Polygon2D polygon, Vector2 p1, Vector2 p3)
    {
        var p2 = new Point2D(p3.X, p1.Y);
        var p4 = new Point2D(p1.X, p3.Y);

        var rect = new Polygon2D(p1.Point2D, p2, p3.Point2D, p4);

        var contained =
            polygon.EnclosesPoint(p1.Point2D)
            && polygon.EnclosesPoint(p2)
            && polygon.EnclosesPoint(p3.Point2D)
            && polygon.EnclosesPoint(p4);

        if (!contained)
            return false;

        foreach (var rectEdge in rect.Edges)
        foreach (var polyEdge in polygon.Edges)
        {
            if (rectEdge == polyEdge)
                continue;

            // If edges don't intersect, the rect is contained
            if (!rectEdge.TryIntersect(polyEdge, out var intersect, Angle.FromDegrees(0)))
                continue;

            // Intersecting at the corners is fine
            if (rect.Vertices.Contains(intersect))
                continue;

            return false;
        }

        return true;
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

public static class Extensions
{
    extension(Vector2 vector)
    {
        public Point2D Point2D => new Point2D(vector.X, vector.Y);
    }
}
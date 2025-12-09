using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Combinatorics.Collections;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2025;

class Day8 : Solver
{
    public override void PartOne()
    {
        var maxConnections = 1000;

        var boxes = GetVectors();

        var combinations = new Combinations<Vector3>(boxes, 2, GenerateOption.WithoutRepetition);

        var connections = combinations
            .Select(g => (Box1: g[0],Box2: g[1], Distance: Vector3.Distance(g[0], g[1])))
            .OrderBy(g => g.Distance)
            .Take(maxConnections)
            .ToList();

        var boxStack = new Stack<Vector3>(boxes);

        var groups = new List<HashSet<Vector3>>();

        foreach (var box in boxes)
        {
            HashSet<Vector3> group;
            if (!groups.Any(g => g.Contains(box)))
            {
                group = [box];
                groups.Add(group);
            }
            else
            {
                group = groups.First(g => g.Contains(box));
            }

            foreach (var connection in connections.Where(c => c.Box1 == box || c.Box2 == box))
            {
                var otherBox = connection.Box1 == box ? connection.Box2 : connection.Box1;
                var otherGroups = groups.Where(g => g != group && g.Contains(otherBox)).ToList();
                foreach (var otherGroup in otherGroups)
                {
                    group.UnionWith(otherGroup);
                    groups.Remove(otherGroup);
                }

                group.Add(otherBox);
            }
        }

        while (boxStack.TryPop(out var box))
        {
            HashSet<Vector3> group;
            if (!groups.Any(g => g.Contains(box)))
            {
                group = [box];
                groups.Add(group);
            }
            else
            {
                group = groups.First(g => g.Contains(box));
            }

            foreach (var connection in connections.Where(c => c.Box1 == box || c.Box2 == box))
            {
                group.Add(connection.Box1);
                group.Add(connection.Box2);
            }
        }

        var topThreeGroups = groups
            .OrderByDescending(g => g.Count)
            .Take(3)
            .Select(g => g.Count)
            .Aggregate((a, b) => a * b);

        Logger.LogInformation("Groups: {Groups}", topThreeGroups);
    }

    private List<Vector3> GetVectors()
    {
        return Input
            .Select(x => new Vector3(x.Split(",").Select(float.Parse).ToArray()))
            .ToList();
    }
}
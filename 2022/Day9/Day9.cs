using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day9 : Solver {

    public override void PartOne() {
        var tail = (x: 0, y: 0);
        var head = (x: 0, y: 0);

        var prevPositions = new Dictionary<(int x, int y), bool>();

        var input = Input
            .Select(l => l.Split(" "))
            .Select(l => (dir: l[0], count: Int32.Parse(l[1])));

        foreach (var (dir, count) in input) {
            foreach (var _ in Enumerable.Range(0, count)) {
                var prevHead = head;

                head = dir switch {
                    "R" => (head.x + 1, head.y),
                    "L" => (head.x - 1, head.y),
                    "U" => (head.x, head.y + 1),
                    "D" => (head.x, head.y - 1),
                    _ => head
                };

                if (
                    (Math.Abs(head.x - tail.x) > 1 && Math.Abs(head.y - tail.y) == 1) || 
                    (Math.Abs(head.x - tail.x) == 1 && Math.Abs(head.y - tail.y) > 1)
                ) {
                    // Head is too far, move diagonally
                    tail = prevHead;
                } else if (Math.Abs(head.x - tail.x) > 1 || Math.Abs(head.y - tail.y) > 1) {
                    // Catch up
                    tail = dir switch {
                        "R" => (tail.x + 1, tail.y),
                        "L" => (tail.x - 1, tail.y),
                        "U" => (tail.x, tail.y + 1),
                        "D" => (tail.x, tail.y - 1),
                        _ => tail
                    };
                } else {
                    // Do nothing, tail and head were ontop of eachother
                }

                prevPositions[tail] = true;
            }
        }

        Console.WriteLine($"Visited: {prevPositions.Where(x => x.Value).Count()}");
    }

    public override void PartTwo() {
        var head = (x: 0, y: 0);
        var trail = Enumerable.Range(0, 9).Select(_ => (x: 0, y: 0)).ToArray();

        var prevPositions = new Dictionary<(int x, int y), bool>();

        var input = Input
            .Select(l => l.Split(" "))
            .Select(l => (dir: l[0], count: Int32.Parse(l[1])));

        foreach (var (dir, count) in input) {
            foreach (var _ in Enumerable.Range(0, count)) {
                head = dir switch {
                    "R" => (head.x + 1, head.y),
                    "L" => (head.x - 1, head.y),
                    "U" => (head.x, head.y + 1),
                    "D" => (head.x, head.y - 1),
                    _ => head
                };

                var lead = head;

                for (var i = 0; i < trail.Length; i++) {
                    var follow = trail[i];

                    if (
                        (Math.Abs(lead.x - follow.x) > 1 && lead.y != follow.y) ||
                        (Math.Abs(lead.y - follow.y) > 1 && lead.x != follow.x)
                    ) {
                        // Diagonal move
                        trail[i] = (
                            follow.x + (lead.x > follow.x ? 1 : - 1),
                            follow.y + (lead.y > follow.y ? 1 : - 1)
                        );
                    } else if (Math.Abs(lead.x - follow.x) > 1 || Math.Abs(lead.y - follow.y) > 1) {
                        if (lead.x > follow.x) trail[i] = (follow.x + 1, follow.y);
                        if (lead.x < follow.x) trail[i] = (follow.x - 1, follow.y);
                        if (lead.y > follow.y) trail[i] = (follow.x, follow.y + 1);
                        if (lead.y < follow.y) trail[i] = (follow.x, follow.y - 1);
                    } else {
                        // Do nothing, leader and follower were on top of eachother
                    }

                    lead = trail[i];
                }

                prevPositions[trail[8]] = true;
            }
        }

        Console.WriteLine($"Visited: {prevPositions.Where(x => x.Value).Count()}");
    }
}
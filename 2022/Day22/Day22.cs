using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day22 : Solver {

    public enum Direction {
        RIGHT = 0,
        DOWN = 1,
        LEFT = 2,
        UP = 3
    }

    static public Direction Rotate(Direction dir, string rotate) {
        var intDir = (int) dir;
        intDir = (intDir + rotate switch {
            "R" => 1,
            "L" => -1,
            _ => throw new ArgumentException()
        }) % 4;

        intDir = intDir < 0 ? 4 + intDir : intDir;
        return (Direction) intDir;
    }

    public override void PartOne() {
        var (mapStr, (inputStr, _)) = InputRaw.Split(Environment.NewLine + Environment.NewLine);

        var map = mapStr.Split(Environment.NewLine);
        var input = Regex
            .Match(inputStr, @"(\d+|[RL])+")
            .Groups[1]
            .Captures
            .Select(c => c.Value)
            .ToList();

        var maxY = map.Length;
        
        (int x, int y) currPos = (map[0].IndexOf('.'), 0);
        Direction currDir = Direction.RIGHT;

        foreach (var cmd in input) {
            // Move command
            if(Int32.TryParse(cmd, out var steps)) {
                var nextPos = currPos;
                while (steps > 0) {
                    nextPos = currDir switch {
                        Direction.RIGHT => (nextPos.x + 1, nextPos.y),
                        Direction.DOWN => (nextPos.x, nextPos.y + 1),
                        Direction.LEFT => (nextPos.x - 1, nextPos.y),
                        Direction.UP => (nextPos.x, nextPos.y - 1),
                        _ => throw new ArgumentException()
                    };

                    // Wrap around
                    nextPos.y = nextPos.y >= maxY ? 0 : nextPos.y;
                    nextPos.y = nextPos.y < 0 ? maxY - 1 : nextPos.y;
                    nextPos.x = nextPos.x >= map[nextPos.y].Length ? 0 : nextPos.x;
                    nextPos.x = nextPos.x < 0 ? map[nextPos.y].Length - 1 : nextPos.x;

                    switch (map[nextPos.y][nextPos.x]) {
                        case ' ':
                            break; // Keep going till we hit a floor/wall tile
                        case '#':
                            steps = 0; // We've hit a wall, don't move any further.
                            break;
                        case '.':
                            currPos = nextPos; // Found a floor tile to move to.
                            steps -= 1;
                            break;
                    }
                }

                Console.WriteLine($"({currPos.x}, {currPos.y}) {Enum.GetName(currDir)} {cmd}");
                continue; // Next command
            }

            // Rotate command
            currDir = Rotate(currDir, cmd);
        }

        var sum = (1000 * (currPos.y + 1)) + (4 * (currPos.x + 1)) + (int) currDir;

        Console.WriteLine($"Result: {sum}");
    }
}
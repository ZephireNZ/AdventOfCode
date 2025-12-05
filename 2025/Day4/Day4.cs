using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2025;

class Day4 : Solver {
    public override void PartOne()
    {
        var grid = GetGrid();

        var xLen = grid.GetLength(0);
        var yLen = grid.GetLength(1);

        var count = 0;

        for (var x = 0; x < xLen; x++)
        {
            for (var y = 0; y < yLen; y++)
            {
                if (grid[x,y] && NeighbourCount(grid, x, y) < 4)
                    count += 1;
            }
        }

        Logger.LogInformation("Total accessible {Count}", count);
    }

    public override void PartTwo()
    {
        var grid = GetGrid();

        var count = 0;

        do
        {
            var removed = RemoveRolls(grid);
            if (removed == 0)
                break;

            count += removed;
            Logger.LogDebug("Removed {Count} this round", removed);
        } while (true);

        Logger.LogInformation("Total removed {Count}", count);
    }

    private int RemoveRolls(bool[,] grid)
    {
        var xLen = grid.GetLength(0);
        var yLen = grid.GetLength(1);

        var count = 0;

        for (var x = 0; x < xLen; x++)
        {
            for (var y = 0; y < yLen; y++)
            {
                if (!grid[x, y]) continue;
                if (NeighbourCount(grid, x, y) >= 4) continue;

                // Remove from the grid
                count += 1;
                grid[x, y] = false;
            }
        }

        return count;
    }

    private bool[,] GetGrid()
    {
        var rows = Input;
        var xLen = rows[0].Length;
        var yLen = rows.Length;

        var ret = new bool[xLen, yLen];

        for (var y = 0; y < rows.Length; y++)
        {
            for (var x = 0; x < rows[y].Length; x++)
            {
                ret[x, y] = rows[y][x] == '@';
            }
        }

        return ret;
    }

    private int NeighbourCount(bool[,] grid, int x, int y)
    {
        var count = 0;
        var xLen = grid.GetLength(0);
        var yLen = grid.GetLength(1);

        foreach (var dx in new[] { -1, 0, 1 })
        {
            foreach (var dy in new[] { -1, 0, 1 })
            {
                if (dx == 0 && dy == 0) continue;

                var px = x + dx;
                var py = y + dy;
                if (px < 0 || px >= xLen || py < 0 || py >= yLen)
                    continue;

                count += grid[px, py] ? 1 : 0;
            }
        }

        return count;
    }
}
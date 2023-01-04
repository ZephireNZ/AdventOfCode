using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day18 : Solver {

    public enum Plane {
        XY,
        XZ,
        YZ
    }

    public List<(Plane p, int x, int y, int z)> GetFaces((int x, int y, int z) c) {
        return new List<(Plane p, int x, int y, int z)>() {
            (Plane.XY, c.x, c.y, c.z),
            (Plane.XY, c.x, c.y, c.z + 1),
            (Plane.XZ, c.x, c.y, c.z),
            (Plane.XZ, c.x, c.y + 1, c.z),
            (Plane.YZ, c.x, c.y, c.z),
            (Plane.YZ, c.x + 1, c.y, c.z)
        };
    }

    public override void PartOne() {
        var cubes = Input
            .Select(c => c.Split(",").Select(v => Int32.Parse(v)).ToArray())
            .Select(c => (x: c[0], y: c[1], z: c[2]));

        var faces = cubes
            .Select(GetFaces)
            .SelectMany(f => f)
            .GroupBy(f => f)
            .Where(f => f.Count() == 1)
            .Count();
        
        Console.WriteLine($"Visible faces: {faces}");
    }

    public override void PartTwo() {
        var cubes = Input
            .Select(c => c.Split(",").Select(v => Int32.Parse(v)).ToArray())
            .Select(c => (x: c[0], y: c[1], z: c[2]))
            .ToList();
        
        var minX = cubes.Min(c => c.x) - 1;
        var minY = cubes.Min(c => c.y) - 1;
        var minZ = cubes.Min(c => c.z) - 1;

        var maxX = cubes.Max(c => c.x) + 1;
        var maxY = cubes.Max(c => c.y) + 1;
        var maxZ = cubes.Max(c => c.z) + 1;

        var visited = new HashSet<(int x, int y, int z)>() {};
        var visitQueue = new Queue<(int x, int y, int z)>() {};

        visited.Add((minX, minY, minZ));
        visitQueue.Enqueue((minX, minY, minZ));

        while (visitQueue.TryDequeue(out var cube)) {
            var neighbours = new List<(int x, int y, int z)>() {
                (cube.x + 1, cube.y, cube.z),
                (cube.x - 1, cube.y, cube.z),
                (cube.x, cube.y + 1, cube.z),
                (cube.x, cube.y - 1, cube.z),
                (cube.x, cube.y, cube.z + 1),
                (cube.x, cube.y, cube.z - 1)
            };

            foreach (var n in neighbours) {
                if(visited.Contains(n)) continue;
                if (n.x < minX || n.x > maxX) continue;
                if (n.y < minY || n.y > maxY) continue;
                if (n.z < minZ || n.z > maxZ) continue;
                if (cubes.Contains(n)) continue;

                visited.Add(n);
                visitQueue.Enqueue(n);
            }
        }

        // We can now calculate faces same as before, then subtract the "walls"
        var faces = visited
            .Select(GetFaces)
            .SelectMany(f => f)
            .GroupBy(f => f)
            .Where(f => f.Count() == 1);
        
        var xy = (maxX-minX+1) * (maxY-minY+1) * 2;
        var xz = (maxX-minX+1) * (maxZ-minZ+1) * 2;
        var yz = (maxY-minY+1) * (maxZ-minZ+1) * 2;

        var facesCount = faces.Count() - xy - xz - yz;

        Console.WriteLine($"Visible outer faces: {facesCount}");
    }
}
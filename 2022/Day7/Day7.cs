using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day7 : Solver {

    public override void PartOne() {

        var currentFolder = "";
        var folderSize = new Dictionary<string, long>();

        foreach (var row in Input) {
            switch (row.Split(" ")) {
                case ["$", "cd", "/"]:
                    currentFolder = "/";
                    break;
                case ["$", "cd", ".."]:
                    currentFolder = currentFolder.Substring(0, currentFolder.LastIndexOf("/"));
                    break;
                case ["$", "cd", var dir]:
                    currentFolder = currentFolder + (currentFolder == "/" ? "" : "/") + dir;
                    break;
                case ["$", "ls"]:
                case ["dir", _]:
                    break;
                case [var size, var file]:
                    if(!folderSize.ContainsKey(currentFolder)) {
                        folderSize[currentFolder] = 0;
                    }

                    folderSize[currentFolder] += Int64.Parse(size);
                    break;
                default:
                    break;
            }
        }

        foreach (var (dir, size) in folderSize.Where(d => d.Key != "/").ToList()) {
            var parentDir = dir.Substring(0, dir.LastIndexOf("/"));
            while (parentDir != "") {
                if(!folderSize.ContainsKey(parentDir)) {
                    folderSize[parentDir] = 0;
                }

                folderSize[parentDir] += size;
                parentDir = parentDir.Substring(0, parentDir.LastIndexOf("/"));
            }
            
            folderSize["/"] += size;
        }

        var sum = folderSize.Where(f => f.Value <= 100000 && f.Key != "/").Select(f => f.Value).Sum();

        Console.WriteLine($"Sum of folders less than 100,000: {sum}.");
    }

    public override void PartTwo() {

        const long DISK_SIZE = 70000000;
        const long DISK_NEEDED = 30000000;

        var currentFolder = "";
        var folderSize = new Dictionary<string, long>();

        foreach (var row in Input) {
            switch (row.Split(" ")) {
                case ["$", "cd", "/"]:
                    currentFolder = "/";
                    break;
                case ["$", "cd", ".."]:
                    currentFolder = currentFolder.Substring(0, currentFolder.LastIndexOf("/"));
                    break;
                case ["$", "cd", var dir]:
                    currentFolder = currentFolder + (currentFolder == "/" ? "" : "/") + dir;
                    break;
                case ["$", "ls"]:
                case ["dir", _]:
                    break;
                case [var size, var file]:
                    if(!folderSize.ContainsKey(currentFolder)) {
                        folderSize[currentFolder] = 0;
                    }

                    folderSize[currentFolder] += Int64.Parse(size);
                    break;
                default:
                    break;
            }
        }

        foreach (var (dir, size) in folderSize.Where(d => d.Key != "/").ToList()) {
            var parentDir = dir.Substring(0, dir.LastIndexOf("/"));
            while (parentDir != "") {
                if(!folderSize.ContainsKey(parentDir)) {
                    folderSize[parentDir] = 0;
                }

                folderSize[parentDir] += size;
                parentDir = parentDir.Substring(0, parentDir.LastIndexOf("/"));
            }
            
            folderSize["/"] += size;
        }

        var used = folderSize["/"];
        var avail = DISK_SIZE - used;
        var cleanup = DISK_NEEDED - avail;

        var result = folderSize.Where(f => f.Value >= cleanup).OrderBy(f => f.Value).First().Value;

        Console.WriteLine($"Smallest folder to remove: {result}.");
    }
}
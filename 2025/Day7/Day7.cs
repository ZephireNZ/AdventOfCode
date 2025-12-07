using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2025;

class Day7 : Solver {
    public override void PartOne()
    {
        var yLen = Input.Length;
        var xLen = Input[0].Length;

        var beams = new bool[xLen];
        var startIndex = Input[0].IndexOf('S');
        beams[startIndex] = true;

        var splits = 0;

        for (var layer = 1; layer < yLen; layer++)
        {
            var splitters = Input[layer]
                .Select((x, i) => x == '^' ? i : -1)
                .Where(i => i != -1)
                .ToArray();

            var newBeams = new bool[beams.Length];
            beams.CopyTo(newBeams, 0);

            foreach (var splitter in splitters)
            {
                if (!beams[splitter]) continue;

                splits += 1;
                newBeams[splitter] = false;
                newBeams[splitter + 1] = true;
                newBeams[splitter - 1] = true;
            }

            newBeams.CopyTo(beams, 0);

            var printout = beams.Select(b => b ? '|' : '.').ToCharString();
            Logger.LogDebug("Beams: {Beams}", printout);
        }

        Logger.LogInformation("Total splits: {Splits}", splits);
    }

    // Precompute splitter locations
    private int[][] Splitters => Input
        .Select(layer =>
            layer
                .Select((x, i) => x == '^' ? i : -1)
                .Where(i => i != -1)
                .ToArray())
        .ToArray();

    // Memoise already calculated timelines to avoid wasted time
    private readonly Dictionary<(int Layer, int Beam), long> _memoTimelines = new();

    public override void PartTwo()
    {
        var startIndex = Input[0].IndexOf('S');

        var timelines = TimelineCounts(1, startIndex);

        Logger.LogInformation("Total timelines: {Timelines}", timelines);
    }

    private long TimelineCounts(int layer, int beamIndex)
    {
        // We've found the bottom of the map
        if (layer >= Input.Length) return 1;

        if (_memoTimelines.TryGetValue((layer, beamIndex), out var memo))
            return memo;

        var splitters = Splitters[layer];

        if (!splitters.Contains(beamIndex))
            return TimelineCounts(layer + 1, beamIndex);

        var timelines = 0L;
        timelines += TimelineCounts(layer + 1, beamIndex - 1);
        timelines += TimelineCounts(layer + 1, beamIndex + 1);
        _memoTimelines[(layer, beamIndex)] = timelines;

        return timelines;
    }
}
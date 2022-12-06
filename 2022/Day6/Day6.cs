using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day6 : Solver {

    public override void PartOne() {
        var window = InputRaw.Take(3).ToList();
        int packetMarker = 0;

        foreach (var (c, i) in InputRaw.Select((c, i) => (c, i + 1)).Skip(3))
        {
            window.Add(c);
            if (window.Distinct().Count() == 4) {
                packetMarker = i;
                break;
            }

            window.RemoveAt(0);
        }

        Console.WriteLine($"Packet Marker index: {packetMarker}");
    }

    public override void PartTwo() {
        var window = InputRaw.Take(13).ToList();
        int packetMarker = 0;

        foreach (var (c, i) in InputRaw.Select((c, i) => (c, i + 1)).Skip(13))
        {
            window.Add(c);
            if (window.Distinct().Count() == 14) {
                packetMarker = i;
                break;
            }

            window.RemoveAt(0);
        }

        Console.WriteLine($"Packet Marker index: {packetMarker}");
    }
}
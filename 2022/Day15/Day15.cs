using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day15 : Solver {

    static long SEARCH_RANGE = 4_000_000L;

    public static long Manhattan((long x, long y) p1, (long x, long y) p2) {
        return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
    }

    class Sensor {
        public (long x, long y) sensor;
        public (long x, long y) beacon;

        public long distance;

        public Sensor((long x, long y) sensor, (long x, long y) beacon) {
            this.sensor = sensor;
            this.beacon = beacon;

            this.distance = Manhattan(sensor, beacon);
        }

        public bool Contains((long x, long y) point) {
            return Manhattan(sensor, point) <= distance;
        }

        public (long min, long max) Slice(long y) {
            var yDist = Math.Abs(y - sensor.y);
            if(yDist > distance) return (0, -1);

            var xDist = distance - yDist;
            return (Math.Max(sensor.x - xDist, 0), Math.Min(sensor.x + xDist, SEARCH_RANGE));
        }

    }

    public override void PartOne() {
        var sensors = Input
            .Select(v => Regex.Match(v, @"Sensor at x=(?<SensorX>[-\d]+), y=(?<SensorY>[-\d]+): closest beacon is at x=(?<BeaconX>[-\d]+), y=(?<BeaconY>[-\d]+)").Groups)
            .Select(g => new Sensor(
                (Int32.Parse(g["SensorX"].Value), Int32.Parse(g["SensorY"].Value)),
                (Int32.Parse(g["BeaconX"].Value), Int32.Parse(g["BeaconY"].Value))
            ))
            .ToList();

        var minX = sensors.Select(s => s.sensor.x - s.distance).Min();
        var maxX = sensors.Select(s => s.sensor.x + s.distance).Max();

        var hasBeacon = 0;

        for (var x = minX; x <= maxX; x++) {
            hasBeacon += sensors.Where(s => s.Contains((x, 2_000_000)) && s.beacon != (x, 2_000_000)).Any() ? 1 : 0;
        }

        Console.WriteLine($"Has Beacons: {hasBeacon}");    
    }

    public override void PartTwo() {

        var sensors = Input
            .Select(v => Regex.Match(v, @"Sensor at x=(?<SensorX>[-\d]+), y=(?<SensorY>[-\d]+): closest beacon is at x=(?<BeaconX>[-\d]+), y=(?<BeaconY>[-\d]+)").Groups)
            .Select(g => new Sensor(
                (Int32.Parse(g["SensorX"].Value), Int32.Parse(g["SensorY"].Value)),
                (Int32.Parse(g["BeaconX"].Value), Int32.Parse(g["BeaconY"].Value))
            ))
            .ToList();

        for (var y = 0; y <= SEARCH_RANGE; y++) {
            var slice = sensors
                .Select(s => s.Slice(y))
                .Where(r => r.max >= r.min)
                .Where(r => r.min <= SEARCH_RANGE && r.max >= 0)
                .OrderBy(r => r.min)
                .ThenBy(r => r.max);

            var max = -1L;

            foreach (var range in slice) {
                if (max + 1 < range.min) {
                    Console.WriteLine($"Found {max + 1}, {y}: {((max + 1) * SEARCH_RANGE) + y}");
                    return;
                }

                max = Math.Max(max, range.max);
            }

            if (max < SEARCH_RANGE) {
                Console.WriteLine($"Found {max + 1}, {y}: {((max + 1) * SEARCH_RANGE) + y}");
                return;
            }
        }
    }
}
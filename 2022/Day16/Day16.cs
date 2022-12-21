using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day16 : Solver {

    public class Valve {

        public string name;
        public int flow;

        public Dictionary<Valve, int> neighbours = new Dictionary<Valve, int>();

        public Valve(string name, int flow) {
            this.name = name;
            this.flow = flow;
        }

        public int FindMaxFlow(IEnumerable<Valve> enabled, int remainingMins) {
            if(remainingMins < 2) return 0; // 1s to turn on plus 1s of value

            var valveFlow = remainingMins * flow;

            var maxFlow = 0;
            foreach (var (n, dist) in neighbours) {
                if (enabled.Contains(n)) continue;
                if (remainingMins <= dist) continue;

                maxFlow = Math.Max(maxFlow, n.FindMaxFlow(enabled.Append(this), remainingMins - dist - 1));
            }

            return valveFlow + maxFlow;
        }

        public override string ToString() {
            return this.name;
        }
    }

    public int FindMaxFlow(IEnumerable<Valve> visited, IEnumerable<(Valve l, int t)> movers) {
        movers = movers.OrderByDescending(m => m.t);
        var mover = movers.First();
        var others = movers.Skip(1);
        if (mover.t < 2) return 0; // Neither has enough time left

        var (valve, remainingMins) = mover;

        var valveFlow = valve.flow * remainingMins;

        var maxFlow = 0;
        foreach (var (n, dist) in valve.neighbours) {
            if (visited.Contains(n)) continue;
            if (remainingMins <= dist) continue;
            maxFlow = Math.Max(maxFlow, FindMaxFlow(visited.Append(n), others.Append((n, remainingMins - dist - 1))));
        }

        maxFlow = Math.Max(maxFlow, FindMaxFlow(visited.Append(valve), others.Append((valve, 0))));

        return valveFlow + maxFlow;
    }

    public void UpdateNeighbours(IEnumerable<Valve> valves) {
        var important = valves.Where(v => v.flow > 0 || v.name == "AA").ToList();

        foreach (var valve in important) {
            var visitQueue = new Queue<Valve>();
            var distance = new Dictionary<Valve, int>();

            visitQueue.Enqueue(valve);
            distance[valve] = 0;

            while (visitQueue.TryDequeue(out var v)) {
                foreach (var (n, dist) in v.neighbours) {
                    if (distance.ContainsKey(n)) continue;

                    visitQueue.Enqueue(n);
                    distance.Add(n, distance[v] + dist);
                }
            }

            valve.neighbours = distance
                .Where(n => important.Contains(n.Key) && n.Key != valve)
                .ToDictionary(n => n.Key, n => n.Value);
        }
    }

    public Dictionary<string, Valve> GenerateValves() {
        var input = Input
            .Select(v => Regex.Match(v, @"Valve (?<VALVE>\w+) has flow rate=(?<FLOW>\d+); tunnel(s|) lead(s|) to valve(s|) (?<TUNNELS>.*)").Groups);

        var valves = input.Select(g => new Valve(
                g["VALVE"].Value,
                Int32.Parse(g["FLOW"].Value)
            ))
            .ToDictionary(v => v.name, v => v);
        
        foreach (var row in input) {
            var valve = valves[row["VALVE"].Value];
            valve.neighbours = row["TUNNELS"]
                .Value
                .Split(",")
                .Select(v => valves[v.Trim()]).ToDictionary(v => v, _ => 1);
        }

        return valves;
    }

    public override void PartOne() {
        var valves = GenerateValves();

        UpdateNeighbours(valves.Values);

        var maxFlow = valves["AA"].FindMaxFlow(Enumerable.Empty<Valve>(), 30);

        Console.WriteLine($"Max Flow: {maxFlow}");
    }

    public override void PartTwo() {

        var valves = GenerateValves();

        UpdateNeighbours(valves.Values);

        var movers = new List<(Valve l, int t)>() {
            (valves["AA"], 26),
            (valves["AA"], 26)
        };

        var maxFlow = FindMaxFlow(new [] {valves["AA"]}, movers);

        Console.WriteLine($"Max Flow: {maxFlow}");
    }
}
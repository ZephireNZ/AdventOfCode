using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;

abstract class Solver {
    public virtual void PartOne() => Console.WriteLine("PartOne Not Implemented");
    public virtual void PartTwo() => Console.WriteLine("PartTwo Not Implemented");

    public string[] Input {
        get {

            var match = new Regex(@"AdventOfCode\.Y(?<YEAR>\d{4})\.Day(?<DAY>\d+)").Match(this.GetType().FullName);
            var (year, day) = (match.Groups["YEAR"].Value, match.Groups["DAY"].Value);

            var path = Path.Combine(year, $"Day{day}", "input.txt");

            return File.ReadAllLines(path);
        }
    }

    public string InputRaw {
        get {

            var match = new Regex(@"AdventOfCode\.Y(?<YEAR>\d{4})\.Day(?<DAY>\d+)").Match(this.GetType().FullName);
            var (year, day) = (match.Groups["YEAR"].Value, match.Groups["DAY"].Value);

            var path = Path.Combine(year, $"Day{day}", "input.txt");

            return File.ReadAllText(path);
        }
    }
}

public static class Extensions {
    public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {
        first = list.Count > 0 ? list[0] : default(T); // or throw
        rest = list.Skip(1).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest) {
        first = list.Count > 0 ? list[0] : default(T); // or throw
        second = list.Count > 1 ? list[1] : default(T); // or throw
        rest = list.Skip(2).ToList();
    }
}

class App {

    static void RunCommand(int year, int day) {

        Console.WriteLine($"Running solver for {year} Day {day}");

        var solver = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && !t.IsAbstract && typeof(Solver).IsAssignableFrom(t))
            .OrderBy(t => t.FullName)
            .Where(t => t.FullName == $"AdventOfCode.Y{year}.Day{day}")
            .Select(t => Assembly.GetEntryAssembly()!.CreateInstance(t.FullName) as Solver)
            .First();
        
        solver.PartOne();
        solver.PartTwo();
    }

    static async Task<int> Main(string[] args) {
        var yearArg = new Argument<int>(
            name: "year",
            description: "Year to execute solution for"
        );

        var dayArg = new Argument<int>(
            name: "day",
            description: "Day to execute solution for"
        );

        var rootCommand = new RootCommand("Solutions for AdventOfCode.");
        rootCommand.AddArgument(yearArg);
        rootCommand.AddArgument(dayArg);

        rootCommand.SetHandler(RunCommand, yearArg, dayArg);

        return await rootCommand.InvokeAsync(args);
    }
}
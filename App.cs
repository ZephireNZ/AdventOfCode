using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdventOfCode;

abstract class Solver {
    public abstract int Year { get; }
    public abstract int Day { get; }

    public virtual void PartOne() => Console.WriteLine("PartOne Not Implemented");
    public virtual void PartTwo() => Console.WriteLine("PartTwo Not Implemented");

    public string[] Input {
        get {
            var path = Path.Combine(Year.ToString(), $"Day{Day}");
            path = Path.Combine(path, "input.txt");

            return File.ReadAllLines(path);
        }
    }
}

class App {

    static void RunCommand(int year, int day) {

        Console.WriteLine($"Running solver for {year} Day {day}");

        var solver = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && !t.IsAbstract && typeof(Solver).IsAssignableFrom(t))
            .OrderBy(t => t.FullName)
            .Select(t => Assembly.GetEntryAssembly()!.CreateInstance(t.FullName) as Solver)
            .Where(s => s.Year == year && s.Day == day)
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
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Numerics;
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

public static class MathHelpers
{
    public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
    {
        while (b != T.Zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T>
        => a / GreatestCommonDivisor(a, b) * b;
    
    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(LeastCommonMultiple);
}

class App {

    const string CS_TEMPLATE = @"using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y{0};

class Day{1} : Solver {{
    public override void PartOne() {{
        
    }}
}}";

    static void RunCommand(int year, int day, bool test) {

        Console.WriteLine($"Running solver for {year} Day {day}");

        var solver = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && !t.IsAbstract && typeof(Solver).IsAssignableFrom(t))
            .OrderBy(t => t.FullName)
            .Where(t => t.FullName == $"AdventOfCode.Y{year}.Day{day}")
            .Select(t => Assembly.GetEntryAssembly()!.CreateInstance(t.FullName) as Solver)
            .First();
        
        var console = Console.Out;

        if (test) {
            Console.SetOut(TextWriter.Null); // Disable output for code

            DateTime start;
            TimeSpan duration;

            var part1Times = new List<double>();
            var part2Times = new List<double>();

            foreach (var _ in Enumerable.Range(0, 5)) // Repeat 10x
            {
                start = DateTime.Now;
                solver.PartOne();
                duration = DateTime.Now - start;
                part1Times.Add(duration.TotalMilliseconds);

                start = DateTime.Now;
                solver.PartTwo();
                duration = DateTime.Now - start;
                part2Times.Add(duration.TotalMilliseconds);
            }

            Console.SetOut(console);

            Console.WriteLine("=== TEST RESULTS ===");
            Console.WriteLine($"Part 1: {part1Times.Average().ToString()}ms");
            Console.WriteLine($"Part 2: {part2Times.Average().ToString()}ms");

            return;
        }
        
        
        
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

        var testOpt = new Option<bool>(
            name: "--test",
            description: "Measure performance of code",
            getDefaultValue: () => false
        );

        var rootCommand = new RootCommand("Solutions for AdventOfCode.");
        rootCommand.AddArgument(yearArg);
        rootCommand.AddArgument(dayArg);
        rootCommand.AddOption(testOpt);

        var createCommand = new Command("create");
        createCommand.AddArgument(yearArg);
        createCommand.AddArgument(dayArg);
        createCommand.SetHandler((year, day) => {
            var path = Path.Combine(year.ToString(), $"Day{day}");
            Directory.CreateDirectory(path);

            File.WriteAllText(Path.Combine(path, $"Day{day}.cs"), String.Format(CS_TEMPLATE, year, day));
            File.Create(Path.Combine(path, "input.txt"));

            var launchStr = File.ReadAllText(Path.Combine(".vscode", "launch.json"));

            launchStr = Regex.Replace(
                launchStr,
                @"""args"": \[""\d+"", ""\d+""\]",    
                $"\"args\": [\"{year}\", \"{day}\"]");

            File.WriteAllText(Path.Combine(".vscode", "launch.json"), launchStr);

            Console.WriteLine($"Done! Created {path}");
        }, yearArg, dayArg);

        rootCommand.Add(createCommand);

        rootCommand.SetHandler(RunCommand, yearArg, dayArg, testOpt);

        return await rootCommand.InvokeAsync(args);
    }
}
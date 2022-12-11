using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022;

class Day11 : Solver {

    class Monkey {
        public int id;

        public Queue<long> items;

        public Func<long, long> operation;

        public int divisor;

        public Func<long, bool> test;

        public int monkeyTrue;

        public int monkeyFalse;

        public long inspected = 0;
    }

    Func<long, long> operation_func(string operation) {
        var (left, (op, (right, _))) = operation.Split("=")[1].Trim().Split(" ");

        return (old) => {
            var leftV = left == "old" ? old : Int32.Parse(left);
            var rightV = right == "old" ? old : Int32.Parse(right);

            return op switch {
                "*" => leftV * rightV,
                "/" => leftV / rightV,
                "+" => leftV + rightV,
                "-" => leftV - rightV,
                _ => throw new ArgumentException()
            };
        };
    }

    List<Monkey> create_monkeys() {
        var monkeys = new List<Monkey>();
        var input = InputRaw
            .Split(Environment.NewLine + Environment.NewLine)
            .Select((m, i) => (m, i));

        foreach (var (mConfig, i) in input) {
            var monkey = new Monkey();
            monkey.id = i;

            foreach (var conf in mConfig.Split(Environment.NewLine).Select(l => l.Trim())) {
                switch (conf.Split(":")) {
                    case ["Starting items", var items]:
                        monkey.items = new Queue<long>(items
                            .Split(",")
                            .Select(i => Int64.Parse(i.Trim())));
                        break;
                    case ["Operation", var operation]:
                        monkey.operation = operation_func(operation);
                        break;
                    case ["Test", var test]:
                        var testVal = Int32.Parse(Regex.Match(test, @"divisible by (\d+)").Groups[1].Value);
                        monkey.test = (val) => val % testVal == 0;
                        monkey.divisor = testVal;
                        break;
                    case ["If true", var m]:
                        var mTrue = Int32.Parse(Regex.Match(m, @"throw to monkey (\d+)").Groups[1].Value);
                        monkey.monkeyTrue = mTrue;
                        break;
                    case ["If false", var m]:
                        var mFalse = Int32.Parse(Regex.Match(m, @"throw to monkey (\d+)").Groups[1].Value);
                        monkey.monkeyFalse = mFalse;
                        break;
                }
            }

            monkeys.Add(monkey);
        }

        return monkeys;
    }

    public override void PartOne() {
        var monkeys = create_monkeys();

        foreach (var round in Enumerable.Range(1, 20)) {
            Console.WriteLine($"Round {round}");

            foreach (var monkey in monkeys) {
                while(monkey.items.TryDequeue(out var item)) {
                    monkey.inspected += 1;
                    item = monkey.operation(item) / 3;

                    var toMonkey = monkey.test(item) ? monkey.monkeyTrue : monkey.monkeyFalse;

                    monkeys[toMonkey].items.Enqueue(item);
                }
            }

            foreach (var monkey in monkeys) {
                var items = monkey.items.Select(i => i.ToString());
                Console.WriteLine($"{monkey.id}\t{String.Join(",", items)}");
            }
        }

        var monkeyBusiness = monkeys
            .Select(m => m.inspected)
            .OrderDescending()
            .Take(2)
            .Aggregate((m, i) => m*i);

        Console.WriteLine($"Monkey Business: {monkeyBusiness}");
    }

    public override void PartTwo() {
        var monkeys = create_monkeys();

        var globalModulo = monkeys.Select(m => m.divisor).Aggregate((m,i) => m*i);

        foreach (var round in Enumerable.Range(1, 10000)) {
            foreach (var monkey in monkeys) {
                while(monkey.items.TryDequeue(out var item)) {
                    monkey.inspected += 1;
                    item = monkey.operation(item) % globalModulo;

                    var toMonkey = monkey.test(item) ? monkey.monkeyTrue : monkey.monkeyFalse;

                    monkeys[toMonkey].items.Enqueue(item);
                }
            }

            
        }

        var monkeyBusiness = monkeys
            .Select(m => m.inspected)
            .OrderDescending()
            .Take(2)
            .Aggregate((m, i) => m*i);

        Console.WriteLine($"Monkey Business: {monkeyBusiness}");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day21 : Solver {

    long Compute(string monkey, Dictionary<string, string> monkeys, Dictionary<string, long> cache) {
        var eq = monkeys[monkey];
        long value;

        if (cache.ContainsKey(monkey)) return cache[monkey];

        switch (eq.Split(' ')) {
            case [var val]:
                value = Int32.Parse(val);
                break;
            case [var a, var op, var b]:
                var aVal = Compute(a, monkeys, cache);
                var bVal = Compute(b, monkeys, cache);

                value = op switch {
                    "+" => aVal + bVal,
                    "-" => aVal - bVal,
                    "*" => aVal * bVal,
                    "/" => aVal / bVal,
                    _ => 0
                };
                break;
            default:
                value = 0;
                break;
        }

        cache[monkey] = value;
        return value;
    }

    public abstract class Monkey {
        public string name;

        public virtual Monkey Simplify() {
            return this;
        }

        public abstract string Render();
    }

    public class OpMonkey : Monkey {
        public Monkey left;
        public Monkey right;
        public string op;

        public OpMonkey(string name, Monkey left, Monkey right, string op) {
            this.name = name;
            this.left = left;
            this.right = right;
            this.op = op;
        }

        public override Monkey Simplify() {
            left = left.Simplify();
            right = right.Simplify();

            if (left is NumMonkey leftN && right is NumMonkey rightN) {
                return this.op switch {
                    "+" => new NumMonkey(this.name, leftN.value + rightN.value),
                    "-" => new NumMonkey(this.name, leftN.value - rightN.value),
                    "*" => new NumMonkey(this.name, leftN.value * rightN.value),
                    "/" => new NumMonkey(this.name, leftN.value / rightN.value),
                    _ => this
                };
            }

            return this;
        }

        public override string Render() {
            return $"({left.Render()} {op} {right.Render()})";
        }
    }

    public class NumMonkey : Monkey {
        public long value;

        public NumMonkey(string name, long value) {
            this.name = name;
            this.value = value;
        }

        public override string Render() {
            return value.ToString();
        }
    }

    public class Human : Monkey {
        public Human(string name) {
            this.name = name;
        }

        public override string Render() {
            return "x";
        }
    }

    public override void PartOne() {
        var monkeys = Input
            .Select(m => m.Split(": "))
            .ToDictionary(m => m[0], m => m[1]);

        var cache = new Dictionary<string, long>();

        var result = Compute("root", monkeys, cache);

        Console.WriteLine($"Result: {result}");
    }

    public Monkey Convert(string name, Dictionary<string, string> monkeys, Dictionary<string, Monkey> cache) {
        if(cache.ContainsKey(name)) return cache[name];

        Monkey monkey;

        if(name == "humn") {
            monkey = new Human(name);
            cache[name] = monkey;
            return monkey;
        }

        monkey = monkeys[name].Split(' ') switch {
            [var num] => new NumMonkey(name, Int64.Parse(num)),
            [var left, var op, var right] => new OpMonkey(
                name,
                Convert(left, monkeys, cache),
                Convert(right, monkeys, cache),
                name == "root" ? "=" : op
            ),
            _ => throw new ArgumentException()
        };

        cache[name] = monkey;
        return monkey;
    }

    public override void PartTwo()
    {
        var monkeys = Input
            .Select(m => m.Split(": "))
            .ToDictionary(m => m[0], m => m[1]);

        var cache = new Dictionary<string, Monkey>();
        
        var root = (OpMonkey) Convert("root", monkeys, cache);

        root.Simplify();

        Console.WriteLine($"Full eq: {root.Render()}");

        // I wrote all this and then it turns out non-integer division was too hard sooooo online solver 

        // var parentVal = (decimal) (root.left is NumMonkey ? ((NumMonkey) root.left).value : ((NumMonkey) root.right).value);
        // OpMonkey parent = root.left is OpMonkey ? (OpMonkey) root.left : (OpMonkey) root.right;
        
        // while (true) {
        //     var num = parent.left is NumMonkey ? ((NumMonkey) parent.left).value : ((NumMonkey) parent.right).value;

        //     if(parent.op == "*" && parentVal % num != 0) {
        //         Console.WriteLine("Oops!");
        //     }

        //     parentVal = parent.op switch {
        //         "+" => parentVal - num,
        //         "-" => parentVal + num,
        //         "*" => parentVal / num,
        //         "/" => parentVal * num,
        //         _ => throw new ArgumentException()
        //     };

        //     if (parent.left is Human || parent.right is Human) {
        //         Console.WriteLine($"Human should yell: {parentVal}");
        //         break;
        //     }

        //     parent = parent.left is OpMonkey ? (OpMonkey) parent.left : (OpMonkey) parent.right;
        // }
    }
}
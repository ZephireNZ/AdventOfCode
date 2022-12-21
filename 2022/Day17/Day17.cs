using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022;

class Day17 : Solver {

    string[][] SHAPES = new string[] {
        @"####",
        @".#.
          ###
          .#.",
        @"..#
          ..#
          ###",
        @"#
          #
          #
          #",
        @"##
          ##"
    }.Select(s => s.Split(Environment.NewLine).Select(l => l.Trim()).ToArray()).ToArray();

    class Map {
        public List<bool[]> map = new List<bool[]>();
    }

    class Shape {
        bool[][] shape;

        int height;

        Shape(string shape) {
            this.shape = shape
                .Split(Environment.NewLine)
                .Select(l => l.Trim())
                .Select(l => l.Select(c => c == '#').ToArray())
                .ToArray();
            
            this.height = shape.Length;
        }

        public bool IsValid((int x, int y) pos, List<bool[]> map) {
            return true;
        }
    }

    public override void PartOne() {
        var input = Input;


    }
}
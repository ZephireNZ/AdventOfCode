using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2021;

class Day4 : Solver {

    class BingoCard {

        public int width;

        public int[] numbers;

        public bool[] hits;

        public BingoCard(int[] numbers, int width) {
            this.numbers = numbers;
            this.width = width;
            this.hits = new bool[numbers.Length];
        }

        public bool draw(int number) {
            for(var i = 0; i < numbers.Length; i++) {
                if(numbers[i] == number) {
                    hits[i] = true;
                }
            }

            // Check Rows
            for (var i = 0; i < hits.Length; i += width) {
                var bingo = true;

                for (var j = 0; j < width; j++) {
                    if (!hits[j + i]) {
                        bingo = false;
                        break;
                    }
                }

                if (bingo) return true;
            }

            // Check Columns
            for (var j = 0; j < width; j++) {
                var bingo = true;

                for (var i = 0; i < hits.Length; i += width) {
                    if (!hits[j + i]) {
                        bingo = false;
                        break;
                    }
                }

                if (bingo) return true;
            }

            return false;
        }

    }

    public override void PartOne() {
        var input = Input;

        var drawOrder = input[0].Split(',').Select(Int32.Parse);

        var cards = new List<BingoCard>();

        var currentCard = new List<int>();
        var width = 0;

        foreach (var line in input.Skip(2))
        {
            if(String.IsNullOrEmpty(line)) {
                cards.Add(new BingoCard(currentCard.ToArray(), width));
                currentCard = new List<int>();
            } else {
                var nums = Regex.Split(line.Trim(), @"\s+").Select(Int32.Parse);
                width = nums.Count();
                currentCard.AddRange(nums);
            }
        }

        cards.Add(new BingoCard(currentCard.ToArray(), width));

        var bingo = false;
        foreach (var call in drawOrder)
        {
            foreach (var card in cards)
            {
                bingo = card.draw(call);
                if (bingo) {
                    Console.WriteLine("BINGO!");

                    var sum = 0;
                    for (var i = 0; i < card.numbers.Length; i++) {
                        if (!card.hits[i]) {
                            sum += card.numbers[i];
                        }
                    }

                    Console.WriteLine($"Bingo Sum: {sum*call}");

                    break;
                }
            }

            if (bingo) break;
        }
    }

    public override void PartTwo() {
        var input = Input;

        var drawOrder = input[0].Split(',').Select(Int32.Parse);

        var cards = new List<BingoCard>();

        var currentCard = new List<int>();
        var width = 0;

        foreach (var line in input.Skip(2))
        {
            if(String.IsNullOrEmpty(line)) {
                cards.Add(new BingoCard(currentCard.ToArray(), width));
                currentCard = new List<int>();
            } else {
                var nums = Regex.Split(line.Trim(), @"\s+").Select(Int32.Parse);
                width = nums.Count();
                currentCard.AddRange(nums);
            }
        }

        cards.Add(new BingoCard(currentCard.ToArray(), width));

        foreach (var call in drawOrder)
        {
            foreach (var card in cards.ToList())
            {
                var bingo = card.draw(call);
                if (bingo) {
                    cards.Remove(card);
                    if (cards.Count == 0) {
                        Console.WriteLine("(final) BINGO!");

                        var sum = 0;
                        for (var i = 0; i < card.numbers.Length; i++) {
                            if (!card.hits[i]) {
                                sum += card.numbers[i];
                            }
                        }

                        Console.WriteLine($"Bingo Sum: {sum*call}");
                    }
                }
            }
        }
    }
}
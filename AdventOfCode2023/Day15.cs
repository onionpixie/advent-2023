

using System.Data;

namespace AdventOfCode
{
    public class Day15 : IDay
    {
        private string[] Lines { get; set; }

        public Day15() {
            Lines = File.ReadAllLines("./Day15.txt");
        }

        public string SolveA () {
            var charsToHash = Lines[0].Split(',').Select(x => x.ToCharArray()).ToArray();
            var sum = 0;
            for (int i = 0; i < charsToHash.Length; i++) {
                var hash = 0;
                foreach (var symbol in charsToHash[i]) {
                    hash += symbol;
                    hash *= 17;
                    hash %= 256;
                }

                sum += hash;
                hash = 0;
            }

            return sum.ToString();
        }

        public string SolveB () {
            return "B";
        }
    }
}
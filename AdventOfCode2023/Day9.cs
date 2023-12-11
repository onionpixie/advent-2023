namespace AdventOfCode
{
    public class Day9 : IDay
    {
        private string[] Lines { get; set; }

        public Day9() {
            Lines = File.ReadAllLines("./Day9.txt");
        }

        public string SolveA () {
            var nextNumbers = new List<int>();
            foreach (var line in Lines) {
                var nextSequence = line.Split(' ').Select(x => int.Parse(x)).ToList();
                nextNumbers.Add(FindNextInSequence(nextSequence));
            }

            return nextNumbers.Sum().ToString();
        }

        public string SolveB () {
            var nextNumbers = new List<int>();
            foreach (var line in Lines) {
                var nextSequence = line.Split(' ').Select(x => int.Parse(x)).ToList();
                nextNumbers.Add(FindPreviousInSequence(nextSequence));
            }
            
            return nextNumbers.Sum().ToString();
        }

        private int FindNextInSequence(List<int> currentSequence) {
            var difference = new List<int>();
            for (int i = 1; i < currentSequence.Count; i++) {
                difference.Add(currentSequence[i] - currentSequence[i-1]);
            }

            if (difference.Distinct().Count() > 1) {
                return currentSequence.Last() + FindNextInSequence(difference);
            }

            return currentSequence.Last() + difference.Distinct().Single();
        }

        private int FindPreviousInSequence(List<int> currentSequence) {
            var difference = new List<int>();
            for (int i = 1; i < currentSequence.Count; i++) {
                difference.Add(currentSequence[i] - currentSequence[i-1]);
            }

            if (difference.Distinct().Count() > 1) {
                return currentSequence.First() - FindPreviousInSequence(difference);
            }

            return currentSequence.First() - difference.Distinct().Single();
        }
    }
}
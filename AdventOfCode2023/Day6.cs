using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day6 : IDay
    {
        private string[] Lines { get; set; }

        public Day6() {
            Lines = File.ReadAllLines("./Day6.txt");
        }

        public string SolveA () {
            var times =  Regex.Split(Lines[0], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();
            var distances =  Regex.Split(Lines[1], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();
            var validTimes = new List<int>();
            for (int i = 0; i < times.Length; i++) {
                var distance = distances[i];
                var time = times[i];

                var shortestTime = 0;
                for (int j = 0; j < time; j++) {
                    var timeHoldingButton = j;
                    var timeLeftToRace = time - j;
                    if ((timeHoldingButton * timeLeftToRace) <= distance) continue;

                    shortestTime = j;
                    break;
                }

                var longestTime = 0;
                for (int j = time; j > 0; j--) {
                    var timeHoldingButton = j;
                    var timeLeftToRace = time - j;
                    if ((timeHoldingButton * timeLeftToRace) <= distance) continue;

                    longestTime = j;
                    break;
                }

                validTimes.Add(longestTime - shortestTime + 1);
            }

            return validTimes.Aggregate((a, b) => a * b).ToString();
        }

        public string SolveB () {
            var times =  Regex.Split(Lines[0], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var time = long.Parse(string.Join("", times));
            var distances =  Regex.Split(Lines[1], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var distance = long.Parse(string.Join("", distances));

            long shortestTime = 0;
            for (long j = 0; j < time; j++) {
                var timeHoldingButton = j;
                var timeLeftToRace = time - j;
                if ((timeHoldingButton * timeLeftToRace) <= distance) continue;

                shortestTime = j;
                break;
            }

            long longestTime = 0;
            for (long j = time; j > 0; j--) {
                var timeHoldingButton = j;
                var timeLeftToRace = time - j;
                if ((timeHoldingButton * timeLeftToRace) <= distance) continue;

                longestTime = j;
                break;
            }

            return (longestTime - shortestTime + 1).ToString();
        }
    }
}
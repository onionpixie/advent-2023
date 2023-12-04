using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day2 : IDay
    {
        private string[] Lines { get; set; }
        public Day2() {
            Lines = File.ReadAllLines("./Day2.txt");
        }

        public string SolveA () {
            double total = 0;

            for (int i = 0; i < Lines.Length; i++) {
                var cubes = Lines[i].Split(": ").Last();
                var numbers = Regex.Split(cubes, @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();
                if (numbers.Any(n => n > 14)) {
                    continue;
                }

                var possible = true;
                var moreCubeInfo = Regex.Split(cubes, @"\W+");
                for (int j = 0; j < moreCubeInfo.Length; j += 2) {
                        if (int.Parse(moreCubeInfo[j]) > 13){
                            if (moreCubeInfo[j + 1] != "blue") {
                                possible = false;
                            break;
                            }
                        }
                        if (int.Parse(moreCubeInfo[j]) > 12) {
                            if (moreCubeInfo[j + 1] == "red") {
                                possible = false;
                            break;
                            }
                        }
                    }

                    if (!possible) continue;

                    total += i + 1;
                }
            return total.ToString();
        }

        public string SolveB () {
            double total = 0;

            for (int i = 0; i < Lines.Length; i++) {
                var redCubes = 0;
                var blueCubes = 0;
                var greenCubes = 0;
                var cubes = Lines[i].Split(": ").Last();
                var moreCubeInfo = Regex.Split(cubes, @"\W+");
                for (int j = 0; j < moreCubeInfo.Length; j += 2) {
                    var cubeCount = int.Parse(moreCubeInfo[j]);
                    switch (moreCubeInfo[j + 1]){
                        case "red":
                            if (cubeCount > redCubes){
                                redCubes = cubeCount;
                            }
                            break;
                        case "blue":
                            if (cubeCount > blueCubes){
                                blueCubes = cubeCount;
                            }
                            break;
                        case "green":
                            if (cubeCount > greenCubes){
                                greenCubes = cubeCount;
                            }
                            break;
                    }  
                }

                var power = greenCubes * redCubes * blueCubes;
                total += power;
            }

            return total.ToString();
        }
    }
}

namespace AdventOfCode
{
    public class Day12 : IDay
    {
        private string[] Lines { get; set; }

        public Day12() {
            Lines = File.ReadAllLines("./Day12.txt");
        }

        public string SolveA () {
            var possibleSolutions = 0;
            foreach (var line in Lines) {
                var numberBrokenStrings = line.Split(' ')[1].Split(',').Select(x => int.Parse(x)).ToArray();
                var springs = line.Split(' ')[0].ToCharArray();

                // if this is true possible solutions = 1
                if (springs.Length == numberBrokenStrings.Sum() + numberBrokenStrings.Length -1){
                    possibleSolutions += 1;
                    continue;
                }

                var possibleArrangements = new List<List<char>> {
                    new()
                };
                var trialOutput = new List<char>();
                var currentNumberBrokenStrings = numberBrokenStrings.First();
                
                for (int i = 0; i < springs.Length; i++) {
                    var currentSpring = springs[i];
                    if (currentSpring != '?') {
                        possibleArrangements.ForEach(x => x.Add(currentSpring));
                        continue;
                    }

                    if (i == 0) {
                        possibleArrangements.Add(new List<char>(){'.'});
                        possibleArrangements.Add(new List<char>(){'#'});
                        continue;
                    }

                    if (i != 0 && springs[i - 1] == '.') {
                        possibleArrangements.ForEach(x => x.Add('.'));
                        var count = possibleArrangements.Count();
                        for (int m = 0; m < count; m++){
                            var newArrangement = new List<char>(possibleArrangements[m]);
                            newArrangement[newArrangement.Count -1] = '#';
                            possibleArrangements.Add(newArrangement);
                        }
                        continue;
                    }

                    for (int k = 0; k < possibleArrangements.Count; k++) {
                        var currentFoundCompleteSpringsInArrangment = 0;
                        var currentSpringBlockSize = 0;
                        var currentNumBrokenStrings = numberBrokenStrings[currentFoundCompleteSpringsInArrangment];
                        for (int l = 0; l < possibleArrangements[k].Count; l++) {
                            if (possibleArrangements[k][l] == '#') {
                                currentSpringBlockSize += 1;
                                continue;
                            }
                            
                            if (currentSpringBlockSize == currentNumBrokenStrings) { 
                                currentFoundCompleteSpringsInArrangment += 1;
                            }

                            currentSpringBlockSize = 0;
                        }

                        if (currentSpringBlockSize == currentNumBrokenStrings) { 
                            possibleArrangements[k].Add('.');
                            continue;
                        }

                        if (possibleArrangements[k].TakeLast(numberBrokenStrings[currentFoundCompleteSpringsInArrangment]).Count(x => x == '#') == numberBrokenStrings[currentFoundCompleteSpringsInArrangment]) {
                            possibleArrangements[k].Add('.');
                        }
                        else {
                            possibleArrangements[k].Add('#');
                        }
                    }
                }

                var distinctArrangements = possibleArrangements.Distinct();
                var numberOfRequiredSprings = numberBrokenStrings.Sum();
                foreach (var possibleArrangement in possibleArrangements){
                    if (possibleArrangement.Count(x => x == '#') == numberOfRequiredSprings) {
                        possibleSolutions += 1;        
                    }
                }
            }
            return possibleSolutions.ToString();
        }


        public string SolveB () {
            return "B";
        }
    }
}
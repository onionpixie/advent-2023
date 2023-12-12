
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;

=======
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
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
<<<<<<< HEAD
                if (springs.Length == numberBrokenStrings.Sum() + numberBrokenStrings.Length -1) {
                    possibleSolutions += 1;
                    Console.WriteLine($"Combinations found: 1");
                    continue;
                }

                // if this is true possible solutions = 1
                if (springs.Where(x => x == '?' || x == '#').Count() == numberBrokenStrings.Sum()) {
                    possibleSolutions += 1;
                    Console.WriteLine($"Combinations found: 1");
                    continue;
                }

                var groups = line.Split(' ')[0].Split('.', StringSplitOptions.RemoveEmptyEntries);

                var combinations = 0;
                if (groups.Count() == numberBrokenStrings.Count()) {
                    for (int i = 0; i < groups.Count(); i++) {
                        if (groups[i].ToCharArray().Length == numberBrokenStrings[i]) {
                            continue;
                        }

                        combinations += (groups[i].ToCharArray().Length - numberBrokenStrings[i]) + 1;
                    }
                }
                else if (groups.Count() < numberBrokenStrings.Count()) {
                    var index = 0;
                    for (int y = 0; y < groups.Length; y++) {
                        string? group = groups[y];
                        var chars = group.Replace("?#", "?.#").Replace("#?", "#.?").Split('.');
                        var currentUnsolvedNumber = numberBrokenStrings[index];
                        var lengthUnused = 0;
                        for (int k = 0; k < chars.Length; k++) {
                            if (chars[k].First() == '#') {
                                if (chars[k].Length == currentUnsolvedNumber){
                                    index ++;
                                    currentUnsolvedNumber = numberBrokenStrings[index];
                                    lengthUnused = 0;
                                    continue;
                                }

                                lengthUnused += chars[k].Length;
                            }
                            else {
                                var length = chars[k].Length;
                                if (lengthUnused != 0) {
                                    var nextLength = chars[k].Length + lengthUnused;
                                    if (nextLength == currentUnsolvedNumber) {
                                        index ++;
                                        currentUnsolvedNumber = numberBrokenStrings[index];
                                        lengthUnused = 0;
                                        continue;
                                    }
                                    if (nextLength < currentUnsolvedNumber) {
                                        lengthUnused += chars[k].Length;
                                        continue;
                                    }
                                    if (nextLength > currentUnsolvedNumber) {
                                        length -= (currentUnsolvedNumber - lengthUnused - 1);
                                    }
                                }

                                if (length < currentUnsolvedNumber) {
                                    lengthUnused += length;
                                    continue;
                                }

                                if (lengthUnused == 0) {
                                    length -= 1; // for the .
                                }

                                var numbersToFit = new List<int>();
                                var isLastGroup = k == chars.Length - 1;

                                if (isLastGroup || length > currentUnsolvedNumber + 1) {
                                    numbersToFit.Add(currentUnsolvedNumber);
                                    var maxToFit = currentUnsolvedNumber + 2; // allow to "joining" .'s
                                    for (int l = index + 1; l < numberBrokenStrings.Length; l++) {
                                        if (isLastGroup || length > numberBrokenStrings[l] + maxToFit) {
                                            numbersToFit.Add(numberBrokenStrings[l]);
                                            maxToFit += numberBrokenStrings[l] + 1;
                                            continue;
                                        }

                                        break;
                                    }

                                    index += numbersToFit.Count;
                                    var totalSpaceToFit = length - (isLastGroup ? 0 : 1);
                                    if (totalSpaceToFit == numbersToFit.Sum() + numbersToFit.Count - 1){
                                        continue;
                                    }

                                    var spaceForLastNumber = totalSpaceToFit - (numbersToFit.Sum() + numbersToFit.Count - 1 - numbersToFit.Last());
                                    var dotsAfterLastNumber = spaceForLastNumber - numbersToFit.Last();
                                    combinations += Factorial(dotsAfterLastNumber) + dotsAfterLastNumber + 1;
                                    // I don't have a fricking clue
                                    //combinations += ((length - 1) - maxToFit) + 1;
                                    continue;
                                }
                            }
                        }

                        
                    }
                }

                Console.WriteLine($"Combinations found: {Math.Max(1, combinations)}");

                possibleSolutions += Math.Max(1, combinations);

            }

            return possibleSolutions.ToString();
        }

        static int Factorial(int n)
        {
           if (n >= 2) return n * Factorial(n - 1);
           return 1;
        } 

        private static void CheckNextBlock(char[] springs, int index,  out char nextSpring, out int numberOfSpringsInNextBlock)
        {
            nextSpring = springs[index];
            numberOfSpringsInNextBlock = 0;
            var count = 0;
            while (nextSpring == '#')
            {
                numberOfSpringsInNextBlock += 1;
                count += 1;
                nextSpring = springs[index + count];
            }
        }

        public string SolveB () {
            var possibleSolutions = 0;
            foreach (var line in Lines) {
                var numberBrokenStrings = line.Split(' ')[1].Split(',').Select(x => int.Parse(x)).ToArray();
                var springs = line.Split(' ')[0].ToCharArray();

                // if this is true possible solutions = 1
=======
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
                if (springs.Length == numberBrokenStrings.Sum() + numberBrokenStrings.Length -1){
                    possibleSolutions += 1;
                    continue;
                }

                var possibleArrangements = new List<List<char>> {
                    new()
                };
<<<<<<< HEAD
=======
                var trialOutput = new List<char>();
                var currentNumberBrokenStrings = numberBrokenStrings.First();
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
                
                for (int i = 0; i < springs.Length; i++) {
                    var currentSpring = springs[i];
                    if (currentSpring != '?') {
                        possibleArrangements.ForEach(x => x.Add(currentSpring));
                        continue;
                    }

                    if (i == 0) {
<<<<<<< HEAD
                        int numberToCheck = numberBrokenStrings.First();
                        CheckNextBlock(springs, i + 1, out char nextSpring, out int numberOfSpringsInNextBlock);

                        if (numberToCheck == numberOfSpringsInNextBlock) {
                            possibleArrangements[0].Add('.');
                        }
                        else if (nextSpring == '.') {
                            possibleArrangements[0].Add('#');
                        }
                        else {
                            possibleArrangements[0].Add('.');
                            possibleArrangements.Add(new List<char>() { '#' });
                        }

=======
                        possibleArrangements.Add(new List<char>(){'.'});
                        possibleArrangements.Add(new List<char>(){'#'});
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
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

<<<<<<< HEAD
                    var complete = false;
=======
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
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
<<<<<<< HEAD
                                if(currentFoundCompleteSpringsInArrangment == numberBrokenStrings.Count()){
                                    complete = true;
                                    break;
                                }
                                currentNumBrokenStrings = numberBrokenStrings[currentFoundCompleteSpringsInArrangment];
=======
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
                            }

                            currentSpringBlockSize = 0;
                        }

<<<<<<< HEAD
                        if (complete || currentSpringBlockSize > 0 && currentSpringBlockSize == currentNumBrokenStrings) { 
=======
                        if (currentSpringBlockSize == currentNumBrokenStrings) { 
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
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

<<<<<<< HEAD
                var numberOfRequiredSprings = numberBrokenStrings.Sum();
                Console.WriteLine($"Required: {numberOfRequiredSprings}, possible: {possibleArrangements.Count()}, {possibleArrangements.Where(x => x.Count(x => x == '#') == numberOfRequiredSprings).Count().ToString()}");
=======
                var distinctArrangements = possibleArrangements.Distinct();
                var numberOfRequiredSprings = numberBrokenStrings.Sum();
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
                foreach (var possibleArrangement in possibleArrangements){
                    if (possibleArrangement.Count(x => x == '#') == numberOfRequiredSprings) {
                        possibleSolutions += 1;        
                    }
                }
            }
            return possibleSolutions.ToString();
        }
<<<<<<< HEAD
=======


        public string SolveB () {
            return "B";
        }
>>>>>>> 470a75e7c03b97e7c44d06132d4294ede5e0b0d6
    }
}
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day1 : IDay
    {
        private string[] Lines { get; set; }
        public Day1() {
            Lines = File.ReadAllLines("./Day1.txt");
        }

        public string SolveA () {
            double total = 0;

            foreach (var line in Lines) {
                var number = char.GetNumericValue(line.SkipWhile(c=>!char.IsDigit(c)).First()) * 10;

                var number2 = char.GetNumericValue(line.Reverse().SkipWhile(c=>!char.IsDigit(c)).First());

                total += number + number2;
            }

            return total.ToString();
        }

        public string SolveB () {
            var dictionary = new Dictionary<string, int>() {
                {"one", 1},
                {"two", 2},
                {"three", 3},
                {"four", 4},
                {"five", 5},
                {"six", 6},
                {"seven", 7},
                {"eight", 8},
                {"nine", 9}
            };
            
            double total = 0;

            foreach (var line in Lines) {
                var chars = line.ToCharArray();
                double firstNumber = 0;
                double secondNumber = 0;

                for (int i = 0; i < chars.Length; i++) {
                    if (char.IsDigit(chars[i])) {
                        firstNumber = char.GetNumericValue(chars[i]) * 10;
                        break;
                    }

                    var foundNumber = false;
                    var reaminingChars = chars.Length - i;
                    var potentialNumbers = new string[] { line.Substring(i, Math.Min(3, reaminingChars)) , line.Substring(i, Math.Min(4, reaminingChars)), line.Substring(i, Math.Min(5, reaminingChars)) };
                    foreach (var potentialNumber in potentialNumbers) {
                        if (dictionary.Keys.Contains(potentialNumber)) {
                            firstNumber = dictionary[potentialNumber] * 10;
                            foundNumber = true;
                            break;
                        }
                    }
                    if (foundNumber) break;
                }

                for (int i = chars.Length - 1; i >= 0; i--) {
                    if (char.IsDigit(chars[i])){
                        secondNumber = char.GetNumericValue(chars[i]);
                        break;
                    }

                    var foundNumber = false;
                    var potentialNumbers = new string[] { line.Substring(Math.Max(i-2, 0), Math.Min(3, chars.Length)) , line.Substring(Math.Max(i-3, 0), Math.Min(4, chars.Length)), line.Substring(Math.Max(i-4, 0), Math.Min(5, chars.Length)) };
                    foreach (var potentialNumber in potentialNumbers){
                        if (dictionary.Keys.Contains(potentialNumber)){
                            secondNumber = dictionary[potentialNumber];
                            foundNumber = true;
                            break;
                        }
                    }
                    if (foundNumber)break;
                }

                total += firstNumber + secondNumber;
            }

            return total.ToString();
        }
    }
}
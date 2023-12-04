using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day4 : IDay
    {
        private string[] Lines { get; set; }
        public Day4() {
            Lines = File.ReadAllLines("./Day4.txt");
        }

        public string SolveA () {
                
            var total = 0;
            for (int i = 0; i < Lines.Length; i++) {
                var cardData = Lines[i].Split(':')[1].Split('|');
                var winningNumbers = Regex.Split(cardData[0], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
                var playersNumbers = Regex.Split(cardData[1], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
                var numberOfWinners = playersNumbers.Intersect(winningNumbers).Distinct().Count();
                var score = 0;
                for (int j = 0; j < numberOfWinners; j++)
                {
                    if (j == 0) {
                        score = 1;
                        continue;
                    }

                    score *= 2;
                }
                total += score;
            }
            
            return total.ToString();
        }

        public string SolveB () {
            var rows = Lines.Length;
            var columns = Lines.First().ToCharArray().Length;

            var matrix = new char[rows, columns];
            for (int i = 0; i < rows; i++) {
                var chars = Lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++) {
                    matrix[i, j] = chars[j];
                }
            }

            var total = 0;

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    var nextValue = matrix[i, j];

                    if (nextValue == '*') {
                        var power = IsGearNextToTwoNumber(matrix, i, j, columns, rows);
                        if (power.HasValue){
                            total += power.Value;
                        }
                    }
                }
            }

            return total.ToString();
        }

        private int? IsGearNextToTwoNumber(char[,] matrix, int row, int column, int columns, int rows) {
            var startColumn = Math.Max(column - 1, 0);
            var endColumn = Math.Min(column + 1, columns-1);
            var startRow = Math.Max(row - 1, 0);
            var endRow = Math.Min(row + 1, rows-1);

            var adjacentNumbersFound = 0;
            var previousCharOnRowWasDigit = false;
            var foundNumbers = new List<int>();
            for (int i = startRow; i <= endRow; i++) {
                previousCharOnRowWasDigit = false;
                for (int j = startColumn; j <= endColumn; j++)
                {
                    var symbol = matrix[i, j];
                    if (char.IsDigit(symbol)){
                        var currentCol = j;
                        int? startofNumber = null;
                        

                        if (!previousCharOnRowWasDigit){
                            adjacentNumbersFound += 1;
                            previousCharOnRowWasDigit = true;
                            if (adjacentNumbersFound > 2){
                                return null;
                            }

                            while (currentCol >= 0 && startofNumber == null){
                            if (char.IsDigit(matrix[i, currentCol])){
                                if (currentCol == 0){
                                    startofNumber = currentCol;
                                }
                                currentCol -= 1;                             
                            }
                            else{
                                startofNumber = currentCol + 1;
                            }
                        }
                        
                        currentCol = j;
                        int? endOfNumber = null;
                        while (currentCol < columns && endOfNumber == null){
                            if (char.IsDigit(matrix[i, currentCol])){
                                if (currentCol == columns - 1){
                                    endOfNumber = currentCol;
                                }
                                currentCol += 1;                             
                            }
                            else{
                                endOfNumber = currentCol - 1;
                            }
                        }


                        var number = "";
                        for (int k = startofNumber.Value; k <= endOfNumber; k++)
                        {
                            number += matrix[i, k];
                        }
                        foundNumbers.Add(int.Parse(number));
                        }
                        else {
                            continue;
                        }
                    }
                    else {
                        previousCharOnRowWasDigit = false;
                    }
                }
            }

            if (adjacentNumbersFound != 2) return null;
            return foundNumbers[0] * foundNumbers[1];
        }
    }
}
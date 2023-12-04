using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day3 : IDay
    {
        private string[] Lines { get; set; }
        public Day3() {
            Lines = File.ReadAllLines("./Day3.txt");
        }

        public string SolveA () {
            var rows = Lines.Length;
            var columns = Lines.First().ToCharArray().Length;

            var matrix = new char[rows, columns];
            for (int i = 0; i < rows; i++) {
                var chars = Lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++) {
                    matrix[i, j] = chars[j];
                }
            }

            var currentNumberToTest = "";
            int? numberToTestStart = null;
            int? numberToTestEnd = null;
            var total = 0;

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    var nextValue = matrix[i, j];
                    if (char.IsDigit(nextValue)) {
                        currentNumberToTest += nextValue;
                        if (!numberToTestStart.HasValue){
                            numberToTestStart = j;
                            continue;
                        }
                        if (j != columns - 1){
                            continue;
                        }
                    }
                    
                    if (string.IsNullOrWhiteSpace(currentNumberToTest)) {
                        continue;
                    }
                    else {
                        if (!numberToTestStart.HasValue) throw new NullReferenceException();
                        numberToTestEnd = j - 1;
                        if (IsNumberAdjcentToSymbol(matrix, numberToTestStart.Value, numberToTestEnd.Value, i, columns, rows)){
                            total += int.Parse(currentNumberToTest);
                        }
                        currentNumberToTest = null;
                        numberToTestStart = null;
                        numberToTestEnd = null;
                    }
                }
            }

            return total.ToString();
        }

        private bool IsNumberAdjcentToSymbol(char[,] matrix, int numberToTestStart, int numberToTestEnd, int rowNumber, int columns, int rows) {
            var startColumn = Math.Max(numberToTestStart - 1, 0);
            var endColumn = Math.Min(numberToTestEnd + 1, columns-1);
            var startRow = Math.Max(rowNumber - 1, 0);
            var endRow = Math.Min(rowNumber + 1, rows-1);

            for (int i = startRow; i <= endRow; i++) {
                for (int j = startColumn; j <= endColumn; j++)
                {
                    var symbol = matrix[i, j];
                    if (char.IsDigit(symbol) || symbol == '.'){
                        continue;
                    }
                    else{
                        return true;
                    }
                }
            }

            return false;
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
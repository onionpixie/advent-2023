

using System.Data;

namespace AdventOfCode
{
    public class Day14 : IDay
    {
        private string[] Lines { get; set; }

        public Day14() {
            Lines = File.ReadAllLines("./Day14.txt");
        }

        public string SolveA () {
            var rows = Lines.Length;
            var columns = Lines.First().Length;

            var columnsWithRocks = new Dictionary<int, int>();
            for (int i = 0; i < columns; i++) {
                columnsWithRocks.Add(i, 0);
            }

            var totalRockWeight = 0;
            var rolledMatrix = new char[rows, columns];
            for (int i = rows - 1; i >= 0 ; i--) {
                var chars = Lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++) {
                    if (i == 0) {
                        if (chars[j] == '.') {
                            if (columnsWithRocks[j] == 0) {
                                rolledMatrix[i, j] = chars[j];
                            }
                            else {
                                totalRockWeight += ResolveStoredRocks(columnsWithRocks, rolledMatrix, i - 1, j, rows);
                            }
                            continue;
                        }
                        else {
                            rolledMatrix[i, j] = chars[j];
                            if (chars[j] == 'O') {
                                totalRockWeight += rows;
                            }
                            totalRockWeight += ResolveStoredRocks(columnsWithRocks, rolledMatrix, i, j, rows);
                            continue;
                        }
                    }

                    if (chars[j] == 'O') {
                        // store rolled rock
                        columnsWithRocks[j] += 1;
                        // add empty space for now
                        rolledMatrix[i, j] = '.';
                        continue;
                    }

                    if (chars[j] == '#') {
                        totalRockWeight += ResolveStoredRocks(columnsWithRocks, rolledMatrix, i, j, rows);
                    }

                    rolledMatrix[i, j] = chars[j];
                }
            }
            
            for (int i = 0; i < rolledMatrix.GetLength(0); i++) {
                for (int j = 0; j < rolledMatrix.GetLength(1); j++) {
                    Console.Write(rolledMatrix[i,j]);
                }
                Console.WriteLine();
            }
            
            return totalRockWeight.ToString();
        }

        private static int ResolveStoredRocks(Dictionary<int, int> columnsWithRocks, char[,] rolledMatrix, int i, int j, int totalRows)
        {
            var weightOfPlacedRocks = 0;
            for (int k = 0; k < columnsWithRocks[j]; k++) {
                rolledMatrix[i + k + 1, j] = 'O';
                weightOfPlacedRocks += (totalRows - (i + k +1));
            }
            // then empty rolled rocks
            columnsWithRocks[j] = 0;

            return weightOfPlacedRocks;
        }

        public string SolveB () {
            return "B";
        }
    }
}
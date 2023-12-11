namespace AdventOfCode
{
    public class Day8 : IDay
    {
        private string[] Lines { get; set; }

        public Day8() {
            Lines = File.ReadAllLines("./Day8.txt");
        }

        public string SolveA () {
            var instructions = Lines[0].ToCharArray();
            var networks = new List<Network>();

            for (int i = 2; i < Lines.Length; i++) {
                var firstSplit = Lines[i].Split('=', StringSplitOptions.RemoveEmptyEntries);

                var network = new Network();
                network.Key = firstSplit[0].Trim();

                var secondHalf = firstSplit[1].Replace(" (", "").Trim();
                secondHalf = secondHalf.Replace(")", "");
                var directions = secondHalf.Split(',', StringSplitOptions.RemoveEmptyEntries);

                network.Left = directions[0];
                network.Right = directions[1].TrimStart();

                networks.Add(network);
            }

            var currentLocation = "AAA";
            var count = 0;
            while (currentLocation != "ZZZ") {
                foreach (var instruction in instructions) {
                    var currentNetwork = networks.Where(n => n.Key == currentLocation).Single();
                    if (instruction == 'L') {
                        currentLocation = currentNetwork.Left;
                    }
                    else {
                        currentLocation = currentNetwork.Right;
                    }

                    count += 1;
                }
            }

            return count.ToString();
        }

       

        public string SolveB () {
            var instructions = Lines[0].ToCharArray();
            var networks = new List<Network>();

            for (int i = 2; i < Lines.Length; i++) {
                var firstSplit = Lines[i].Split('=', StringSplitOptions.RemoveEmptyEntries);

                var network = new Network();
                network.Key = firstSplit[0].Trim();

                var lastKey = network.Key.ToCharArray().Last();
                if (lastKey == 'A') {
                    network.StartPoint = true;
                }
                else if (lastKey == 'Z') {
                    network.EndPoint = true;
                }

                var secondHalf = firstSplit[1].Replace(" (", "").Trim();
                secondHalf = secondHalf.Replace(")", "");
                var directions = secondHalf.Split(',', StringSplitOptions.RemoveEmptyEntries);

                network.Left = directions[0];
                network.Right = directions[1].TrimStart();

                networks.Add(network);
            }

            var startPoints = networks.Where(n => n.StartPoint);
            var listCycleCounts = new List<long>();
            foreach (var startPoint in startPoints) {
                var currentLocation = startPoint.Key;
                var endPoint = false;
                var count = 0;
                while (!endPoint) {
                    foreach (var instruction in instructions) {
                        var currentNetwork = networks.Where(n => n.Key == currentLocation).Single();
                        if (currentNetwork.EndPoint) {
                            endPoint = true;
                            listCycleCounts.Add(count);
                            break;
                        }
                        
                        if (instruction == 'L') {
                            currentLocation = currentNetwork.Left;
                        }
                        else {
                            currentLocation = currentNetwork.Right;
                        }

                        count += 1;
                    }
                }
            }

            return FindLowestCommonMultiple(listCycleCounts.ToArray()).ToString();
        }

        static long FindLowestCommonMultiple(long[] cycleNumbers) 
        { 
            // find biggest number
            long biggestNumber = cycleNumbers.Max();
            long result = 1; 
        
            // Find all factors that are present 
            // in two or more array elements. 
            int x = 2; // Current factor. 
            while (x <= biggestNumber) { 
                // Store indexes of all array elements that are divisible by x. 
                var indexes = new List<long>(); 
                for (int j = 0; j < cycleNumbers.Length; j++) { 
                    if (cycleNumbers[j] % x == 0) { 
                        indexes.Add(j); 
                    } 
                } 
        
                // If there are 2 or more array elements that are divisible by x. 
                if (indexes.Count >= 2) { 
                    // Reduce all array elements divisible by x. 
                    for (int j = 0; j < indexes.Count; j++) { 
                        cycleNumbers[indexes[j]] = cycleNumbers[indexes[j]] / x; 
                    } 

                    // multiple result by x
                    result *= x; 
                } 
                else { 
                    x++; 
                } 
            } 
        
            // Then multiply result by all reduced array elements 
            foreach (var cycleNumber in cycleNumbers) { 
                result *= cycleNumber; 
            } 
        
            return result; 
        } 
            
    }

    public class Network {
        public string Key { get; set; }

        public string Left { get; set; }

        public string Right { get; set; }

        public bool StartPoint { get; set; }

        public bool EndPoint { get; set; }
    }
}
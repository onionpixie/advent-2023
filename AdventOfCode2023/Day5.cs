using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day5 : IDay
    {
        private string[] Lines { get; set; }
        public Day5() {
            Lines = File.ReadAllLines("./Day5.txt");
        }

        public string SolveA ()
        {
            var seeds = Regex.Split(Lines[0], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => long.Parse(x)).ToList();

            var seedToSoilMaps = CreateMap("seed-to-soil map:", 0, out var indexOfEndOfMap);
            var soilToFertilizerMaps = CreateMap("soil-to-fertilizer map:", indexOfEndOfMap, out indexOfEndOfMap);
            var fertilizerToWaterMaps = CreateMap("fertilizer-to-water map:", indexOfEndOfMap, out indexOfEndOfMap);
            var waterToLightMaps = CreateMap("water-to-light map:", indexOfEndOfMap, out indexOfEndOfMap);
            var lightToTemperatureMaps = CreateMap("light-to-temperature map:", indexOfEndOfMap, out indexOfEndOfMap);
            var temperatureToHumidityMaps = CreateMap("temperature-to-humidity map:", indexOfEndOfMap, out indexOfEndOfMap);
            var humidityToLocationMaps = CreateMap("humidity-to-location map:", indexOfEndOfMap, out indexOfEndOfMap);

            long? nearestLocation = null;
            foreach (var seed in seeds)
            {
                var soil = Map(seedToSoilMaps, seed);
                var fertilizer = Map(soilToFertilizerMaps, soil);
                var water = Map(fertilizerToWaterMaps, fertilizer);
                var light = Map(waterToLightMaps, water);
                var temperature = Map(lightToTemperatureMaps, light);
                var humidity = Map(temperatureToHumidityMaps, temperature);
                var location = Map(humidityToLocationMaps, humidity);

                if (nearestLocation == null || location < nearestLocation) {
                    nearestLocation = location;
                }
            }

            return nearestLocation.Value.ToString();
        }

        private long Map (List<Map> maps, long currentValue){
            foreach (var map in maps){
                if (map.sourceRangeStart < currentValue && currentValue < map.sourceRangeEnd) {
                    return currentValue + map.valueToAdd;
                }
            }

            return currentValue;
        }

        private List<Map> CreateMap(string mapToFind, int startIndex, out int indexOfEndOfMap)
        {
            var mapFound = false;
            var maps = new List<Map>();
            indexOfEndOfMap = 0;

            for (int i = startIndex; i < Lines.Length; i++)
            {
                if (!mapFound && Lines[i].Contains(mapToFind)) {
                    mapFound = true;
                    
                    continue;
                }

                // we are in a soil map so crate dictionary lookup based on values
                if (mapFound && !string.IsNullOrWhiteSpace(Lines[i]))
                {
                    var input = Lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
                    var destinationRangeStart = input[0];
                    var sourceRangeStart = input[1];
                    var rangeLength = input[2];

                    maps.Add(new AdventOfCode.Map(sourceRangeStart, sourceRangeStart + rangeLength - 1, destinationRangeStart, destinationRangeStart = rangeLength -1, destinationRangeStart - sourceRangeStart));
                }

                // finished with soil mapping
                if (mapFound && string.IsNullOrWhiteSpace(Lines[i]))
                {
                    indexOfEndOfMap = i;
                    break;
                }
            }

            return maps;
        }

        public string SolveB () {
            var totalCards = Lines.Length;

            
            return totalCards.ToString();
        }

        private int FindNumberOfCards(List<int> losingCards, Dictionary<int, List<int>> winningCards, int gameId, int totalCardsMade)
        {
            if (losingCards.Contains(gameId)){
                return totalCardsMade += 1;
            }

            if (!winningCards.ContainsKey(gameId)) throw new Exception("Couldn't find the card!");

            var cardsCreated = winningCards[gameId];
            totalCardsMade += cardsCreated.Count;

            foreach (var card in cardsCreated){
                FindNumberOfCards(losingCards, winningCards, card, totalCardsMade);
            }

            return totalCardsMade;
        }
    }

    public class Map{
        public long sourceRangeStart { get; set; }
        public long sourceRangeEnd { get; set; }
        public long destinationRangeStart { get; set; }
        public long destinationRangeEnd { get; set; }
        public long valueToAdd { get; set; }

        public Map (long sourceRangeStart, long sourceRangeEnd, long destinationRangeStart, long destinationRangeEnd, long valueToAdd){
            this.sourceRangeStart = sourceRangeStart;
            this.sourceRangeEnd = sourceRangeEnd;
            this.destinationRangeStart = destinationRangeStart;
            this.valueToAdd = valueToAdd;
        }
    }
}
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day5 : IDay
    {
        private string[] Lines { get; set; }

        public Day5() {
            Lines = File.ReadAllLines("./Day5.txt");
        }

        public string SolveA () {
            var seeds = Regex.Split(Lines[0], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => long.Parse(x)).ToList();
            var seedToSoilMaps = CreateMap("seed-to-soil map:", 0, out var indexOfEndOfMap);
            var soilToFertilizerMaps = CreateMap("soil-to-fertilizer map:", indexOfEndOfMap, out indexOfEndOfMap);
            var fertilizerToWaterMaps = CreateMap("fertilizer-to-water map:", indexOfEndOfMap, out indexOfEndOfMap);
            var waterToLightMaps = CreateMap("water-to-light map:", indexOfEndOfMap, out indexOfEndOfMap);
            var lightToTemperatureMaps = CreateMap("light-to-temperature map:", indexOfEndOfMap, out indexOfEndOfMap);
            var temperatureToHumidityMaps = CreateMap("temperature-to-humidity map:", indexOfEndOfMap, out indexOfEndOfMap);
            var humidityToLocationMaps = CreateMap("humidity-to-location map:", indexOfEndOfMap, out indexOfEndOfMap);

            long? nearestLocation = null;
            foreach (var seed in seeds) {
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

            if (!nearestLocation.HasValue) return "Fail: Location not found";

            return nearestLocation.Value.ToString();
        }

        private static long Map (List<Map> maps, long currentValue) {
            var newValue = currentValue;
            foreach (var map in maps) {
                if (map.SourceRangeStart <= newValue && newValue <= map.SourceRangeEnd) {
                    return newValue += map.Difference;
                }
            }

            return newValue;
        }

        private List<Map> CreateMap(string mapToFind, int startIndex, out int indexOfEndOfMap)
        {
            var mapFound = false;
            var maps = new List<Map>();
            indexOfEndOfMap = 0;

            for (int i = startIndex; i < Lines.Length; i++) {
                if (!mapFound && Lines[i].Contains(mapToFind)) {
                    mapFound = true;
                    continue;
                }

                if (mapFound && !string.IsNullOrWhiteSpace(Lines[i])) {
                    var input = Lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
                    var destinationRangeStart = input[0];
                    var sourceRangeStart = input[1];
                    var rangeLength = input[2];

                    maps.Add(new AdventOfCode.Map(sourceRangeStart, sourceRangeStart + rangeLength - 1, destinationRangeStart, destinationRangeStart + rangeLength - 1));
                }

                // finished with mapping
                if (mapFound && string.IsNullOrWhiteSpace(Lines[i])) {
                    indexOfEndOfMap = i;
                    break;
                }
            }

            return maps;
        }

        public string SolveB () {
            var seeds = Regex.Split(Lines[0], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => long.Parse(x)).ToList();
            var seedRanges = new List<SeedRange>();
            for (int i = 0; i < seeds.Count; i+=2) {
                seedRanges.Add(new SeedRange{SeedStart = seeds[i], SeedEnd = seeds[i] + seeds[i+1]});
            }

            var seedToSoilMaps = CreateMap("seed-to-soil map:", 0, out var indexOfEndOfMap);
            var soilToFertilizerMaps = CreateMap("soil-to-fertilizer map:", indexOfEndOfMap, out indexOfEndOfMap);
            var fertilizerToWaterMaps = CreateMap("fertilizer-to-water map:", indexOfEndOfMap, out indexOfEndOfMap);
            var waterToLightMaps = CreateMap("water-to-light map:", indexOfEndOfMap, out indexOfEndOfMap);
            var lightToTemperatureMaps = CreateMap("light-to-temperature map:", indexOfEndOfMap, out indexOfEndOfMap);
            var temperatureToHumidityMaps = CreateMap("temperature-to-humidity map:", indexOfEndOfMap, out indexOfEndOfMap);
            var humidityToLocationMaps = CreateMap("humidity-to-location map:", indexOfEndOfMap, out indexOfEndOfMap);
            
            // assuming it'll find it before long.MaxValue / 32 is reached, otherwise may have to re-consider strategy
            for (long i = 0; i < long.MaxValue / 32; i++) {
                var humidty = ReverseMap(humidityToLocationMaps, i);
                var temperature = ReverseMap(temperatureToHumidityMaps, humidty);
                var light = ReverseMap(lightToTemperatureMaps, temperature);
                var water = ReverseMap(waterToLightMaps, light);
                var fertilizer = ReverseMap(fertilizerToWaterMaps, water);
                var soil = ReverseMap(soilToFertilizerMaps, fertilizer);
                var seed = ReverseMap(seedToSoilMaps, soil);
                if (HaveSeed(seed, seedRanges)) return i.ToString();
            }

            return "fail";
        }

        private static bool HaveSeed(long seed, List<SeedRange> seedRanges) {
            foreach (var map in seedRanges) {
                if (map.SeedStart <= seed && seed <= map.SeedEnd) {
                    return true;
                }
            }

            return false;
        }

        private static long ReverseMap(List<Map> maps, long currentValue) {
            var newValue = currentValue;
            foreach (var map in maps) {
                if (map.DestinationRangeStart <= newValue && newValue <= map.DestinationRangeEnd) {
                    return newValue += map.ReverseDifference;
                }
            }

            return newValue;
        }
    }

    public class Map {
        public long SourceRangeStart { get; set; }

        public long SourceRangeEnd { get; set; }

        public long DestinationRangeStart { get; set; }

        public long DestinationRangeEnd { get; set; }

        public Map (long sourceRangeStart, long sourceRangeEnd, long destinationRangeStart, long destinationRangeEnd){
            this.SourceRangeStart = sourceRangeStart;
            this.SourceRangeEnd = sourceRangeEnd;
            this.DestinationRangeStart = destinationRangeStart;
            this.DestinationRangeEnd = destinationRangeEnd;
        }

        public long Difference => DestinationRangeStart - SourceRangeStart;

        public long ReverseDifference => SourceRangeStart - DestinationRangeStart;
    }

    public class SeedRange {
        public long SeedStart { get; set; }

        public long SeedEnd { get; set; }
    }
}
using System.Data;

namespace AdventOfCode
{
    public class Day15 : IDay
    {
        private string[] Lines { get; set; }

        public Day15() {
            Lines = File.ReadAllLines("./Day15.txt");
        }

        public string SolveA () {
            var charsToHash = Lines[0].Split(',').Select(x => x.ToCharArray()).ToArray();
            var sum = 0;
            for (int i = 0; i < charsToHash.Length; i++) {
                var hash = 0;
                foreach (var symbol in charsToHash[i]) {
                    hash += symbol;
                    hash *= 17;
                    hash %= 256;
                }

                sum += hash;
                hash = 0;
            }

            return sum.ToString();
        }

        private const char addLensSymbol = '=';

        public string SolveB () {
            var boxes = new List<Box>();
            var charsToHash = Lines[0].Split(',').ToArray();
            
            for (int i = 0; i < charsToHash.Length; i++) {
                var label = charsToHash[i].Split('=')[0].Split('-')[0];
                var boxId = 0;
                foreach (var symbol in label) {
                    boxId += symbol;
                    boxId *= 17;
                    boxId %= 256;
                }

                var existingBox = boxes.SingleOrDefault(b => b.BoxId == boxId);
                if (existingBox == null){
                    var newBox = new Box { BoxId = boxId };
                    boxes.Add(newBox);
                    existingBox = newBox;
                }
                
                var isAddOperation = charsToHash[i].Contains(addLensSymbol);
                var newQueue = new Queue<Lens>();
                var foundLens = false;
                foreach (var lens in existingBox.Lenses) {
                    if (lens.Label == label) {
                        if (isAddOperation) {
                            var strength = charsToHash[i].Split(addLensSymbol)[1];
                            var newLens = new Lens { Label = label, Strength = int.Parse(strength)};
                            newQueue.Enqueue(newLens);
                            foundLens = true;
                            continue;
                        }
                        else {
                            // remove lens
                            continue;
                        }
                    }
                    newQueue.Enqueue(lens);
                }

                if (isAddOperation && !foundLens) {
                    var strength = charsToHash[i].Split(addLensSymbol)[1];
                    var newLens = new Lens { Label = label, Strength = int.Parse(strength)};
                    newQueue.Enqueue(newLens);
                }

                existingBox.Lenses = newQueue;
            }

            var focusingPower = 0;
            foreach (var box in boxes) {
                var power = 0;
                var lensSlotNumber = 1;
                while (box.Lenses.Count > 0) {
                    power += (box.BoxId + 1) * lensSlotNumber * box.Lenses.Dequeue().Strength; 
                    lensSlotNumber ++;
                }
                focusingPower += power;
            }

            return focusingPower.ToString();
        }

        public class Box {
            public int BoxId { get; set; }

            public Queue<Lens> Lenses { get; set; } = new Queue<Lens>();
        }

        public class Lens {
            public string Label { get; set; }

            public int Strength { get; set; }
        }
    }
}
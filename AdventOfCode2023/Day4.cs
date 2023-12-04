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
            var totalCards = Lines.Length;
            var losingCards = new List<int>();
            var winningCards = new Dictionary<int, List<int>>();
            var numberOfEachCardMade = new Dictionary<int, int>();
            for (int i = Lines.Length - 1; i >= 0; i--) {
                var gameId = Regex.Split(Lines[i].Split(':')[0], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).Single();
                numberOfEachCardMade.Add(gameId, 1);

                var cardData = Lines[i].Split(':')[1].Split('|');
                var winningNumbers = Regex.Split(cardData[0], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
                var playersNumbers = Regex.Split(cardData[1], @"\D+").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
                var numberOfWinners = playersNumbers.Intersect(winningNumbers).Distinct().Count();
                
                if (numberOfWinners == 0){
                    losingCards.Add(gameId);
                    continue;
                }

                var cardsCreated = new List<int>();
                
                for (int k = 1; k <= numberOfWinners; k++) {
                    numberOfEachCardMade = FindCardsCreated(numberOfEachCardMade, gameId + k, losingCards, winningCards);
                    cardsCreated.Add(gameId + k);
                }
                winningCards.Add(gameId, cardsCreated);
            }
            
            var totalCardsMade = 0;
            foreach (var entry in numberOfEachCardMade){
                totalCardsMade += entry.Value;
            }

            return totalCardsMade.ToString();
        }

        private Dictionary<int, int> FindCardsCreated(Dictionary<int, int> numberOfEachCardMade, int gameId, List<int> losingCards, Dictionary<int, List<int>> winningCards)
        {
            numberOfEachCardMade[gameId] += 1;

            if (losingCards.Contains(gameId)){
                return numberOfEachCardMade;
            }

            if (!winningCards.ContainsKey(gameId)) throw new Exception("Couldn't find the card!");

            var cardsCreated = winningCards[gameId];
            for (int k = 1; k <= cardsCreated.Count; k++) {
                numberOfEachCardMade = FindCardsCreated(numberOfEachCardMade, gameId + k, losingCards, winningCards);
            }

            return numberOfEachCardMade;
        }
    }
}
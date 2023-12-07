namespace AdventOfCode
{
    public class Day7 : IDay
    {
        private string[] Lines { get; set; }

        private readonly char[] cardValueRanking = new char[] {'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'};

        private readonly char[] cardValueRankingWithJoker = new char[] {'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'};

        public Day7() {
            Lines = File.ReadAllLines("./Day7.txt");
        }

        public string SolveA () {
            var rankedHandsDictionary = new Dictionary<int, List<KeyValuePair<char[], int>>> {
                { 1, new List<KeyValuePair<char[], int>>() }, // five of a kind
                { 2, new List<KeyValuePair<char[], int>>() }, // four of a kind
                { 3, new List<KeyValuePair<char[], int>>() }, // full house
                { 4, new List<KeyValuePair<char[], int>>() }, // three of a kind
                { 5, new List<KeyValuePair<char[], int>>() }, // two pair
                { 6, new List<KeyValuePair<char[], int>>() }, // one pair
                { 7, new List<KeyValuePair<char[], int>>() } // high card
            };


            foreach (var line in Lines) {
                var cardString = line.Split(' ')[0];
                var cards = cardString.ToCharArray();
                var value = int.Parse(line.Split(' ')[1]);
                var differentCards = cards.Distinct().ToArray();
                switch (differentCards.Count()) {
                    case 1:
                        // five of a kind
                        rankedHandsDictionary[1].Add(new KeyValuePair<char[], int>(cards, value));
                        break;
                    case 2:
                        // could be four or a kind or full house
                        var numberOfFirstCard = cards.Where(c => c == differentCards[0]).Count();
                        if (numberOfFirstCard == 1 || numberOfFirstCard == 4) {
                            rankedHandsDictionary[2].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        else {
                            rankedHandsDictionary[3].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        break;
                    case 3:
                        // could be three or a kind or two pair
                        numberOfFirstCard = cards.Where(c => c == differentCards[0]).Count();
                        var numberOfSecondCard = cards.Where(c => c == differentCards[1]).Count();
                        var numberOfLastCard = cards.Where(c => c == differentCards[2]).Count();
                        if (numberOfFirstCard == 3 || numberOfSecondCard == 3 || numberOfLastCard == 3) {
                            rankedHandsDictionary[4].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        else {
                            rankedHandsDictionary[5].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        break;
                    case 4:
                        // one pair
                        rankedHandsDictionary[6].Add(new KeyValuePair<char[], int>(cards, value));
                        break;
                    case 5:
                        // high card
                        rankedHandsDictionary[7].Add(new KeyValuePair<char[], int>(cards, value));
                        break;
                }
            }

            var currentRanking = Lines.Count();
            var totalWinnings = 0;
            foreach (var holding in rankedHandsDictionary) {
                if (holding.Value.Count == 0) continue;

                var handOrder = new SortedList<int, KeyValuePair<char[], int>>();
                currentRanking = SortHandsByRanking(holding.Value, 0, currentRanking, handOrder, cardValueRanking);
                
                foreach (var hand in handOrder.OrderByDescending(ho => ho.Key)) {
                    totalWinnings += hand.Key * hand.Value.Value;
                }
            }

            return totalWinnings.ToString();
        }

        private int SortHandsByRanking(List<KeyValuePair<char[], int>> holdings, int cardPosition, int currentRanking, SortedList<int, KeyValuePair<char[], int>> handOrder, char[] cardRanking)
        {
            foreach(var ranking in cardRanking) {
                var matchingCards = holdings.Where(c => c.Key[cardPosition] == ranking);
                if (matchingCards.Count() == 0) continue;

                if (matchingCards.Count() == 1) {
                    handOrder.Add(currentRanking, matchingCards.Single());
                    currentRanking--;
                    continue;
                }

                currentRanking = SortHandsByRanking(matchingCards.ToList(), cardPosition + 1, currentRanking, handOrder, cardRanking);
            }

            return currentRanking;
        }

        public string SolveB () {
            var rankedHandsDictionary = new Dictionary<int, List<KeyValuePair<char[], int>>> {
                { 1, new List<KeyValuePair<char[], int>>() }, // five of a kind
                { 2, new List<KeyValuePair<char[], int>>() }, // four of a kind
                { 3, new List<KeyValuePair<char[], int>>() }, // full house
                { 4, new List<KeyValuePair<char[], int>>() }, // three of a kind
                { 5, new List<KeyValuePair<char[], int>>() }, // two pair
                { 6, new List<KeyValuePair<char[], int>>() }, // one pair
                { 7, new List<KeyValuePair<char[], int>>() } // high card
            };


            foreach (var line in Lines) {
                var cardString = line.Split(' ')[0];
                var cards = cardString.ToCharArray();
                var value = int.Parse(line.Split(' ')[1]);
                var numberOfJokers = cards.Count(c => c == 'J');
                var differentCards = cards.Where(c => c != 'J').Distinct().ToArray();
                switch (differentCards.Count()) {
                    case 0:
                    case 1:
                        // five of a kind
                        rankedHandsDictionary[1].Add(new KeyValuePair<char[], int>(cards, value));
                        break;
                    case 2:
                        // could be four or a kind or full house or improved with joker
                        var numberOfFirstCard = cards.Where(c => c == differentCards[0]).Count();
                        var numberOfSecondCard = cards.Where(c => c == differentCards[1]).Count();
                        if (numberOfFirstCard == 1 || numberOfSecondCard == 1) {
                            rankedHandsDictionary[2].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        else {
                            rankedHandsDictionary[3].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        break;
                    case 3:
                        // could be three or a kind or two pair  or improved with joker
                        numberOfFirstCard = cards.Where(c => c == differentCards[0]).Count();
                        numberOfSecondCard = cards.Where(c => c == differentCards[1]).Count();
                        var numberOfLastCard = cards.Where(c => c == differentCards[2]).Count();
                        if (numberOfFirstCard == 3 || numberOfSecondCard == 3 || numberOfLastCard == 3) {
                            if (numberOfJokers != 0) throw new Exception ("JOKERS?!");
                            rankedHandsDictionary[4].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        else if  (numberOfFirstCard == 1 && numberOfSecondCard == 1 && numberOfLastCard == 1) {
                            if (numberOfJokers != 2) throw new Exception ("JOKERS?!");
                            rankedHandsDictionary[4].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        else if  (numberOfJokers == 1 && (numberOfFirstCard == 2 || numberOfSecondCard == 2 || numberOfLastCard == 2)) {
                            rankedHandsDictionary[4].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        else {
                            rankedHandsDictionary[5].Add(new KeyValuePair<char[], int>(cards, value));
                        }
                        break;
                    case 4:
                        // one pair
                        rankedHandsDictionary[6].Add(new KeyValuePair<char[], int>(cards, value));
                        break;
                    case 5:
                        // high card
                        rankedHandsDictionary[7].Add(new KeyValuePair<char[], int>(cards, value));
                        break;
                }
            }

            var currentRanking = Lines.Count();
            var totalWinnings = 0;
            foreach (var holding in rankedHandsDictionary) {
                if (holding.Value.Count == 0) continue;

                var handOrder = new SortedList<int, KeyValuePair<char[], int>>();
                currentRanking = SortHandsByRanking(holding.Value, 0, currentRanking, handOrder, cardValueRankingWithJoker);
                
                foreach (var hand in handOrder.OrderByDescending(ho => ho.Key)) {
                    totalWinnings += hand.Key * hand.Value.Value;
                }
            }

            return totalWinnings.ToString();
        }
    }
}
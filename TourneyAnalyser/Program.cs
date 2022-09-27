using Newtonsoft.Json.Linq;
using System.Text;

namespace TourneyAnalyser
{
    public class CardInfo
    {
        public CardInfo()
        {
            winRates = new List<double>();
        }

        private List<double> winRates;

        public void UpdateWinRate(double winRateInc, int games)
        {
            for (int i = 0; i < games; i++)
            {
                winRates.Add(winRateInc);
            }
        }

        public string name { get; set; }
        public int main1 { get; set; }
        public int main2 { get; set; }
        public int main3 { get; set; }
        public int side1 { get; set; }
        public int side2 { get; set; }
        public int side3 { get; set; }
        public int mainTotal { get { return main1 + (main2 * 2) + (main3 * 3); } }
        public int mainedBy { get { return main1 + main2 + main3; } }
        public double mainAverage { get { double average = (double)mainTotal / (double)mainedBy; return double.IsFinite(average) ? average : 0; } }
        public int sideTotal { get { return side1 + (side2 * 2) + (side3 * 3); } }
        public int sidedBy { get { return side1 + side2 + side3; } }
        public double sideAverage { get { double average = (double)sideTotal / (double)sidedBy; return double.IsFinite(average) ? average : 0; } }

        public int uniquePlayers { get { return winRates.Count; } }
        public double winRate { get { return winRates.Average(); } }

        public override string ToString()
        {
            return name + '¬' + main1 + '¬' + main2 + '¬' + main3 + '¬' + mainTotal + '¬' + mainedBy + '¬' + mainAverage + '¬' + side1 + '¬' + side2 + '¬' + side3 + '¬' + sideTotal + '¬' + sidedBy + '¬' + sideAverage + '¬' + winRate + "¬" + uniquePlayers + '¬' + (mainTotal + sideTotal) + '\n';
        }
    }

    internal class Program
    {
        private static readonly string _versionUrl = @"https://db.ygoprodeck.com/api/v7/checkDBVer.php";
        private static readonly string _dataUrl = @"https://db.ygoprodeck.com/api/v7/cardinfo.php";
        private static readonly string _versionDir = @"currentversion.php.json";
        private static readonly string _dataDir = @"cardinfo.php.json";
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();

            string json = string.Empty;

            // Use local if up to date
            if (File.Exists(_versionDir) && File.Exists(_dataDir))
                if (File.ReadAllText(_versionDir).Equals(client.GetStringAsync(_versionUrl).Result))
                {
                    Console.WriteLine("Using local database");
                    json = File.ReadAllText(_dataDir);
                }

            // Update Local
            if (json.Equals(string.Empty))
            {
                Console.WriteLine("Updating local database");
                json = client.GetStringAsync(_dataUrl).Result;
                File.WriteAllText(_dataDir, json);
                File.WriteAllText(_versionDir, client.GetStringAsync(_versionUrl).Result);
            }

            // Dictionary of Card Names by ID, resolves Alt Art ID issues
            var cardNames = new Dictionary<string, string>();
            foreach (JObject card in (JArray)JObject.Parse(json)["data"])
            {
                string name = card.SelectToken("name").ToString();
                foreach (JObject altArt in card.SelectToken("card_images"))
                    cardNames.Add(altArt.SelectToken("id").ToString(), name);
            }

            // Analyse card counts in decks
            Dictionary<string, double[]> winrates = new Dictionary<string, double[]>();
            foreach (var line in File.ReadLines("winrates.txt"))
            {
                var tuple = line.Split(",");
                if(!winrates.ContainsKey(tuple[0]))
                {
                    double[] dub = new double[2];
                    dub[0] = Double.Parse(tuple[1]);
                    dub[1] = int.Parse(tuple[2]);
                    winrates.Add(tuple[0], dub);
                }
            }
            Dictionary<string, CardInfo> analysis = new Dictionary<string, CardInfo>();
            foreach (var deck in Directory.EnumerateFiles("Decks", "*.ydk", SearchOption.AllDirectories))
            {
                Console.WriteLine("File: \"{0}\"", deck);
                // Read Deck
                bool siding = false;
                List<string> main = new List<string>();
                List<string> side = new List<string>();
                foreach (string line in File.ReadAllLines(deck))
                    if (line.StartsWith('!'))
                        siding = true;
                    else if (!line.StartsWith("#") && !line.Equals(string.Empty))
                    {
                        // Database sometimes out of date or just missing
                        string name = line;
                        try
                        {
                            name = cardNames[line];
                        }
                        catch (Exception e)
                        {
                        }

                        if (siding)
                            side.Add(name);
                        else
                            main.Add(name);
                    }


                // Main Deck Analysis
                foreach (string card in main.Distinct())
                {
                    // Add new card to analysis
                    if (!analysis.ContainsKey(card))
                    {
                        analysis.Add(card, new CardInfo());
                        analysis[card].name = card;
                    }


                    // Increment counts
                    switch (main.Count(x => x.Equals(card)))
                    {
                        case 1:
                            analysis[card].main1++;
                            break;
                        case 2:
                            analysis[card].main2++;
                            break;
                        case 3:
                            analysis[card].main3++;
                            break;
                    }
                }

                // Side Deck Analysis
                foreach (string card in side.Distinct())
                {
                    // Add new card to analysis
                    if (!analysis.ContainsKey(card))
                    {
                        analysis.Add(card, new CardInfo());
                        analysis[card].name = card;
                    }

                    // Increment counts
                    switch (side.Count(x => x.Equals(card)))
                    {
                        case 1:
                            analysis[card].side1++;
                            break;
                        case 2:
                            analysis[card].side2++;
                            break;
                        case 3:
                            analysis[card].side3++;
                            break;
                    }
                }



                double winRate = 0;
                double[] dub;
                int games = 0;
                if (winrates.TryGetValue(deck.Split('\\')[1], out dub))
                {
                    winRate = dub[0];
                    games = (int)dub[1];
                }
                else
                {
                    Console.WriteLine("No winrate found for: " + deck + " Manual option: \n" + "WR: ");
                    double.TryParse(Console.ReadLine(), out winRate);
                    Console.WriteLine("\nGames: ");
                    int.TryParse(Console.ReadLine(), out games);
                }
                List<string> cards = main.Concat(side).ToList();
                foreach (string card in cards.Distinct())
                {
                    analysis[card].UpdateWinRate(winRate, games);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Name¬1¬2¬3¬Total¬Players¬Avg¬1¬2¬3¬Total¬Players¬Avg¬WinRate¬Players¬Total\n");
            foreach (CardInfo card in analysis.Values)
            {
                sb.Append(card);
            }

            File.WriteAllText("analysis.txt", sb.ToString());
        }
    }
}
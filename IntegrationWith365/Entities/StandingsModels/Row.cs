using IntegrationWith365.Entities.GamesModels;

namespace IntegrationWith365.Entities.StandingsModels
{
    public class Row
    {
        public Competitor Competitor { get; set; }

        public int GamePlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public int GamesEven { get; set; }
        public int For { get; set; }
        public int Against { get; set; }
        public double Ratio { get; set; }
        public int Strike { get; set; }
        public int Position { get; set; }
    }
}

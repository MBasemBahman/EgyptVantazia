using IntegrationWith365.Entities.GamesModels;

namespace IntegrationWith365.Entities.GameModels
{
    public class GameReturn
    {
        public Game Game { get; set; }
    }

    public class Game
    {
        public string StatusText { get; set; }

        public bool IsEnded => StatusText is "انتهت" or "Ended";

        public Competitor HomeCompetitor { get; set; }

        public Competitor AwayCompetitor { get; set; }
    }
}

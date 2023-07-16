using IntegrationWith365.Entities.GamesModels;

namespace IntegrationWith365.Entities.GameModels
{
    public class GameReturn
    {
        public Game Game { get; set; }

        public string LastUpdateId { get; set; }
    }

    public class Game
    {
        public string StatusText { get; set; }

        public DateTime StartTime { get; set; }

        public bool IsEnded => StatusText is "انتهت" or "Ended";

        public List<GameMember> Members { get; set; }

        public Competitor HomeCompetitor { get; set; }

        public Competitor AwayCompetitor { get; set; }

        public List<Event> Events { get; set; }

        public List<Stage> Stages { get; set; }

        public bool HalfTimeEnded => Stages != null && Stages.Any(a => a.Id == 7 && a.IsEnded);
        
         public bool EndMatchEnded => Stages != null && Stages.Any(a => a.Id == 9 && a.IsEnded);
    }
}

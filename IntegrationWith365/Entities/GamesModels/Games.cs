namespace IntegrationWith365.Entities.GamesModels
{
    public class GamesReturn
    {
        public Paging Paging { get; set; }
        public IList<Games> Games { get; set; }
    }

    public class Games
    {
        public int Id { get; set; } // matchId
        public int SeasonNum { get; set; }
        public int RoundNum { get; set; } // gameWeek
        public string StartTime { get; set; }
        public DateTime StartTimeVal => DateTime.Parse(StartTime);
        public string StatusText { get; set; }
        public bool IsEnded => StatusText is "انتهت" or "Ended";
        public Competitor HomeCompetitor { get; set; }
        public Competitor AwayCompetitor { get; set; }
    }
}

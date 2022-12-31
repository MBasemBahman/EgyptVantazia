namespace IntegrationWith365.Entities.GamesModels
{
    public class Member
    {
        public int Id { get; set; } // PlayerId
        public double Ranking { get; set; }

        public int AthleteId { get; set; }

        public IList<Stat> Stats { get; set; }
    }
}

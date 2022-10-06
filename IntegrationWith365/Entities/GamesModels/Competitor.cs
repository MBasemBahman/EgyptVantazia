namespace IntegrationWith365.Entities.GamesModels
{
    public class Competitor // Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Score { get; set; }
        public Lineups Lineups { get; set; }
    }
}

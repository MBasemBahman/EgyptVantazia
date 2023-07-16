namespace IntegrationWith365.Entities.GameModels
{
    public class Stage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public double HomeCompetitorScore { get; set; }
        public double AwayCompetitorScore { get; set; }
        public bool IsEnded { get; set; }
        public bool? IsCurrent { get; set; }
    }
}

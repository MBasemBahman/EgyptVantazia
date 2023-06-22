namespace IntegrationWith365.Entities.GamesModels
{
    public class Statistics
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsMajor { get; set; }
        public string Value { get; set; }
        public double ValuePercentage { get; set; }
        public bool IsPrimary { get; set; }
    }
}

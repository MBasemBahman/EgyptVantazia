namespace IntegrationWith365.Entities.SquadsModels
{
    public class Athlete
    {
        public int Id { get; set; } // Player id
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Age { get; set; }
        public int JerseyNum { get; set; }
        public Position Position { get; set; }
    }
}

namespace IntegrationWith365.Entities.GameModels
{
    public class Event
    {
        public int CompetitorId { get; set; }

        public int PlayerId { get; set; }

        public double GameTime { get; set; }

        public EventType EventType { get; set; }

        public List<int> ExtraPlayers { get; set; }
    }
}

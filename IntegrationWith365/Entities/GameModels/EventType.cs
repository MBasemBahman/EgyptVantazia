namespace IntegrationWith365.Entities.GameModels
{
    public class EventType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubTypeId { get; set; }
        public string SubTypeName { get; set; }


        public int Value { get; set; }
        public double GameTime { get; set; }

    }
}

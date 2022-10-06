namespace IntegrationWith365.Entities.SquadsModels
{
    public class SquadReturn
    {
        public IList<Squad> Squads { get; set; }
    }
    public class Squad
    {
        public IList<Athlete> Athletes { get; set; }
    }
}

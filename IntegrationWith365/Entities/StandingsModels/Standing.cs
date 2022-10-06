
namespace IntegrationWith365.Entities.StandingsModels
{
    public class StandingsReturn
    {
        public IList<Standing> Standings { get; set; }
    }

    public class Standing
    {
        public IList<Row> Rows { get; set; }
    }
}

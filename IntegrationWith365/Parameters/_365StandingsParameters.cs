namespace IntegrationWith365.Parameters
{
    public class _365StandingsParameters : _365Parameters
    {
        public int SeasonNum { get; set; }

        public int StageNum { get; set; } = 1;

        public bool Live { get; set; } = false;
    }
}

using static Contracts.Extensions.StringExtensions;

namespace IntegrationWith365.Entities
{
    public class Paging
    {
        public string NextPage { get; set; }

        public string PreviousPage { get; set; }

        public int NextAfterGame => int.Parse(NextPage.Between("aftergame=", "&direction"));

        public int PreviousAfterGame => int.Parse(PreviousPage.Between("aftergame=", "&direction"));
    }
}

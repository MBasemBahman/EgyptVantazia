using static Contracts.Extensions.StringExtensions;

namespace IntegrationWith365.Entities
{
    public class Paging
    {
        public string NextPage { get; set; }

        public string PreviousPage { get; set; }

        public int NextAfterGame => NextPage.IsEmpty() ? 0 : int.Parse(NextPage.Between("aftergame=", "&direction"));

        public int PreviousAfterGame => PreviousPage.IsEmpty() ? 0 : int.Parse(PreviousPage.Between("aftergame=", "&direction"));
    }
}

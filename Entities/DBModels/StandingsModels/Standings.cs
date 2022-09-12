using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;

namespace Entities.DBModels.StandingsModels
{
    public class Standings : AuditEntity
    {
        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public Season Season { get; set; }

        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public Team Team { get; set; }

        [DisplayName(nameof(GamePlayed))]
        public int GamePlayed { get; set; }

        [DisplayName(nameof(GamesWon))]
        public int GamesWon { get; set; }

        [DisplayName(nameof(GamesLost))]
        public int GamesLost { get; set; }

        [DisplayName(nameof(GamesEven))]
        public int GamesEven { get; set; }

        [DisplayName(nameof(_365_For))]
        public int _365_For { get; set; }

        [DisplayName(nameof(Against))]
        public int Against { get; set; }

        [DisplayName(nameof(Ratio))]
        public int Ratio { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }
    }
}

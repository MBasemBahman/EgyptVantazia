using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.StandingsModels
{
    public class StandingsParameters : RequestParameters
    {
        public int Fk_Season { get; set; }

        public int Fk_Team { get; set; }

        public int _365_For { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }
        
        public string DashboardSearch { get; set; }
    }

    public class StandingsModel : AuditEntity
    {
        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public SeasonModel Season { get; set; }

        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public TeamModel Team { get; set; }

        [DisplayName(nameof(GamePlayed))]
        public int GamePlayed { get; set; }

        [DisplayName(nameof(GamesWon))]
        public int GamesWon { get; set; }

        [DisplayName(nameof(GamesLost))]
        public int GamesLost { get; set; }

        [DisplayName(nameof(GamesEven))]
        public int GamesEven { get; set; }

        [DisplayName(nameof(For))]
        public int For { get; set; }

        [DisplayName(nameof(Against))]
        public int Against { get; set; }

        [DisplayName(nameof(Ratio))]
        public double Ratio { get; set; }

        [DisplayName(nameof(Strike))]
        public int Strike { get; set; }

        [DisplayName(nameof(Position))]
        public int Position { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }
    }

    public class StandingsCreateOrEditModel
    {
        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(GamePlayed))]
        public int GamePlayed { get; set; }

        [DisplayName(nameof(GamesWon))]
        public int GamesWon { get; set; }

        [DisplayName(nameof(GamesLost))]
        public int GamesLost { get; set; }

        [DisplayName(nameof(GamesEven))]
        public int GamesEven { get; set; }

        [DisplayName(nameof(For))]
        public int For { get; set; }

        [DisplayName(nameof(Against))]
        public int Against { get; set; }

        [DisplayName(nameof(Ratio))]
        public double Ratio { get; set; }

        [DisplayName(nameof(Strike))]
        public int Strike { get; set; }

        [DisplayName(nameof(Position))]
        public int Position { get; set; }
    }
}

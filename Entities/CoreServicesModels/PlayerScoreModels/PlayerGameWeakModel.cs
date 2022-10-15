using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerScoreModels
{
    public class PlayerGameWeakParameters : RequestParameters
    {
        [DisplayName(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }
    }

    public class PlayerGameWeakModel : AuditEntity
    {
        [DisplayName(nameof(TeamGameWeak))]
        [ForeignKey(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public TeamGameWeakModel TeamGameWeak { get; set; }

        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public PlayerModel Player { get; set; }

        [DisplayName(nameof(Ranking))]
        public double Ranking { get; set; }
    }

    public class PlayerGameWeakCreateOrEditModel
    {
        public PlayerGameWeakCreateOrEditModel()
        {
            PlayerGameWeakScores = new List<PlayerGameWeakScoreCreateOrEditModel>();
        }
        [DisplayName(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Ranking))]
        public double Ranking { get; set; }

        [DisplayName(nameof(PlayerGameWeakScores))]
        public List<PlayerGameWeakScoreCreateOrEditModel> PlayerGameWeakScores { get; set; }
    }
}

using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerMarkModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerMarkModels
{
    public class PlayerMarkTeamGameWeakParameters : RequestParameters
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        [ForeignKey(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }
        
        public List<int> Fk_TeamGameWeaks { get; set; }
    }

    public class PlayerMarkTeamGameWeakModel : AuditEntity
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(PlayerMark))]
        public PlayerMarkModel PlayerMark { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        [ForeignKey(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public TeamGameWeakModel TeamGameWeak { get; set; }
    }

    public class PlayerMarkTeamGameWeakCreateOrEditModel
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        [ForeignKey(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }
    }
}

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
    public class PlayerMarkGameWeakScoreParameters : RequestParameters
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(PlayerGameWeakScore))]
        [ForeignKey(nameof(PlayerGameWeakScore))]
        public int Fk_PlayerGameWeakScore { get; set; }
    }

    public class PlayerMarkGameWeakScoreModel : AuditEntity
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(PlayerMark))]
        public PlayerMarkModel PlayerMark { get; set; }

        [DisplayName(nameof(PlayerGameWeakScore))]
        [ForeignKey(nameof(PlayerGameWeakScore))]
        public int Fk_PlayerGameWeakScore { get; set; }

        [DisplayName(nameof(PlayerGameWeakScore))]
        public PlayerGameWeakScoreModel PlayerGameWeakScore { get; set; }
    }

    public class PlayerMarkGameWeakScoreCreateOrEditModel
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(PlayerGameWeakScore))]
        [ForeignKey(nameof(PlayerGameWeakScore))]
        public int Fk_PlayerGameWeakScore { get; set; }
    }
}

using Entities.DBModels.SeasonModels;

namespace Entities.DBModels.PlayerMarkModels
{
    public class PlayerMarkReasonMatch : AuditEntity
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(PlayerMark))]
        public PlayerMark PlayerMark { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        [ForeignKey(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public TeamGameWeak TeamGameWeak { get; set; }
    }
}

using Entities.DBModels.SeasonModels;

namespace Entities.DBModels.PlayerMarkModels
{
    public class PlayerMarkGameWeak : AuditEntity
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(PlayerMark))]
        public PlayerMark PlayerMark { get; set; }

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeak GameWeak { get; set; }
    }
}

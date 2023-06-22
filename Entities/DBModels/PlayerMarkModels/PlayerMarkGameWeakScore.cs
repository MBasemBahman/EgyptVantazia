using Entities.DBModels.PlayerScoreModels;

namespace Entities.DBModels.PlayerMarkModels
{
    public class PlayerMarkGameWeakScore : AuditEntity
    {
        [DisplayName(nameof(PlayerMark))]
        [ForeignKey(nameof(PlayerMark))]
        public int Fk_PlayerMark { get; set; }

        [DisplayName(nameof(PlayerMark))]
        public PlayerMark PlayerMark { get; set; }

        [DisplayName(nameof(PlayerGameWeakScore))]
        [ForeignKey(nameof(PlayerGameWeakScore))]
        public int Fk_PlayerGameWeakScore { get; set; }

        [DisplayName(nameof(PlayerGameWeakScore))]
        public PlayerGameWeakScore PlayerGameWeakScore { get; set; }
    }
}

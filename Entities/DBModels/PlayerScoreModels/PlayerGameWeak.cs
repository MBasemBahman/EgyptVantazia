using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;

namespace Entities.DBModels.PlayerScoreModels
{
    public class PlayerGameWeak : AuditEntity
    {
        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeak GameWeak { get; set; }

        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public Player Player { get; set; }

        [DisplayName(nameof(Ranking))]
        public double Ranking { get; set; }

        [DisplayName(nameof(PlayerGameWeakScores))]
        public IList<PlayerGameWeakScore> PlayerGameWeakScores { get; set; }
    }
}

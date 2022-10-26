using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;

namespace Entities.DBModels.PlayerScoreModels
{
    public class PlayerGameWeak : AuditEntity
    {
        [DisplayName(nameof(TeamGameWeak))]
        [ForeignKey(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public TeamGameWeak TeamGameWeak { get; set; }

        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public Player Player { get; set; }

        [DisplayName(nameof(Ranking))]
        public double Ranking { get; set; }

        [DisplayName(nameof(TotalPoints))]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(PlayerGameWeakScores))]
        public IList<PlayerGameWeakScore> PlayerGameWeakScores { get; set; }
    }
}

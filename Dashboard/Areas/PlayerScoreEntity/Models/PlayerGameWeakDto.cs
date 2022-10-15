using Dashboard.Areas.SeasonEntity.Models;
using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.PlayerScoreModels;
using System.ComponentModel;

namespace Dashboard.Areas.PlayerScoreEntity.Models
{
    public class PlayerGameWeakFilter : DtParameters
    {
        public int Fk_GameWeak { get; set; }

        public int Fk_Player { get; set; }
    }
    public class PlayerGameWeakDto : PlayerGameWeakModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public new TeamGameWeakDto TeamGameWeak { get; set; }

        [DisplayName(nameof(Player))]
        public new PlayerDto Player { get; set; }

        [DisplayName(nameof(PlayerGameWeakScores))]
        public List<PlayerGameWeakScoreDto> PlayerGameWeakScores { get; set; }
    }
}

using Entities.CoreServicesModels.PlayerScoreModels;
using System.ComponentModel;

namespace Dashboard.Areas.PlayerScoreEntity.Models
{
    public class PlayerGameWeakScoreFilter : DtParameters
    {
        public int Fk_PlayerGameWeak { get; set; }

        public int Fk_GameWeak { get; set; }

        public int Fk_Player { get; set; }

        public int Fk_ScoreType { get; set; }
    }
    public class PlayerGameWeakScoreDto : PlayerGameWeakScoreModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(ScoreType))]
        public new ScoreTypeDto ScoreType { get; set; }
    }
}

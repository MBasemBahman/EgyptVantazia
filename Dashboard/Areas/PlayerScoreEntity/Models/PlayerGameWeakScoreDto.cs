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
        public int Fk_Season { get; set; }
        public bool? IsEnded { get; set; }
        public List<int> Fk_Players { get; set; }
        public List<int> Fk_Teams { get; set; }
        public double PointsFrom { get; set; }
        public double PointsTo { get; set; }
        [DefaultValue(0)]
        public double RateFrom { get; set; }
        [DefaultValue(10)]
        public double RateTo { get; set; }

        public List<int> Fk_ScoreTypes { get; set; }

        public string DashboardSearch { get; set; }
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

using Entities.CoreServicesModels.PlayerStateModels;
using System.ComponentModel;

namespace Dashboard.Areas.PlayerStateEntity.Models
{
    public class PlayerSeasonScoreStateFilter : DtParameters
    {
        public int Id { get; set; }
        public List<int> Fk_Players { get; set; }
        public List<int> Fk_ScoreStates { get; set; }
        public int Fk_Season { get; set; }
        public double? PointsFrom { get; set; }
        public double? PointsTo { get; set; }
        public double? PercentFrom { get; set; }
        public double? PercentTo { get; set; }
    }
    public class PlayerSeasonScoreStateDto : PlayerSeasonScoreStateModel
    {
        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }
}

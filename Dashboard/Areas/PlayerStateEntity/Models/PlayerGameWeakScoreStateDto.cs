using Entities.CoreServicesModels.PlayerStateModels;
using System.ComponentModel;

namespace Dashboard.Areas.PlayerStateEntity.Models
{
    public class PlayerGameWeakScoreStateFilter : DtParameters
    {
        public int Id { get; set; }
        public List<int> Fk_Players { get; set; }
        public List<int> Fk_ScoreStates { get; set; }
        public int? Fk_GameWeak { get; set; }
        public List<int> Fk_GameWeaks { get; set; }
        public double? PointsFrom { get; set; }
        public double? PointsTo { get; set; }
        public double? PercentFrom { get; set; }
        public double? PercentTo { get; set; }
        public double? ValueFrom { get; set; }
        public double? ValueTo { get; set; }
    }
    public class PlayerGameWeakScoreStateDto : PlayerGameWeakScoreStateModel
    {
        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }
}

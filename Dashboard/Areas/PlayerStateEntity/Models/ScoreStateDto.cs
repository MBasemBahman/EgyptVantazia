using Entities.CoreServicesModels.PlayerStateModels;
using System.ComponentModel;

namespace Dashboard.Areas.PlayerStateEntity.Models
{
    public class ScoreStateFilter : DtParameters
    {
        public int Id { get; set; }
    }
    public class ScoreStateDto : ScoreStateModel
    {
        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }
}

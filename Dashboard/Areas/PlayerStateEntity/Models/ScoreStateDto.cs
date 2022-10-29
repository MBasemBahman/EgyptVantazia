using Entities.CoreServicesModels.LocationModels;
using System.ComponentModel;
using Entities.CoreServicesModels.PlayerStateModels;

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

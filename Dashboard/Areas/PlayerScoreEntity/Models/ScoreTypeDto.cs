using Entities.CoreServicesModels.PlayerScoreModels;
using System.ComponentModel;

namespace Dashboard.Areas.PlayerScoreEntity.Models
{
    public class ScoreTypeFilter : DtParameters
    {

    }
    public class ScoreTypeDto : ScoreTypeModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}

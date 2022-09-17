using Entities.CoreServicesModels.PlayerScoreModels;
using System.ComponentModel;

namespace API.Areas.PlayerScoreArea.Models
{
    public class ScoreTypeDto : ScoreTypeModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_TypeId))]
        public new string _365_TypeId { get; set; }
    }
}

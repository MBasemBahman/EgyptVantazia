using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using System.ComponentModel;

namespace API.Areas.TeamArea.Models
{
    public class PlayerDto : PlayerModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_PlayerId))]
        public new int _365_PlayerId { get; set; }
    }
}

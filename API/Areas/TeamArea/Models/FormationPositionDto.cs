using Entities.CoreServicesModels.TeamModels;
using System.ComponentModel;

namespace API.Areas.TeamArea.Models
{
    public class FormationPositionDto : FormationPositionModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_PositionId))]
        public new int _365_PositionId { get; set; }
    }
}

using Entities.CoreServicesModels.TeamModels;
using Entities.RequestFeatures;
using System.ComponentModel;

namespace API.Areas.TeamArea.Models
{
    public class TeamDto : TeamModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_TeamId))]
        public new int _365_TeamId { get; set; }
    }
}

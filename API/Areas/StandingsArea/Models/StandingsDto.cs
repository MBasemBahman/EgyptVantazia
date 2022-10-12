using Entities.CoreServicesModels.StandingsModels;
using System.ComponentModel;

namespace API.Areas.StandingsArea.Models
{
    public class StandingsDto : StandingsModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_For))]
        public int _365_For { get; set; }
    }
}

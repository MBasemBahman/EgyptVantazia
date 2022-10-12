using Entities.CoreServicesModels.SeasonModels;
using System.ComponentModel;

namespace API.Areas.SeasonArea.Models
{
    public class TeamGameWeakDto : TeamGameWeakModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_MatchId))]
        public new string _365_MatchId { get; set; }

        [SwaggerIgnore]
        [DisplayName(nameof(_365_MatchUpId))]
        public string _365_MatchUpId { get; set; }
    }
}

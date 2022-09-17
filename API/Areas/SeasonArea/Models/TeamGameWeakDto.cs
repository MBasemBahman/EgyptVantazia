using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.Extensions;
using Entities.RequestFeatures;
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
        public new string _365_MatchUpId { get; set; }
    }
}

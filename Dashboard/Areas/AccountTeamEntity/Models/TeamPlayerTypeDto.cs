using Entities.CoreServicesModels.AccountTeamModels;
using System.ComponentModel;

namespace Dashboard.Areas.AccountTeamEntity.Models
{
    public class TeamPlayerTypeFilter : DtParameters
    {

    }
    public class TeamPlayerTypeDto : TeamPlayerTypeModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}

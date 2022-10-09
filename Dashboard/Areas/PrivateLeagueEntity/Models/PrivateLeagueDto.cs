using Dashboard.Areas.AccountEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using System.ComponentModel;

namespace Dashboard.Areas.PrivateLeagueEntity.Models
{
    public class PrivateLeagueFilter : DtParameters
    {

    }
    public class PrivateLeagueDto : PrivateLeagueModel
    {

        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}

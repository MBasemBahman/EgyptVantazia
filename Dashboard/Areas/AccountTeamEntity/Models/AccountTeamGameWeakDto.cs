using Entities.CoreServicesModels.AccountTeamModels;
using System.ComponentModel;
using Dashboard.Areas.SeasonEntity.Models;

namespace Dashboard.Areas.AccountTeamEntity.Models
{
    public class AccountTeamGameWeakDto : AccountTeamGameWeakModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public new AccountTeamDto AccountTeam { get; set; }

        [DisplayName(nameof(GameWeak))]
        public new GameWeakDto GameWeak { get; set; }
    }
}

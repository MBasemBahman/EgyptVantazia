using Dashboard.Areas.SeasonEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using System.ComponentModel;
namespace Dashboard.Areas.AccountTeamEntity.Models
{
    public class AccountTeamPlayerGameWeakDto : AccountTeamPlayerGameWeakModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(AccountTeamPlayer))]
        public new AccountTeamPlayerDto AccountTeamPlayer { get; set; }

        [DisplayName(nameof(TeamPlayerType))]
        public new TeamPlayerTypeDto TeamPlayerType { get; set; }

        [DisplayName(nameof(GameWeak))]
        public new GameWeakDto GameWeak { get; set; }

    }
}

using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using System.ComponentModel;
namespace Dashboard.Areas.AccountTeamEntity.Models
{
    public class AccountTeamPlayerDto : AccountTeamPlayerModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public new AccountTeamDto AccountTeam { get; set; }

        [DisplayName(nameof(Player))]
        public new PlayerDto Player { get; set; }

        [DisplayName(nameof(AccountTeamPlayerGameWeaks))]
        public List<AccountTeamPlayerGameWeakDto> AccountTeamPlayerGameWeaks { get; set; }

    }
}

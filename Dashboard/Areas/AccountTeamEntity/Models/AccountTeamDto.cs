using Dashboard.Areas.AccountEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Dashboard.Areas.SeasonEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;
namespace Dashboard.Areas.AccountTeamEntity.Models
{
    public class AccountTeamFilter : DtParameters
    {
        public int Id { get; set; }

        public int Fk_Account { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        [DisplayName(nameof(AccountFullName))]
        public string AccountFullName { get; set; }

        [DisplayName(nameof(AccountUserName))]
        public string AccountUserName { get; set; }

        public string DashboardSearch { get; set; }
    }
    public class AccountTeamDto : AccountTeamModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Account))]
        public new AccountDto Account { get; set; }

        [DisplayName(nameof(Season))]
        public new SeasonDto Season { get; set; }

        [DisplayName(nameof(AccountTeamGameWeaks))]
        public List<AccountTeamGameWeakDto> AccountTeamGameWeaks { get; set; }

        [DisplayName(nameof(AccountTeamPlayers))]
        public List<AccountTeamPlayerDto> AccountTeamPlayers { get; set; }

        [DisplayName(nameof(PlayerTransfers))]
        public List<PlayerTransferDto> PlayerTransfers { get; set; }
    }

    public enum AccountTeamReturnPageEnum
    {
        Index = 1,
        AccountProfile = 2
    }
}

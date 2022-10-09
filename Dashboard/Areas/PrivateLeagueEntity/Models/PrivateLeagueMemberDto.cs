using Dashboard.Areas.AccountEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using System.ComponentModel;

namespace Dashboard.Areas.PrivateLeagueEntity.Models
{
    public class PrivateLeagueMemberFilter : DtParameters
    {
        public int Fk_Account { get; set; }

        [DisplayName("PrivateLeague")]
        public int Fk_PrivateLeague { get; set; }
    }
    public class PrivateLeagueMemberDto : PrivateLeagueMemberModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Account))]
        public new AccountDto Account { get; set; }

        [DisplayName(nameof(PrivateLeague))]
        public new PrivateLeagueDto PrivateLeague { get; set; }
    }
}

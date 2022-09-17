using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PrivateLeagueModels
{
    public class PrivateLeagueMemberParameters : RequestParameters
    {
        public int Fk_Account { get; set; }

        public int Fk_PrivateLeague { get; set; }
    }

    public class PrivateLeagueMemberModel : AuditEntity
    {
        [DisplayName(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public AccountModel Account { get; set; }

        public int Fk_PrivateLeague { get; set; }

        [DisplayName(nameof(IsAdmin))]
        public bool IsAdmin { get; set; }
    }
}

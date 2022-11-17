using Entities.CoreServicesModels.AccountModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PrivateLeagueModels
{
    public class PrivateLeagueMemberParameters : RequestParameters
    {
        public int Fk_Account { get; set; }

        public int Fk_Season { get; set; }

        public int Fk_PrivateLeague { get; set; }

        public bool? IsAdmin { get; set; }
    }

    public class PrivateLeagueMemberModel : AuditEntity
    {
        [DisplayName(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public AccountModel Account { get; set; }

        [DisplayName(nameof(PrivateLeague))]
        public PrivateLeagueModel PrivateLeague { get; set; }

        public int Fk_PrivateLeague { get; set; }

        [DisplayName(nameof(IsAdmin))]
        public bool IsAdmin { get; set; }

        [DisplayName(nameof(Ranking))]
        public double Ranking { get; set; }
    }

    public class PrivateLeagueMemberCreateModel
    {
        public int Fk_PrivateLeague { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public IList<int> Fk_Accounts { get; set; }
    }
}

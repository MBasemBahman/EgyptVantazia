using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamParameters : RequestParameters
    {
        public int Fk_Account { get; set; }
        public int Fk_Season { get; set; }

        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        [DisplayName(nameof(AccountFullName))]
        public string AccountFullName { get; set; }

        [DisplayName(nameof(AccountUserName))]
        public string AccountUserName { get; set; }

    }

    public class AccountTeamModel : AuditImageEntity
    {
        [DisplayName(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public AccountModel Account { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public SeasonModel Season { get; set; }

        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(TotalPoints))]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(TotalMoney))]
        public int TotalMoney { get; set; }
    }

    public class AccountTeamCreateModel
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ImageFile))]
        public IFormFile ImageFile { get; set; }
    }
}

using Dashboard.Areas.Location.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.UserModels;
using System.ComponentModel;

namespace Dashboard.Areas.AccountEntity.Models
{
    public class AccountDto : AccountModel
    {
        [DisplayName(nameof(FullName))]
        public string FullName { get; set; }

        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Country))]
        public new CountryDto Country { get; set; }

        [DisplayName(nameof(Nationality))]
        public new CountryDto Nationality { get; set; }

        [DisplayName(nameof(LastActive))]
        public new string LastActive { get; set; }

    }

    public class AccountFilter : DtParameters
    {
    }

    public class UserAccountCreateOrEditModel
    {
        public UserAccountCreateOrEditModel()
        {
            Account = new();
            User = new();
        }
        public AccountCreateModel Account { get; set; }

        public UserCreateModel User { get; set; }
        public string ImageUrl { get; set; }
    }

    public enum AccountProfileItems
    {
        Details = 1
    }

}

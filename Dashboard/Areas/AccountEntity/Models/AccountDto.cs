using Dashboard.Areas.Location.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.UserModels;
using System.ComponentModel;

namespace Dashboard.Areas.AccountEntity.Models
{
    public class AccountDto : AccountModel
    {
        [DisplayName(nameof(FullName))]
        public new string FullName { get; set; }

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
        public int Fk_User { get; set; }

        public string UserName { get; set; }

        [DisplayName(nameof(Phone))]
        public string Phone { get; set; }

        [DisplayName("UserName")]
        public string AccountUserName { get; set; }

        [DisplayName("FullName")]
        public string AccountFullName { get; set; }

        [DisplayName(nameof(Email))]
        public string Email { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; } = null;

        [DisplayName(nameof(CreatedAtTo))]
        public DateTime? CreatedAtTo { get; set; } = null;

        [DisplayName("Country")]
        public int Fk_Country { get; set; }

        [DisplayName("Nationality")]
        public int Fk_Nationality { get; set; }


        [DisplayName("FavouriteTeam")]
        public int Fk_FavouriteTeam { get; set; }
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

        public List<AccountSubscriptionModel> Subscriptions { get; set; }
    }

    public enum AccountProfileItems
    {
        Details = 1,
        AccountTeam = 2,
        AccountSubscription = 3,
        Payment = 4
    }

}

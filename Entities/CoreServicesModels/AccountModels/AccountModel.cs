using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.LocationModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountModels
{
    public class AccountParameters : RequestParameters
    {
        public int Fk_Subscription { get; set; }
        public int Fk_User { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }
        public string DashboardSearch { get; set; }

        public string AccountUserName { get; set; }

        public string AccountFullName { get; set; }

        public string Email { get; set; }

        [DisplayName(nameof(CreatedAtFrom))]
        public DateTime? CreatedAtFrom { get; set; } = null;

        [DisplayName(nameof(CreatedAtTo))]
        public DateTime? CreatedAtTo { get; set; } = null;

        public List<int> Fk_Accounts { get; set; }

        public int Fk_Account_Ignored { get; set; }

        public bool? IsLoginBefore { get; set; } = null;

        public DateTime? LastActiveFrom { get; set; }

        public DateTime? LastActiveTo { get; set; }

        public int Fk_Country { get; set; }

        public int Fk_Nationality { get; set; }

        public int Fk_Season { get; set; }

        [DisplayName(nameof(ShowAds))]
        public bool? ShowAds { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }
    }

    public class AccountModel : AuditImageEntity
    {
        [DisplayName(nameof(FullName))]
        public string FullName { get; set; }

        [DisplayName(nameof(Name))]
        public string Name { get; set; }

        [DisplayName(nameof(UserName))]
        public string UserName { get; set; }

        [DisplayName(nameof(EmailAddress))]
        public string EmailAddress { get; set; }

        [DisplayName(nameof(PhoneNumber))]
        public string PhoneNumber { get; set; }

        [DisplayName(nameof(LastActive))]
        public DateTime? LastActive { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        [DisplayName(nameof(PhoneNumberTwo))]
        public string PhoneNumberTwo { get; set; }

        [DisplayName(nameof(Country))]
        [ForeignKey(nameof(Country))]
        public int Fk_Country { get; set; }

        [DisplayName(nameof(Country))]
        public CountryModel Country { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Nationality { get; set; }

        [DisplayName(nameof(Season))]
        public CountryModel Nationality { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public SeasonModel Season { get; set; }

        [DisplayName(nameof(Address))]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public int Fk_AccountTeam { get; set; }

        public AccountTeamModel AccountTeam { get; set; }

        [DisplayName(nameof(ShowAds))]
        public bool ShowAds { get; set; }
        
        [DisplayName(nameof(AccountSubscriptionsCount))]
        public int AccountSubscriptionsCount { get; set; }
        
        [DisplayName(nameof(GoldSubscriptionsCount))]
        public int GoldSubscriptionsCount { get; set; }
    }

    public class AccountEditModel
    {
        [DisplayName(nameof(FullName))]
        public string FullName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        [DisplayName(nameof(PhoneNumberTwo))]
        public string PhoneNumberTwo { get; set; }

        [DisplayName(nameof(Country))]
        public int Fk_Country { get; set; }

        [DisplayName(nameof(Fk_Nationality))]
        public int Fk_Nationality { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Address))]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
    }

    public class AccountCreateModel
    {
        [DisplayName(nameof(FullName))]
        public string FullName { get; set; }

        [DisplayName(nameof(Country))]
        public int Fk_Country { get; set; }

        [DisplayName("Nationality")]
        public int Fk_Nationality { get; set; }

        [DisplayName("Season")]
        public int Fk_Season { get; set; }


        [DisplayName(nameof(Address))]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName(nameof(PhoneNumberTwo))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumberTwo { get; set; }

        [DisplayName(nameof(ShowAds))]
        public bool ShowAds { get; set; }
    }
}

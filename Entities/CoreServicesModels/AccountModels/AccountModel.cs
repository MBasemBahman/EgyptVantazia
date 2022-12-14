using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.LocationModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountModels
{
    public class AccountParameters : RequestParameters
    {
        public int Fk_Subscription { get; set; }
        public int Fk_User { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

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

        public int Fk_FavouriteTeam { get; set; }

        public string RefCode { get; set; }


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

        [DisplayName(nameof(Nationality))]
        [ForeignKey(nameof(Nationality))]
        public int Fk_Nationality { get; set; }

        [DisplayName(nameof(Nationality))]
        public CountryModel Nationality { get; set; }

        [DisplayName(nameof(Address))]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        [ForeignKey(nameof(FavouriteTeam))]
        public int Fk_FavouriteTeam { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        public TeamModel FavouriteTeam { get; set; }

        [DisplayName(nameof(RefCode))]
        public string RefCode { get; set; }

        [DisplayName(nameof(RefCodeCount))]
        public int RefCodeCount { get; set; }

        public int Fk_AccountTeam { get; set; }

        public AccountTeamModel AccountTeam { get; set; }

        [DisplayName(nameof(AccountRefCodeCount))]
        public int AccountRefCodeCount { get; set; }
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

        [DisplayName(nameof(Address))]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName(nameof(Fk_FavouriteTeam))]
        public int Fk_FavouriteTeam { get; set; }
    }

    public class AccountCreateModel
    {
        [DisplayName(nameof(FullName))]
        public string FullName { get; set; }

        [DisplayName(nameof(Country))]
        public int Fk_Country { get; set; }

        [DisplayName("Nationality")]
        public int Fk_Nationality { get; set; }

        [DisplayName(nameof(Address))]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName("FavouriteTeam")]
        public int Fk_FavouriteTeam { get; set; }

        [DisplayName(nameof(RefCode))]
        public string RefCode { get; set; }

        [DisplayName(nameof(PhoneNumberTwo))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumberTwo { get; set; }

        [DisplayName(nameof(RefCodeCount))]
        public int RefCodeCount { get; set; }
    }
}

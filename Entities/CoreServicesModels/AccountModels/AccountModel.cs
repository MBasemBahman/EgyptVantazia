using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.LocationModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountModels
{
    public class AccountParameters : RequestParameters
    {
        public int Fk_User { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        public string AccountUserName { get; set; }

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
    }

    public class AccountModel : AuditImageEntity
    {
        [DisplayName(nameof(FirstName))]
        public string FirstName { get; set; }

        [DisplayName(nameof(LastName))]
        public string LastName { get; set; }

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
    }

    public class AccountEditModel
    {
        [DisplayName(nameof(FirstName))]
        public string FirstName { get; set; }

        [DisplayName(nameof(LastName))]
        public string LastName { get; set; }

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
        [DisplayName(nameof(FirstName))]
        public string FirstName { get; set; }

        [DisplayName(nameof(LastName))]
        public string LastName { get; set; }

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
}

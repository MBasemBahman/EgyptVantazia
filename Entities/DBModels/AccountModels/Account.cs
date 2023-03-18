using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.LocationModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.DBModels.TeamModels;
using Entities.DBModels.UserModels;

namespace Entities.DBModels.AccountModels
{
    [Index(nameof(RefCode), IsUnique = true)]
    public class Account : AuditImageEntity
    {
        [DisplayName(nameof(User))]
        [ForeignKey(nameof(User))]
        public int Fk_User { get; set; }

        [DisplayName(nameof(User))]
        public User User { get; set; }

        [DisplayName(nameof(FullName))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string FullName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        [DisplayName(nameof(PhoneNumberTwo))]
        public string PhoneNumberTwo { get; set; }

        [DisplayName(nameof(Country))]
        [ForeignKey(nameof(Country))]
        public int Fk_Country { get; set; }

        [DisplayName(nameof(Country))]
        public Country Country { get; set; }

        [DisplayName(nameof(Nationality))]
        [ForeignKey(nameof(Nationality))]
        public int Fk_Nationality { get; set; }

        [DisplayName(nameof(Nationality))]
        public Country Nationality { get; set; }

        [DisplayName(nameof(Address))]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        [ForeignKey(nameof(FavouriteTeam))]
        public int Fk_FavouriteTeam { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        public Team FavouriteTeam { get; set; }

        [DisplayName(nameof(RefCode))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string RefCode { get; set; }

        [DisplayName(nameof(RefCodeCount))]
        public int RefCodeCount { get; set; }

        [DisplayName(nameof(ShowAds))]
        public bool ShowAds { get; set; }

        [DisplayName(nameof(AccountTeams))]
        public IList<AccountTeam> AccountTeams { get; set; }

        [DisplayName(nameof(PrivateLeagueMembers))]
        public IList<PrivateLeagueMember> PrivateLeagueMembers { get; set; }

        [DisplayName(nameof(AccountSubscriptions))]
        public IList<AccountSubscription> AccountSubscriptions { get; set; }

        [DisplayName(nameof(Payments))]
        public IList<Payment> Payments { get; set; }

        [DisplayName(nameof(NewAccountRefCode))]
        public AccountRefCode NewAccountRefCode { get; set; }

        [DisplayName(nameof(RefAccountsRefCode))]
        public IList<AccountRefCode> RefAccountsRefCode { get; set; }
    }
}

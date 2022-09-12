using Entities.DBModels.AccountModels;
using Entities.DBModels.SeasonModels;

namespace Entities.DBModels.TeamModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class Team : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }

        [DisplayName(nameof(Players))]
        public IList<Player> Players { get; set; }

        [DisplayName(nameof(PlayerPrices))]
        public IList<PlayerPrice> PlayerPrices { get; set; }

        [DisplayName(nameof(HomeGameWeaks))]
        public IList<TeamGameWeak> HomeGameWeaks { get; set; }

        [DisplayName(nameof(AwayGameWeaks))]
        public IList<TeamGameWeak> AwayGameWeaks { get; set; }

        [DisplayName(nameof(AccountFavourites))]
        public IList<Account> AccountFavourites { get; set; }

        public TeamLang TeamLang { get; set; }
    }

    public class TeamLang : LangEntity<Team>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

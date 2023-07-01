using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayerStateModels;
using Entities.DBModels.StandingsModels;
using Entities.DBModels.TeamModels;

namespace Entities.DBModels.SeasonModels
{
    public class Season : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }

        [DisplayName(nameof(_365_CompetitionsId))]
        public string _365_CompetitionsId { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool IsCurrent { get; set; }

        [DisplayName(nameof(GameWeaks))]
        public IList<GameWeak> GameWeaks { get; set; }

        [DisplayName(nameof(AccountTeams))]
        public IList<AccountTeam> AccountTeams { get; set; }

        [DisplayName(nameof(Standings))]
        public IList<Standings> Standings { get; set; }

        [DisplayName(nameof(PlayerSeasonScoreStates))]
        public IList<PlayerSeasonScoreState> PlayerSeasonScoreStates { get; set; }

        [DisplayName(nameof(AccountSubscriptions))]
        public IList<AccountSubscription> AccountSubscriptions { get; set; }

        [DisplayName(nameof(Teams))]
        public List<Team> Teams { get; set; }

        public SeasonLang SeasonLang { get; set; }
    }

    public class SeasonLang : LangEntity<Season>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.StandingsModels;

namespace Entities.DBModels.SeasonModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class Season : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool IsCurrent { get; set; }

        [DisplayName(nameof(GameWeaks))]
        public IList<GameWeak> GameWeaks { get; set; }

        [DisplayName(nameof(AccountTeams))]
        public IList<AccountTeam> AccountTeams { get; set; }

        [DisplayName(nameof(Standings))]
        public IList<Standings> Standings { get; set; }

        public SeasonLang SeasonLang { get; set; }
    }

    public class SeasonLang : LangEntity<Season>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.NewsModels;
using Entities.DBModels.PlayerScoreModels;

namespace Entities.DBModels.SeasonModels
{
    public class GameWeak : AuditEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_GameWeakId))]
        public string _365_GameWeakId { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public Season Season { get; set; }

        [DisplayName(nameof(TeamGameWeaks))]
        public IList<TeamGameWeak> TeamGameWeaks { get; set; }

        [DisplayName(nameof(News))]
        public IList<News> News { get; set; }

        [DisplayName(nameof(PlayerGameWeaks))]
        public IList<PlayerGameWeak> PlayerGameWeaks { get; set; }

        [DisplayName(nameof(AccountTeamGameWeaks))]
        public IList<AccountTeamGameWeak> AccountTeamGameWeaks { get; set; }

        [DisplayName(nameof(AccountTeamPlayerGameWeaks))]
        public IList<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks { get; set; }

        public SeasonLang SeasonLang { get; set; }
    }

    public class GameWeakLang : LangEntity<GameWeak>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

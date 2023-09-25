using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.NewsModels;
using Entities.DBModels.PlayerMarkModels;
using Entities.DBModels.PlayerStateModels;
using Entities.DBModels.PlayersTransfersModels;

namespace Entities.DBModels.SeasonModels
{
    public class GameWeak : AuditEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool IsCurrent { get; set; }

        [DisplayName(nameof(IsNext))]
        public bool IsNext { get; set; }

        [DisplayName(nameof(IsPrev))]
        public bool IsPrev { get; set; }

        [DisplayName(nameof(_365_GameWeakId))]
        public string _365_GameWeakId { get; set; }

        [DisplayName(nameof(_365_GameWeakIdValue))]
        [NotMapped]
        public int _365_GameWeakIdValue { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public Season Season { get; set; }

        [DisplayName(nameof(Deadline))]
        public DateTime? Deadline { get; set; }

        [DisplayName(nameof(EndTime))]
        public DateTime? EndTime { get; set; }

        [DisplayName(nameof(JobId))]
        public string JobId { get; set; }

        [DisplayName(nameof(SecondJobId))]
        public string SecondJobId { get; set; }

        [DisplayName(nameof(EndTimeJobId))]
        public string EndTimeJobId { get; set; }

        [DisplayName(nameof(TeamGameWeaks))]
        public IList<TeamGameWeak> TeamGameWeaks { get; set; }

        [DisplayName(nameof(News))]
        public IList<News> News { get; set; }

        [DisplayName(nameof(AccountTeamGameWeaks))]
        public IList<AccountTeamGameWeak> AccountTeamGameWeaks { get; set; }

        [DisplayName(nameof(AccountTeamPlayerGameWeaks))]
        public IList<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks { get; set; }

        [DisplayName(nameof(PlayerTransfers))]
        public IList<PlayerTransfer> PlayerTransfers { get; set; }

        [DisplayName(nameof(PlayerGameWeakScoreStates))]
        public IList<PlayerGameWeakScoreState> PlayerGameWeakScoreStates { get; set; }

        [DisplayName(nameof(PlayerMarkGameWeaks))]
        public List<PlayerMarkGameWeak> PlayerMarkGameWeaks { get; set; }

        public GameWeakLang GameWeakLang { get; set; }
    }

    public class GameWeakLang : LangEntity<GameWeak>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

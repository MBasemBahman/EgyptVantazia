using Entities.DBModels.TeamModels;

namespace Entities.DBModels.AccountTeamModels
{
    public class AccountTeamPlayer : BaseEntity
    {
        [DisplayName(nameof(AccountTeam))]
        [ForeignKey(nameof(AccountTeam))]
        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public AccountTeam AccountTeam { get; set; }

        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public Player Player { get; set; }

        [DisplayName(nameof(AccountTeamPlayerGameWeaks))]
        public IList<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks { get; set; }
    }
}

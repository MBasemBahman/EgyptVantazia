using Entities.CoreServicesModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamPlayerParameters : RequestParameters
    {
        public int Fk_Account { get; set; }
        public int Fk_AccountTeam { get; set; }
        public int Fk_Player { get; set; }
        public int Fk_Season { get; set; }
        public int Fk_GameWeak { get; set; }
        public int Fk_SeasonForScore { get; set; }
        public int Fk_GameWeakForScore { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? IsNextGameWeak { get; set; }
        public bool IncludeNextMatch { get; set; }
        public bool IncludeScore { get; set; }
        public List<int> Fk_ScoreStatesForSeason { get; set; }

        public List<int> Fk_ScoreStatesForGameWeak { get; set; }

        public bool? IsTransfer { get; set; }

        public DateTime? FromDeadLine { get; set; }

        public DateTime? ToDeadLine { get; set; }
    }

    public class AccountTeamPlayerModel : BaseEntity
    {
        [DisplayName(nameof(AccountTeam))]
        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public AccountTeamModel AccountTeam { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public PlayerModel Player { get; set; }

        public AccountTeamPlayerGameWeakModel AccountTeamPlayerGameWeak { get; set; }
    }

    public class AccountTeamPlayerBulkCreateModel
    {
        public List<AccountTeamPlayerCreateModel> Players { get; set; }
    }

    public class AccountTeamPlayerBulkUpdateModel
    {
        public List<AccountTeamPlayerUpdateModel> Players { get; set; }
    }

    public class AccountTeamPlayerCreateModel
    {
        public bool IsPrimary { get; set; }

        public int Fk_TeamPlayerType { get; set; }

        public int Fk_PlayerPosition { get; set; }

        public int Fk_Player { get; set; }

        public int Order { get; set; }
    }

    public class AccountTeamPlayerUpdateModel
    {
        public bool IsPrimary { get; set; }

        public int Fk_TeamPlayerType { get; set; }

        public int Fk_PlayerPosition { get; set; }

        public int Fk_AccountTeamPlayer { get; set; }

        public int Order { get; set; }
    }

    public class AccountTeamCheckStructureModel
    {
        public bool IsPrimary { get; set; }

        public int Fk_TeamPlayerType { get; set; }

        public int Fk_PlayerPosition { get; set; }
    }
}

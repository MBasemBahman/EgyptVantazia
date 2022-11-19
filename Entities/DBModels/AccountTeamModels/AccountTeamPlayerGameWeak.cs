using Entities.DBModels.SeasonModels;

namespace Entities.DBModels.AccountTeamModels
{
    public class AccountTeamPlayerGameWeak : AuditEntity
    {
        [DisplayName(nameof(AccountTeamPlayer))]
        [ForeignKey(nameof(AccountTeamPlayer))]
        public int Fk_AccountTeamPlayer { get; set; }

        [DisplayName(nameof(AccountTeamPlayer))]
        public AccountTeamPlayer AccountTeamPlayer { get; set; }

        [DisplayName(nameof(TeamPlayerType))]
        [ForeignKey(nameof(TeamPlayerType))]
        public int Fk_TeamPlayerType { get; set; }

        [DisplayName(nameof(TeamPlayerType))]
        public TeamPlayerType TeamPlayerType { get; set; } // captian or vice captian

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeak GameWeak { get; set; }

        [DisplayName(nameof(IsPrimary))]
        public bool IsPrimary { get; set; }

        [DisplayName(nameof(IsTransfer))]
        public bool IsTransfer { get; set; }

        [DisplayName(nameof(Order))]
        public int Order { get; set; }

        [DisplayName(nameof(TrippleCaptain))]
        public bool TrippleCaptain { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }

        [DisplayName(nameof(HavePoints))]
        public bool HavePoints { get; set; }

        [DisplayName(nameof(HavePointsInTotal))]
        public bool HavePointsInTotal { get; set; }
    }
}

using Entities.CoreServicesModels.SeasonModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamPlayerGameWeakParameters : RequestParameters
    {
        public int Fk_AccountTeamPlayer { get; set; }

        public int Fk_TeamPlayerType { get; set; }

        public int Fk_AccountTeam { get; set; }

        public int Fk_Player { get; set; }

        public int Fk_Account { get; set; }

        public int Fk_Season { get; set; }

        public int Fk_GameWeak { get; set; }

        public bool? IsTransfer { get; set; }

        public bool? IsPrimary { get; set; }

        public int GameWeakFrom { get; set; }
        public int GameWeakTo { get; set; }
    }
    
    public class AccountTeamPlayerGameWeakModel : AuditEntity
    {

        public string PlayerName { get; set; }

        [DisplayName(nameof(AccountTeamPlayer))]
        public int Fk_AccountTeamPlayer { get; set; }

        [DisplayName(nameof(AccountTeamPlayer))]
        public AccountTeamPlayerModel AccountTeamPlayer { get; set; }

        [DisplayName(nameof(TeamPlayerType))]
        public int Fk_TeamPlayerType { get; set; }

        [DisplayName(nameof(TeamPlayerType))]
        public TeamPlayerTypeModel TeamPlayerType { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeakModel GameWeak { get; set; }

        [DisplayName(nameof(IsPrimary))]
        public bool IsPrimary { get; set; }

        [DisplayName(nameof(IsTransfer))]
        public bool IsTransfer { get; set; }

        [DisplayName(nameof(Order))]
        public int Order { get; set; }

        [DisplayName(nameof(Points))]
        public int? Points { get; set; }

        [DisplayName(nameof(HavePoints))]
        public bool HavePoints { get; set; }

        [DisplayName(nameof(HavePointsInTotal))]
        public bool HavePointsInTotal { get; set; }

        public bool IsPlayed { get; set; }

        public bool IsDelayed { get; set; }

        public bool NotHaveMatch { get; set; }

        public bool IsParticipate { get; set; }

        public int? Top15 { get; set; }
    }

    public class AccountTeamPlayerGameWeakModelForCalc
    {
        public int Fk_AccountTeamPlayer { get; set; }
        public int Fk_TeamPlayerType { get; set; }
        public bool IsPrimary { get; set; }
        public int Order { get; set; }
        public int? Points { get; set; }
        public string PlayerName { get; set; }
    }
}

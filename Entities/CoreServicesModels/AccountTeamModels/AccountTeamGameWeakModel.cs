using Entities.CoreServicesModels.SeasonModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamGameWeakParameters : RequestParameters
    {
        public int Fk_AccountTeam { get; set; }

        public int Fk_GameWeak { get; set; }
    }

    public class AccountTeamGameWeakModel : AuditEntity
    {
        [DisplayName(nameof(AccountTeam))]
        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public AccountTeamModel AccountTeam { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeakModel GameWeak { get; set; }

        [DisplayName(nameof(BenchBoost))]
        public bool BenchBoost { get; set; }

        [DisplayName(nameof(FreeHit))]
        public bool FreeHit { get; set; }

        [DisplayName(nameof(WildCard))]
        public bool WildCard { get; set; }

        [DisplayName(nameof(TotalPoints))]
        public int TotalPoints { get; set; }
    }
}

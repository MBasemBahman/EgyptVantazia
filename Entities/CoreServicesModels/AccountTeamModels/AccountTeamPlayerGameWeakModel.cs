using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamPlayerGameWeakParameters : RequestParameters
    {
        public int Fk_AccountTeamPlayer { get; set; }

        public int Fk_TeamPlayerType { get; set; }
    }
    public class AccountTeamPlayerGameWeakModel : AuditEntity
    {
        public int Fk_AccountTeamPlayer { get; set; }

        public int Fk_TeamPlayerType { get; set; }

        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(TrippleCaptain))]
        public bool TrippleCaptain { get; set; }
    }
}

using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamPlayerParameters : RequestParameters
    {
        public int Fk_AccountTeam { get; set; }
        public int Fk_Player { get; set; }

    }
    public class AccountTeamPlayerModel : BaseEntity
    {
        public int Fk_AccountTeam { get; set; }
        public int Fk_Player { get; set; }

    }
}

using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerScoreModels
{
    public class PlayerGameWeakParameters : RequestParameters
    {
        public int Fk_GameWeak { get; set; }
        public int Fk_Player { get; set; }

    }
    public class PlayerGameWeakModel : AuditEntity
    {
        public int Fk_GameWeak { get; set; }

        public int Fk_Player { get; set; }

    }
}

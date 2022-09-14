using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.TeamModels
{
    public class PlayerPriceParameters : RequestParameters
    {
        public int Fk_Team { get; set; }

        public int Fk_Player { get; set; }
    }
    public class PlayerPriceModel : AuditEntity
    {
        public int Fk_Team { get; set; }

        public int Fk_Player { get; set; }

        [DisplayName(nameof(BuyPrice))]
        public int BuyPrice { get; set; }

        [DisplayName(nameof(SellPrice))]
        public int SellPrice { get; set; }
    }
}

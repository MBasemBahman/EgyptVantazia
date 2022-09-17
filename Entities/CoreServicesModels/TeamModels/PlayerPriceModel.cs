using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.TeamModels
{
    public class PlayerPriceParameters : RequestParameters
    {
        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }
    }

    public class PlayerPriceModel : AuditEntity
    {
        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public TeamModel Team { get; set; }

        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public PlayerModel Player { get; set; }

        [DisplayName(nameof(BuyPrice))]
        public int BuyPrice { get; set; }

        [DisplayName(nameof(SellPrice))]
        public int SellPrice { get; set; }
    }
}

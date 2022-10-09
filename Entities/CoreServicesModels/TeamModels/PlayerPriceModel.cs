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
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public TeamModel Team { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public PlayerModel Player { get; set; }

        [DisplayName(nameof(BuyPrice))]
        public double BuyPrice { get; set; }

        [DisplayName(nameof(SellPrice))]
        public double SellPrice { get; set; }
    }

    public class PlayerPriceCreateOrEditModel
    {
        [DisplayName(nameof(BuyPrice))]
        public double BuyPrice { get; set; }

        [DisplayName(nameof(SellPrice))]
        public double SellPrice { get; set; }

        public int Id { get; set; }

        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }
    }
}

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
    
    public class PlayerPriceEditModel
    {
        [DisplayName(nameof(BuyPrice))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [Range(0, double.MaxValue)]
        public double BuyPrice { get; set; }

        [DisplayName(nameof(SellPrice))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [Range(0, double.MaxValue)]
        public double SellPrice { get; set; }

        public int Id { get; set; }

        [DisplayName(nameof(Player))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public int Fk_Player { get; set; }
        
        [DisplayName(nameof(Team))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public int Fk_Team { get; set; }
    }
}

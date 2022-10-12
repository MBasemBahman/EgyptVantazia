using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.TeamModels
{
    public class PlayerParameters : RequestParameters
    {
        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }
    }
    public class PlayerModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ShortName))]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }

        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public TeamModel Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public PlayerPositionModel PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerNumber))]
        public string PlayerNumber { get; set; }

        [DisplayName(nameof(Age))]
        public int Age { get; set; }
        [DisplayName(nameof(BuyPrice))]
        public double BuyPrice { get; set; }

        [DisplayName(nameof(SellPrice))]
        public double SellPrice { get; set; }
    }

    public class PlayerCreateOrEditModel
    {
        public PlayerCreateOrEditModel()
        {
            PlayerPrices = new List<PlayerPriceCreateOrEditModel>();
        }
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.ArLang}")]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }

        [DisplayName(nameof(Age))]
        public int Age { get; set; }

        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        [ForeignKey(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerNumber))]
        public string PlayerNumber { get; set; }


        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }

        [DisplayName(nameof(PlayerPrices))]
        public List<PlayerPriceCreateOrEditModel> PlayerPrices { get; set; }

        public PlayerLangModel PlayerLang { get; set; }
    }

    public class PlayerLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.EnLang}")]
        public string ShortName { get; set; }
    }
}

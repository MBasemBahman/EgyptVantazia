using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.TeamModels
{
    public class FormationPositionParameters : RequestParameters
    {
        [DisplayName(nameof(_365_PositionId))]
        public string _365_PositionId { get; set; }
    }

    public class FormationPositionModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ShortName))]
        public string ShortName { get; set; }

        [DisplayName(nameof(PlayersCount))]
        public int PlayersCount { get; set; }

        [DisplayName(nameof(_365_PositionId))]
        public string _365_PositionId { get; set; }
    }

    public class FormationPositionCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.ArLang}")]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_PositionId))]
        public string _365_PositionId { get; set; }

        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }

        public FormationPositionLangModel FormationPositionLang { get; set; }
    }

    public class FormationPositionLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.EnLang}")]
        public string ShortName { get; set; }
    }
}

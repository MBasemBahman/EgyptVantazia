using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.SeasonModels
{
    public class SeasonParameters : RequestParameters
    {
        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool? IsCurrent { get; set; }
    }

    public class SeasonModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool IsCurrent { get; set; }
    }

    public class SeasonCreateOrEditModel
    {
        public SeasonCreateOrEditModel()
        {
            GameWeaks = new List<GameWeakCreateOrEditModel>();
        }
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool IsCurrent { get; set; }

        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }

        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }

        [DisplayName(nameof(GameWeaks))]
        public List<GameWeakCreateOrEditModel> GameWeaks { get; set; }
        public SeasonLangModel SeasonLang { get; set; }
    }

    public class SeasonLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

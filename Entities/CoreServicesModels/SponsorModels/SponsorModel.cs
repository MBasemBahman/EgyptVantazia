using Entities.DBModels.SponsorModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.SponsorModels
{
    public class SponsorParameters : RequestParameters
    {
        public AppViewEnum? AppViewEnum { get; set; }

        public bool GetViews { get; set; } = false;
    }

    public class SponsorModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(LinkUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string LinkUrl { get; set; }

        [DisplayName(nameof(SponsorViewsCount))]
        public int SponsorViewsCount { get; set; }

        [DisplayName(nameof(SponsorViews))]
        public IList<AppViewEnum> SponsorViews { get; set; }
    }

    public class SponsorCreateOrEditModel
    {
        public SponsorCreateOrEditModel()
        {
            SponsorViews = new List<AppViewEnum>();
        }
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(LinkUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string LinkUrl { get; set; }

        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }

        public SponsorLangModel SponsorLang { get; set; }

        [DisplayName(nameof(SponsorViews))]
        public List<AppViewEnum> SponsorViews { get; set; }
    }

    public class SponsorLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

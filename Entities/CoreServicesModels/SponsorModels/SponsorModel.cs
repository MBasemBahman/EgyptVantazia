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
}

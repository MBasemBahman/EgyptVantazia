using Entities.DBModels.SponsorModels;

namespace Entities.CoreServicesModels.SponsorModels
{
    public class SponsorModel : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(LinkUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string LinkUrl { get; set; }

        public SponsorLang SponsorLang { get; set; }
    }
}

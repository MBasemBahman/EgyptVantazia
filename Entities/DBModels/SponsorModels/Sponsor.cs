namespace Entities.DBModels.SponsorModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class Sponsor : AuditImageEntity, ILookUpEntity
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

    public class SponsorLang : LangEntity<Sponsor>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

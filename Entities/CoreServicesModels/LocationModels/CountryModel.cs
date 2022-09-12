namespace Entities.CoreServicesModels.LocationModels
{
    public class CountryModel : AuditLookUpEntity
    {
    }

    public class CountryCreateOrEditModel
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        public string Name { get; set; }

        public CountryLangModel CountryLang { get; set; }
    }

    public class CountryLangModel
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        public string Name { get; set; }
    }

}

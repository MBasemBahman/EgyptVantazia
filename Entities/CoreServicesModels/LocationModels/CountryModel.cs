namespace Entities.CoreServicesModels.LocationModels
{
    public class CountryModel : AuditImageEntity, ILookUpEntity
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(Name))]
        public string Name { get; set; }


        [DisplayName(nameof(Order))]
        public int Order { get; set; }

        [DisplayName(nameof(AccountsCount))]
        public int AccountsCount { get; set; }

        [DisplayName(nameof(NationalitiesCount))]
        public int NationalitiesCount { get; set; }
    }

    public class CountryCreateOrEditModel
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        public string Name { get; set; }


        [DisplayName(nameof(Order))]
        public int Order { get; set; }

        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }
        public CountryLangModel CountryLang { get; set; }
    }

    public class CountryLangModel
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        public string Name { get; set; }
    }

}

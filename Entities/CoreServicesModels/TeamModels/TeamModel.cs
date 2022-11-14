using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.TeamModels
{
    public class TeamParameters : RequestParameters
    {
        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }


        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        public bool? IsActive { get; set; }
    }

    public class TeamModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ShortName))]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }

        [DisplayName(nameof(ShirtImageUrl))]
        public string ShirtImageUrl { get; set; }

        public bool IsAwayTeam { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }
    }

    public class TeamCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.ArLang}")]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }

        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(ShirtImageUrl))]
        public string ShirtImageUrl { get; set; }

        [DisplayName(nameof(ShirtStorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string ShirtStorageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }

        public TeamLangModel TeamLang { get; set; }
    }

    public class TeamLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.EnLang}")]
        public string ShortName { get; set; }
    }
}

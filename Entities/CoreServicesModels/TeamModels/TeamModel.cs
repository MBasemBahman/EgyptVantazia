using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.TeamModels
{
    public class TeamParameters : RequestParameters
    {
        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }
    }

    public class TeamModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }
    }

    public class TeamCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }

        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }

        public TeamLangModel TeamLang { get; set; }
    }

    public class TeamLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

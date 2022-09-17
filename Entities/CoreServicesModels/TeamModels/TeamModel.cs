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
}

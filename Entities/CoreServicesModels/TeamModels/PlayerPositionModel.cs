using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.TeamModels
{
    public class PlayerPositionParameters : RequestParameters
    {
        [DisplayName(nameof(_365_PositionId))]
        public string _365_PositionId { get; set; }
    }

    public class PlayerPositionModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_PositionId))]
        public string _365_PositionId { get; set; }
    }
}

using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.SeasonModels
{
    public class SeasonParameters : RequestParameters
    {
        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }
    }

    public class SeasonModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }

        [DisplayName(nameof(_365_AfterGameStartId))]
        public string _365_AfterGameStartId { get; set; }
    }
}

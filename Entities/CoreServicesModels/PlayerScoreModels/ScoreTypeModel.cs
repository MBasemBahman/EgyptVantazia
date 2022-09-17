using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerScoreModels
{
    public class ScoreTypeParameters : RequestParameters
    {
        [DisplayName(nameof(_365_TypeId))]
        public string _365_TypeId { get; set; }
    }

    public class ScoreTypeModel : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(Description))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName(nameof(_365_TypeId))]
        public string _365_TypeId { get; set; }
    }
}

using Entities.DBModels.SeasonModels;

namespace Entities.CoreServicesModels.SeasonModels
{
    public class SeasonModel : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }

        public SeasonLang SeasonLang { get; set; }
    }
}

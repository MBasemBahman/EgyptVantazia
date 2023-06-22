using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerMarkModels
{
    public class MarkParameters : RequestParameters
    {
        public List<int> Ids { get; set; }
    }

    public class MarkModel : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }
    }

    public class MarkCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        public MarkLangModel MarkLang { get; set; }
    }

    public class MarkLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

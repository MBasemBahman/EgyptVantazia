using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.MatchStatisticModels
{
    public class StatisticCategoryParameters : RequestParameters
    {
        public string _365_Id { get; set; }
    }
    public class StatisticCategoryModel : AuditLookUpEntity
    {
        [DisplayName(nameof(_365_Id))]
        public string _365_Id { get; set; }
    }

    public class StatisticCategoryCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_Id))]
        public string _365_Id { get; set; }

        public StatisticCategoryLangModel StatisticCategoryLang { get; set; }
    }

    public class StatisticCategoryLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

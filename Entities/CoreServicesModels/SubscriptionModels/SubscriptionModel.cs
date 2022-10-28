using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.SubscriptionModels
{
    public class SubscriptionParameters : RequestParameters
    {

    }

    public class SubscriptionModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }

    public class SubscriptionCreateOrEditModel
    {
        public SubscriptionCreateOrEditModel()
        {

        }
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        public string StorageUrl { get; set; }

        public SubscriptionLangModel SubscriptionLang { get; set; }
    }

    public class SubscriptionLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

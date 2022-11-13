using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.SubscriptionModels
{
    public class SubscriptionParameters : RequestParameters
    {
        public bool? IsActive { get; set; }
        public bool? ForAction { get; set; }
    }

    public class SubscriptionModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(Cost))]
        public int Cost { get; set; }

        [DisplayName(nameof(Discount))]
        public int Discount { get; set; }

        [DisplayName(nameof(CostAfterDiscount))]
        public int CostAfterDiscount => Cost - Discount;

        [DisplayName(nameof(ForAction))]
        public bool ForAction { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }
    }

    public class SubscriptionCreateOrEditModel
    {
        public SubscriptionCreateOrEditModel()
        {
        }

        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(Cost))]
        public int Cost { get; set; }

        [DisplayName(nameof(Discount))]
        public int Discount { get; set; }

        [DisplayName(nameof(ForAction))]
        public bool ForAction { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; } = true;

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

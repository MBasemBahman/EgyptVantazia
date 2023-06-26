using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PromoCodeModels
{
    public class PromoCodeParameters : RequestParameters
    {
        [DisplayName(nameof(IsActive))]
        public bool? IsActive { get; set; }

        public string Code { get; set; }

        public int Fk_Subscription { get; set; }

        public int Fk_Account { get; set; }
    }
    public class PromoCodeModel : AuditLookUpEntity
    {

        [DisplayName($"{nameof(Description)}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Description { get; set; }

        [DisplayName(nameof(Code))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Code { get; set; }

        [DisplayName(nameof(Discount))]
        public int Discount { get; set; }

        [DisplayName(nameof(MaxDiscount))]
        public int? MaxDiscount { get; set; }

        [DisplayName(nameof(MaxUse))]
        public int? MaxUse { get; set; }

        [DisplayName(nameof(MaxUsePerUser))]
        public int? MaxUsePerUser { get; set; }

        [DisplayName(nameof(IsActive))]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        [DisplayName(nameof(ExpirationDate))]
        public DateTime ExpirationDate { get; set; }

        [DisplayName("UsedCount")]
        public int UsedCount { get; set; }

        [DisplayName("UserUsedCount")]
        public int UserUsedCount { get; set; }

        [DisplayName("DiscountAmount")]
        public double DiscountAmount { get; set; }

        [DisplayName("IsValid")]
        public bool IsValid => IsActive && !IsExpired && !IsMaxReach && !IsMaxReachPerUser;

        public bool IsExpired => !(ExpirationDate.Date >= DateTime.UtcNow.Date);

        public bool IsMaxReach => !(MaxUse == null || MaxUse > UsedCount);

        public bool IsMaxReachPerUser => !(MaxUsePerUser == null || MaxUsePerUser > UserUsedCount);
    }

    public class PromoCodeCreateOrEditModel
    {
        public PromoCodeCreateOrEditModel()
        {
            Fk_Subscriptions = new List<int>();
        }

        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(Description)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Description { get; set; }

        [DisplayName(nameof(Code))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Code { get; set; }

        [DisplayName(nameof(Discount))]
        public int Discount { get; set; }

        [DisplayName(nameof(MinPrice))]
        public int? MinPrice { get; set; }

        [DisplayName(nameof(MaxDiscount))]
        public int? MaxDiscount { get; set; }

        [DisplayName(nameof(MaxUse))]
        public int? MaxUse { get; set; }

        [DisplayName(nameof(MaxUsePerUser))]
        public int? MaxUsePerUser { get; set; }

        [DisplayName(nameof(IsActive))]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        [DisplayName(nameof(ExpirationDate))]
        public DateTime ExpirationDate { get; set; }

        [DisplayName(nameof(PromoCodeLang))]
        public PromoCodeLangModel PromoCodeLang { get; set; }

        [DisplayName("Subscriptions")]
        public List<int> Fk_Subscriptions { get; set; }
    }

    public class PromoCodeLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(Description)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Description { get; set; }

    }
}

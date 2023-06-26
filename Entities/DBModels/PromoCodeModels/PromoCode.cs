using Entities.DBModels.AccountModels;

namespace Entities.DBModels.PromoCodeModels
{
    [Index(nameof(Code), IsUnique = true)]
    public class PromoCode : AuditEntity
    {
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

        [DisplayName(nameof(PromoCodeSubscriptions))]
        public List<PromoCodeSubscription> PromoCodeSubscriptions { get; set; }

        [DisplayName(nameof(AccountSubscriptions))]
        public IList<AccountSubscription> AccountSubscriptions { get; set; }

        [DisplayName(nameof(PromoCodeLang))]
        public PromoCodeLang PromoCodeLang { get; set; }
    }

    public class PromoCodeLang : LangEntity<PromoCode>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(Description)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Description { get; set; }
    }
}

using Entities.DBModels.AccountModels;
using Entities.DBModels.PromoCodeModels;

namespace Entities.DBModels.SubscriptionModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class Subscription : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(Description)}{PropertyAttributeConstants.ArLang}")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName(nameof(Cost))]
        public int Cost { get; set; }

        [DisplayName(nameof(Discount))]
        public int Discount { get; set; }

        [DisplayName(nameof(CostAfterDiscount))]
        public int CostAfterDiscount => Cost - Discount;

        [DisplayName(nameof(ForAction))]
        public bool ForAction { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; } = true;

        [DisplayName(nameof(Order))]
        public int Order { get; set; }

        [DisplayName(nameof(AccountSubscriptions))]
        public IList<AccountSubscription> AccountSubscriptions { get; set; }

        [DisplayName(nameof(PromoCodeSubscriptions))]
        public List<PromoCodeSubscription> PromoCodeSubscriptions { get; set; }

        public SubscriptionLang SubscriptionLang { get; set; }
    }

    public class SubscriptionLang : LangEntity<Subscription>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(Description)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}

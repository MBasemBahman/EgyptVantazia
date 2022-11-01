using Entities.DBModels.AccountModels;

namespace Entities.DBModels.SubscriptionModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class Subscription : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(AccountSubscriptions))]
        public IList<AccountSubscription> AccountSubscriptions { get; set; }

        public SubscriptionLang SubscriptionLang { get; set; }
    }

    public class SubscriptionLang : LangEntity<Subscription>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

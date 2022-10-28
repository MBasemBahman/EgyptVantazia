using Entities.DBModels.SubscriptionModels;

namespace Entities.DBModels.AccountModels
{
    public class AccountSubscription : BaseEntity
    {
        [DisplayName(nameof(Account))]
        [ForeignKey(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public Account Account { get; set; }

        [DisplayName(nameof(Subscription))]
        [ForeignKey(nameof(Subscription))]
        public int Fk_Subscription { get; set; }

        [DisplayName(nameof(Subscription))]
        public Subscription Subscription { get; set; }

        [DisplayName(nameof(StartDate))]
        public DateTime StartDate { get; set; }

        [DisplayName(nameof(EndDate))]
        public DateTime EndDate { get; set; }

        [DisplayName(nameof(IsAction))]
        public bool IsAction { get; set; }
    }
}

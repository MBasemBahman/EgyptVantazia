using Entities.DBModels.SubscriptionModels;

namespace Entities.DBModels.PromoCodeModels
{
    public class PromoCodeSubscription : BaseEntity
    {
        [DisplayName(nameof(PromoCode))]
        [ForeignKey(nameof(PromoCode))]
        public int Fk_PromoCode { get; set; }

        [DisplayName(nameof(PromoCode))]
        public PromoCode PromoCode { get; set; }

        [DisplayName(nameof(Subscription))]
        [ForeignKey(nameof(Subscription))]
        public int Fk_Subscription { get; set; }

        [DisplayName(nameof(Subscription))]
        public Subscription Subscription { get; set; }
    }
}

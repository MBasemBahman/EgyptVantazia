using Entities.DBModels.PromoCodeModels;
using Entities.CoreServicesModels.SubscriptionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PromoCodeModels
{
    public class PromoCodeSubscriptionParameters : RequestParameters
    {
        public int Fk_PromoCode { get; set; }
        public int Fk_Subscription { get; set; }

    }
    public class PromoCodeSubscriptionModel : BaseEntity
    {
        [DisplayName(nameof(PromoCode))]
        [ForeignKey(nameof(PromoCode))]
        public int Fk_PromoCode { get; set; }

        [DisplayName(nameof(PromoCode))]
        public PromoCodeModel PromoCode { get; set; }

        [DisplayName(nameof(Subscription))]
        [ForeignKey(nameof(Subscription))]
        public int Fk_Subscription { get; set; }

        [DisplayName(nameof(Subscription))]
        public SubscriptionModel Subscription { get; set; }
    }
}

﻿using Entities.DBModels.PromoCodeModels;
using Entities.DBModels.SeasonModels;
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

        [DisplayName(nameof(PromoCode))]
        [ForeignKey(nameof(PromoCode))]
        public int? Fk_PromoCode { get; set; }

        [DisplayName(nameof(PromoCode))]
        public PromoCode PromoCode { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public Season Season { get; set; }

        [DisplayName(nameof(IsAction))]
        public bool IsAction { get; set; }

        [DisplayName(nameof(IsAction))]
        public bool IsActive { get; set; }

        [DisplayName(nameof(Order_id))]
        public string Order_id { get; set; }

        [DisplayName(nameof(Cost))]
        public int Cost { get; set; }
    }
}

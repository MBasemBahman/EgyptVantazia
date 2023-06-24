using Dashboard.Areas.SubscriptionEntity.Models;
using Entities.CoreServicesModels.PromoCodeModels;
using System.ComponentModel;

namespace Dashboard.Areas.PromoCodeEntity.Models
{
    public class PromoCodeFilter : DtParameters
    {
        public int Id { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool? IsActive { get; set; }
    }
    public class PromoCodeDto : PromoCodeModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(ExpirationDate))]
        public new string ExpirationDate { get; set; }

        [DisplayName(nameof(Subscriptions))]
        public List<SubscriptionDto> Subscriptions { get; set; }
    }
}

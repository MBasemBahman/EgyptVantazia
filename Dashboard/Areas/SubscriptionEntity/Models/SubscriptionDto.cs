using Entities.CoreServicesModels.SubscriptionModels;
using System.ComponentModel;

namespace Dashboard.Areas.SubscriptionEntity.Models
{
    public class SubscriptionFilter : DtParameters
    {
        public int Id { get; set; }
    }
    public class SubscriptionDto : SubscriptionModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}

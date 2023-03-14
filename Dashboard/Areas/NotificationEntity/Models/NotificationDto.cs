using Entities.CoreServicesModels.NotificationModels;
using System.ComponentModel;

namespace Dashboard.Areas.NotificationEntity.Models
{
    public class NotificationFilter : DtParameters
    {
        public int Id { get; set; }
    }
    public class NotificationDto : NotificationModel
    {
        [DisplayName(nameof(ShowAt))]
        public new string ShowAt { get; set; }
        
        [DisplayName(nameof(ExpireAt))]
        public new string ExpireAt { get; set; }
        
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}

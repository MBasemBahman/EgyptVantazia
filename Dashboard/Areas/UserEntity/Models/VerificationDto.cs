using Entities.CoreServicesModels.UserModels;
using System.ComponentModel;

namespace Dashboard.Areas.UserEntity.Models
{
    public class VerificationDto : VerificationModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }

    public class VerificationFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_User { get; set; }
    }
}

using Entities.CoreServicesModels.UserModels;
using System.ComponentModel;

namespace Dashboard.Areas.UserEntity.Models
{
    public class UserDeviceDto : UserDeviceModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }

    public class UserDeviceFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_User { get; set; }
    }
}

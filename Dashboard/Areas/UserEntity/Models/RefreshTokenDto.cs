using Entities.CoreServicesModels.UserModels;
using System.ComponentModel;

namespace Dashboard.Areas.UserEntity.Models
{
    public class RefreshTokenDto : RefreshTokenModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(Expires))]
        public new string Expires { get; set; }

        [DisplayName(nameof(Revoked))]
        public new string Revoked { get; set; }
    }

    public class RefreshTokenFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_User { get; set; }
    }
}

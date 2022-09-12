using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.UserModels
{
    public class VerificationParameters : RequestParameters
    {
        public int Fk_User { get; set; }
    }
    public class VerificationModel : BaseEntity
    {
        [DisplayName(nameof(Code))]
        public string Code { get; set; }

        [DisplayName(nameof(Expires))]
        public DateTime Expires { get; set; }

        [DisplayName(nameof(CreatedByIp))]
        public string CreatedByIp { get; set; }

        [DisplayName(nameof(IsVerified))]
        public bool IsVerified { get; set; }

        [DisplayName(nameof(IsExpired))]
        public bool IsExpired { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }
    }
}

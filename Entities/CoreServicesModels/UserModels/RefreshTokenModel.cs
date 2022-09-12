using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.UserModels
{
    public class RefreshTokenParameters : RequestParameters
    {
        public int Fk_User { get; set; }
        public int refreshTokenTTL { get; set; }
    }
    public class RefreshTokenModel : BaseEntity
    {
        [DisplayName(nameof(Token))]
        public string Token { get; set; }

        [DisplayName(nameof(Expires))]
        public DateTime Expires { get; set; }

        [DisplayName(nameof(CreatedByIp))]
        public string CreatedByIp { get; set; }

        [DisplayName(nameof(Revoked))]
        public DateTime? Revoked { get; set; }

        [DisplayName(nameof(RevokedByIp))]
        public string RevokedByIp { get; set; }

        [DisplayName(nameof(ReplacedByToken))]
        public string ReplacedByToken { get; set; }

        [DisplayName(nameof(ReasonRevoked))]
        public string ReasonRevoked { get; set; }

        [DisplayName(nameof(IsExpired))]
        public bool IsExpired { get; set; }

        [DisplayName(nameof(IsRevoked))]
        public bool IsRevoked { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }
    }
}

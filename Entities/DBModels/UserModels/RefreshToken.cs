namespace Entities.DBModels.UserModels
{
    [Index(nameof(Token), IsUnique = true)]
    public class RefreshToken : BaseEntity
    {
        [ForeignKey(nameof(User))]
        [DisplayName(nameof(User))]
        public int Fk_User { get; set; }

        [DisplayName(nameof(User))]
        public User User { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(Token))]
        public string Token { get; set; }

        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName(nameof(Expires))]
        public DateTime Expires { get; set; }

        [DisplayName(nameof(CreatedByIp))]
        public string CreatedByIp { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName(nameof(Revoked))]
        public DateTime? Revoked { get; set; }

        [DisplayName(nameof(RevokedByIp))]
        public string RevokedByIp { get; set; }

        [DisplayName(nameof(ReplacedByToken))]
        public string ReplacedByToken { get; set; }

        [DisplayName(nameof(ReasonRevoked))]
        public string ReasonRevoked { get; set; }

        [DisplayName(nameof(IsExpired))]
        public bool IsExpired => DateTime.UtcNow >= Expires;

        [DisplayName(nameof(IsRevoked))]
        public bool IsRevoked => Revoked != null;

        [DisplayName(nameof(IsActive))]
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}

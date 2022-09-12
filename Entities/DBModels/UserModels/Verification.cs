namespace Entities.DBModels.UserModels
{
    [Index(nameof(Code), IsUnique = true)]
    public class Verification : BaseEntity
    {
        [ForeignKey(nameof(User))]
        [DisplayName(nameof(User))]
        public int Fk_User { get; set; }

        [DisplayName(nameof(User))]
        public User User { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(Code))]
        public string Code { get; set; }

        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName(nameof(Expires))]
        public DateTime Expires { get; set; }

        [DisplayName(nameof(CreatedByIp))]
        public string CreatedByIp { get; set; }

        [DisplayName(nameof(IsVerified))]
        public bool IsVerified { get; set; }

        [DisplayName(nameof(IsExpired))]
        public bool IsExpired => DateTime.UtcNow >= Expires;

        [DisplayName(nameof(IsActive))]
        public bool IsActive => !IsExpired;
    }
}

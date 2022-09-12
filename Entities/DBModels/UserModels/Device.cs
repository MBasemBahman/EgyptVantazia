
namespace Entities.DBModels.UserModels
{
    [Index(nameof(NotificationToken), IsUnique = true)]
    public class Device : BaseEntity
    {
        [ForeignKey(nameof(User))]
        [DisplayName(nameof(User))]
        public int Fk_User { get; set; }

        [DisplayName(nameof(User))]
        public User User { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(NotificationToken))]
        public string NotificationToken { get; set; }

        [DisplayName(nameof(DeviceType))]
        public string DeviceType { get; set; }

        [DisplayName(nameof(AppVersion))]
        public string AppVersion { get; set; }

        [DisplayName(nameof(DeviceVersion))]
        public string DeviceVersion { get; set; }

        [DisplayName(nameof(DeviceModel))]
        public string DeviceModel { get; set; }
    }
}

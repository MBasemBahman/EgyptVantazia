using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.UserModels
{
    public class UserDeviceParameters : RequestParameters
    {
        public int Fk_User { get; set; }
    }

    public class UserDeviceModel : BaseEntity
    {
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

    public class DeviceCreateModel
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string NotificationToken { get; set; }

        public string DeviceType { get; set; }

        public string AppVersion { get; set; }

        public string DeviceVersion { get; set; }

        public string DeviceModel { get; set; }
    }
}

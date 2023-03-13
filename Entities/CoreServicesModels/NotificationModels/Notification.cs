using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.NotificationModels
{
    public class NotificationParameters : RequestParameters
    {
        public bool IsActive { get; set; }

        public List<NotificationOpenTypeEnum> OpenTypes { get; set; }
    }

    public class NotificationModel : AuditImageEntity
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Title)}{PropertyAttributeConstants.ArLang}")]
        public string Title { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Description)}{PropertyAttributeConstants.ArLang}")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName(nameof(OpenType))]
        public NotificationOpenTypeEnum OpenType { get; set; }

        [DisplayName(nameof(OpenValue))]
        public string OpenValue { get; set; }

        [DisplayName(nameof(ShowAt))]
        [DataType(DataType.DateTime)]
        public DateTime? ShowAt { get; set; }

        [DisplayName(nameof(ExpireAt))]
        [DataType(DataType.DateTime)]
        public DateTime? ExpireAt { get; set; }
    }

    public class NotificationCreateOrEditModel : AuditImageEntity
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Title)}{PropertyAttributeConstants.ArLang}")]
        public string Title { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Description)}{PropertyAttributeConstants.ArLang}")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName(nameof(OpenType))]
        public NotificationOpenTypeEnum OpenType { get; set; }

        [DisplayName(nameof(OpenValue))]
        public string OpenValue { get; set; }

        [DisplayName(nameof(ShowAt))]
        [DataType(DataType.DateTime)]
        public DateTime? ShowAt { get; set; }

        [DisplayName(nameof(ExpireAt))]
        [DataType(DataType.DateTime)]
        public DateTime? ExpireAt { get; set; }

        public NotificationLangModel NotificationLang { get; set; }
    }

    public class NotificationLangModel
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Title)}{PropertyAttributeConstants.EnLang}")]
        public string Title { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName($"{nameof(Description)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}

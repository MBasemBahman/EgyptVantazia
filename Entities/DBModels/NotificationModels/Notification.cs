using static Entities.EnumData.LogicEnumData;

namespace Entities.DBModels.NotificationModels
{
    public class Notification : AuditImageEntity
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

        public NotificationLang NotificationLang { get; set; }
    }

    public class NotificationLang : LangEntity<Notification>
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

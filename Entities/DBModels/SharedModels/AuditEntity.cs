using Entities.Extensions;

namespace Entities.DBModels.SharedModels
{
    public class AuditEntity : BaseEntity, IAuditEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName(nameof(CreatedBy))]
        public string CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName(nameof(LastModifiedAt))]
        public DateTime LastModifiedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public string LastModifiedAtString => LastModifiedAt.AddHours(2).ToShortDateTimeString();

        [DisplayName(nameof(LastModifiedBy))]
        public string LastModifiedBy { get; set; }
    }
}

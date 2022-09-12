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

        [DisplayName(nameof(LastModifiedBy))]
        public string LastModifiedBy { get; set; }
    }
}

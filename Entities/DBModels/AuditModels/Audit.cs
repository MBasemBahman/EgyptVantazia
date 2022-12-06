namespace Entities.DBModels.AuditModels
{
    public class Audit : BaseEntity
    {
        public string TableName { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string CreatedBy { get; set; }
    }
}

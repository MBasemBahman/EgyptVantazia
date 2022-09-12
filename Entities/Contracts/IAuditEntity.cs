namespace Entities.Contracts
{
    public interface IAuditEntity
    {
        public string CreatedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
}

namespace Entities.DBModels.AuditModels
{
    [Index(nameof(MyJobId), IsUnique = true)]
    public class JobAudit : AuditEntity
    {
        [DisplayName(nameof(HangfireJobId))]
        public string HangfireJobId { get; set; }

        [DisplayName(nameof(HangfireJobId))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string MyJobId { get; set; }

        [DisplayName(nameof(Arguments))]
        public string Arguments { get; set; }
    }
}

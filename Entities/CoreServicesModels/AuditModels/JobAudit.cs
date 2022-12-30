using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AuditModels
{
    public class JobAuditParameters : RequestParameters
    {
        public string HangfireJobId { get; set; }

        public string MyJobId { get; set; }
    }

    public class JobAuditModel : AuditEntity
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

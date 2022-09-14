using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.LogModels
{
    public class LogParameters : RequestParameters
    {
        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }
    }
    public class LogModel : BaseEntity
    {
        [DisplayName(nameof(Level))]
        public string Level { get; set; }

        [DisplayName(nameof(Details))]
        public string Details { get; set; }

        [DisplayName(nameof(StackTrace))]
        public string StackTrace { get; set; }

        [DisplayName(nameof(Logger))]
        public string Logger { get; set; }
    }
}

using Entities.CoreServicesModels.LogModels;
using System.ComponentModel;

namespace Dashboard.Areas.LogEntity.Models
{
    public class LogDto : LogModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }

    public class LogFilter : DtParameters
    {
        public int Id { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }
    }
}

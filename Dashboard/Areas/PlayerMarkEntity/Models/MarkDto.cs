using System.ComponentModel;
using Entities.CoreServicesModels.PlayerMarkModels;

namespace Dashboard.Areas.PlayerMarkEntity.Models
{
    public class MarkFilter : DtParameters
    {
        public int Id { get; set; }
    }
    public class MarkDto : MarkModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}

using Entities.CoreServicesModels.SponsorModels;
using System.ComponentModel;

namespace Dashboard.Areas.SponsorEntity.Models
{
    public class SponsorFilter : DtParameters
    {
        public int Id { get; set; }
    }
    public class SponsorDto : SponsorModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}

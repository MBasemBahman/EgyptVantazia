using Entities.CoreServicesModels.NewsModels;
using System.ComponentModel;

namespace Dashboard.Areas.NewsEntity.Models
{
    public class NewsAttachmentFilter : DtParameters
    {
        public int Fk_News { get; set; }
    }
    public class NewsAttachmentDto : NewsAttachmentModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }
}

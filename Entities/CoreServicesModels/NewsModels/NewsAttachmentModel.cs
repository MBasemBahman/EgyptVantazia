using Entities.DBModels.NewsModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.NewsModels
{
    public class NewsAttachmentParameters : RequestParameters
    {
        public int Fk_News { get; set; }

    }
    public class NewsAttachmentModel : AttachmentEntity
    {
        [DisplayName(nameof(News))]
        [ForeignKey(nameof(News))]
        public int Fk_News { get; set; }
    }
}

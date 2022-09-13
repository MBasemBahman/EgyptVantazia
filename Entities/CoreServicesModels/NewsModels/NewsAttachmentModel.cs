using Entities.DBModels.NewsModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.NewsModels
{
    public class NewsAttachmentParameters:RequestParameters
    {
        public int Fk_News { get; set; }

    }
    public class NewsAttachmentModel: AttachmentEntity
    {
        [DisplayName(nameof(News))]
        [ForeignKey(nameof(News))]
        public int Fk_News { get; set; }
    }
}

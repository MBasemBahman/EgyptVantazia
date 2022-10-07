using Entities.CoreServicesModels.NewsModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;

namespace Dashboard.Areas.NewsEntity.Models
{
    public class NewsFilter : DtParameters
    {
        public int Id { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int? Fk_GameWeak { get; set; }

        public bool GetAttachments { get; set; } = false;
    }
    public class NewsDto : NewsModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }


    public enum NewsProfileItems
    {
        Details = 1,
        NewsAttachment = 2
    }
}

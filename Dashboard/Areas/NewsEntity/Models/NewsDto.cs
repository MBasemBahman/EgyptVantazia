using Entities.CoreServicesModels.NewsModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.NewsEntity.Models
{
    public class NewsFilter : DtParameters
    {
        public int Id { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(NewsTypeEnum))]
        public NewsTypeEnum NewsTypeEnum { get; set; }

        public bool GetAttachments { get; set; } = false;

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }
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

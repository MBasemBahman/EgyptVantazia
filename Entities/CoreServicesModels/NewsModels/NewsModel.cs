using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.NewsModels
{
    public class NewsParameters : RequestParameters
    {
        [DisplayName(nameof(GameWeak))]
        public int? Fk_GameWeak { get; set; }

        public bool GetAttachments { get; set; } = false;
    }
    public class NewsModel : AuditImageEntity
    {
        [DisplayName(nameof(Title))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Title { get; set; }

        [DisplayName(nameof(ShortDescription))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [DisplayName(nameof(LongDescription))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.MultilineText)]
        public string LongDescription { get; set; }

        [DisplayName(nameof(NewsTypeEnum))]
        public NewsTypeEnum NewsTypeEnum { get; set; }

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int? Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeakModel GameWeak { get; set; }

        [DisplayName(nameof(AttachmentsCount))]
        public int AttachmentsCount { get; set; }

        [DisplayName(nameof(NewsAttachments))]
        public IList<NewsAttachmentModel> NewsAttachments { get; set; }
    }
}

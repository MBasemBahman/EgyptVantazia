using Entities.DBModels.SeasonModels;
using static Entities.EnumData.LogicEnumData;

namespace Entities.DBModels.NewsModels
{
    public class News : AuditImageEntity
    {
        [DisplayName($"{nameof(Title)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Title { get; set; }

        [DisplayName($"{nameof(ShortDescription)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [DisplayName($"{nameof(LongDescription)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.MultilineText)]
        public string LongDescription { get; set; }

        [DisplayName(nameof(NewsTypeEnum))]
        public NewsTypeEnum NewsTypeEnum { get; set; }

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int? Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeak GameWeak { get; set; }

        [DisplayName(nameof(NewsAttachments))]
        public IList<NewsAttachment> NewsAttachments { get; set; }

        public NewsLang NewsLang { get; set; }
    }

    public class NewsLang : LangEntity<News>
    {
        [DisplayName($"{nameof(Title)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Title { get; set; }

        [DisplayName($"{nameof(ShortDescription)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [DisplayName($"{nameof(LongDescription)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.MultilineText)]
        public string LongDescription { get; set; }
    }
}

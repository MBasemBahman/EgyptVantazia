
namespace Entities.DBModels.NewsModels
{
    public class NewsAttachment : AttachmentEntity
    {
        [DisplayName(nameof(News))]
        [ForeignKey(nameof(News))]
        public int Fk_News { get; set; }

        [DisplayName(nameof(News))]
        public News News { get; set; }
    }
}

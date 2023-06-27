namespace Entities.DBModels.PlayerMarkModels
{
    public class Mark : AuditLookUpEntity, IImageEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        public string StorageUrl { get; set; }
        public string ImageUrl { get; set; }
        
        [DisplayName(nameof(PlayerMarks))]
        public List<PlayerMark> PlayerMarks { get; set; }

        public MarkLang MarkLang { get; set; }
    }

    public class MarkLang : LangEntity<Mark>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

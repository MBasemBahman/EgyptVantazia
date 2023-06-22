namespace Entities.DBModels.PlayerMarkModels
{
    public class Mark : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

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

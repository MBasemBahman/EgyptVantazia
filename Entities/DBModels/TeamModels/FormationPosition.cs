namespace Entities.DBModels.TeamModels
{
    //[Index(nameof(Name), IsUnique = true)]
    public class FormationPosition : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.ArLang}")]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_PositionId))]
        public string _365_PositionId { get; set; }

        [DisplayName(nameof(Players))]
        public IList<Player> Players { get; set; }

        public FormationPositionLang FormationPositionLang { get; set; }
    }

    public class FormationPositionLang : LangEntity<FormationPosition>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.EnLang}")]
        public string ShortName { get; set; }
    }
}

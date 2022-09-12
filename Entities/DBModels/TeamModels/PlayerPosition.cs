namespace Entities.DBModels.TeamModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class PlayerPosition : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_PositionId))]
        public string _365_PositionId { get; set; }

        [DisplayName(nameof(Players))]
        public IList<Player> Players { get; set; }

        public PlayerPositionLang PlayerPositionLang { get; set; }
    }

    public class PlayerPositionLang : LangEntity<PlayerPosition>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

namespace Entities.DBModels.PrivateLeagueModels
{
    [Index(nameof(UniqueCode), IsUnique = true)]
    public class PrivateLeague : AuditEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(UniqueCode))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string UniqueCode { get; set; }

        [DisplayName(nameof(PrivateLeagueMembers))]
        public IList<PrivateLeagueMember> PrivateLeagueMembers { get; set; }

        public PrivateLeagueLang PrivateLeagueLang { get; set; }
    }

    public class PrivateLeagueLang : LangEntity<PrivateLeague>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

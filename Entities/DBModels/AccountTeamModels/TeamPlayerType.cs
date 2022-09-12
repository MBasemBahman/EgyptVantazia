namespace Entities.DBModels.AccountTeamModels
{
    public class TeamPlayerType : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(AccountTeamPlayerGameWeaks))]
        public IList<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks { get; set; }

        public TeamPlayerTypeLang TeamPlayerTypeLang { get; set; }
    }

    public class TeamPlayerTypeLang : LangEntity<TeamPlayerType>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

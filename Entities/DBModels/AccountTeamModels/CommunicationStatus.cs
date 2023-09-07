namespace Entities.DBModels.AccountTeamModels
{
    public class CommunicationStatus : LookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        public List<AccountTeam> AccountTeams { get; set; }
    }
}

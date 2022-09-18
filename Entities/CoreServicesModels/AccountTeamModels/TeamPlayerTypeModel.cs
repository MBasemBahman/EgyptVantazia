namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class TeamPlayerTypeModel : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }
    }
}

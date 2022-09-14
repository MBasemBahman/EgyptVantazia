namespace Entities.CoreServicesModels.PrivateLeagueModels
{
    public class PrivateLeagueModel : AuditEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(UniqueCode))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string UniqueCode { get; set; }

    }
}

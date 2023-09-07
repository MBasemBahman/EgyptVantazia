namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class CommunicationStatusModel : LookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }
    }

    public class CommunicationStatusCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}

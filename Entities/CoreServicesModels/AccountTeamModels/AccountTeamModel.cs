using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamParameters : RequestParameters
    {
        public int Fk_Account { get; set; }
        public int Fk_Season { get; set; }

    }
    public class AccountTeamModel : AuditImageEntity
    {
        public int Fk_Account { get; set; }

        public int Fk_Season { get; set; }

        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(TotalPoints))]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(TotalMoney))]
        public int TotalMoney { get; set; }

    }
}

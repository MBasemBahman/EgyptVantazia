using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PrivateLeagueModels
{
    public class PrivateLeagueParameters : RequestParameters
    {
        public int Fk_Account { get; set; }

        public bool? IsAdmin { get; set; }

        public string UniqueCode { get; set; }

        public bool? HaveMembers { get; set; }

        public int Fk_Season { get; set; }
    }

    public class PrivateLeagueModel : AuditEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(UniqueCode))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string UniqueCode { get; set; }

        [DisplayName(nameof(MemberCount))]
        public int MemberCount { get; set; }

        public double MyPosition { get; set; }
    }

    public class PrivateLeagueCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }


    }

    public class PrivateLeagueCreateModel
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        public IList<int> Fk_Accounts { get; set; }
    }
}

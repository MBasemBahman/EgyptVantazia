using Entities.RequestFeatures;


namespace Entities.CoreServicesModels.AccountModels
{
    public class AccountParameters : RequestParameters
    {
        public int Fk_User { get; set; }

        public int Fk_Country { get; set; }

        public int Fk_Nationality { get; set; }

        public int Fk_FavouriteTeam { get; set; }
    }
    public class AccountModel : AuditImageEntity
    {
        public int Fk_User { get; set; }

        [DisplayName(nameof(FirstName))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string FirstName { get; set; }

        [DisplayName(nameof(LastName))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        [DisplayName(nameof(PhoneNumberTwo))]
        public string PhoneNumberTwo { get; set; }

        public int Fk_Country { get; set; }

        public int Fk_Nationality { get; set; }

        [DisplayName(nameof(Address))]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public int Fk_FavouriteTeam { get; set; }

    }
}

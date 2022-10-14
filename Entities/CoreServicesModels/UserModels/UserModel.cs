using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.UserModels
{
    public class UserParameters : RequestParameters
    {
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }


    }

    public class UserModel : AuditEntity
    {
        [DisplayName(nameof(Name))]
        public string Name { get; set; }

        [DisplayName(nameof(UserName))]
        public string UserName { get; set; }

        [DisplayName(nameof(EmailAddress))]
        public string EmailAddress { get; set; }

        [DisplayName(nameof(PhoneNumber))]
        public string PhoneNumber { get; set; }
    }

    public class UserCreateModel
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(Name))]
        public string Name { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(UserName))]
        public string UserName { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        [DisplayName(nameof(Password))]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [DisplayName(nameof(EmailAddress))]
        public string EmailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        [DisplayName(nameof(PhoneNumber))]
        public string PhoneNumber { get; set; }
    }
}

using Entities.DBModels.AccountModels;
using Entities.DBModels.DashboardAdministrationModels;

namespace Entities.DBModels.UserModels
{
    [Index(nameof(UserName), IsUnique = true)]
    public class User : AuditEntity
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(Name))]
        public string Name { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(UserName))]
        public string UserName { get; set; }

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

        [DisplayName(nameof(IsExternalLogin))]
        [DefaultValue(false)]
        public bool IsExternalLogin { get; set; }

        [DisplayName(nameof(Culture))]
        public string Culture { get; set; }

        [DisplayName(nameof(DashboardAdministrator))]
        public DashboardAdministrator DashboardAdministrator { get; set; }

        [DisplayName(nameof(Account))]
        public Account Account { get; set; }

        [DisplayName(nameof(RefreshTokens))]
        public List<RefreshToken> RefreshTokens { get; set; }

        [DisplayName(nameof(Verifications))]
        public List<Verification> Verifications { get; set; }

        [DisplayName(nameof(Devices))]
        public IList<Device> Devices { get; set; }
    }
}

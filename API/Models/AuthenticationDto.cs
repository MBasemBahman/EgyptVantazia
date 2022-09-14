using Entities.Constants;
using Entities.CoreServicesModels.AccountModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserWithAccountForRegistrationDto
    {
        public UserForRegistrationDto User { get; set; }

        public AccountCreateModel Account { get; set; }
    }

    public class UserWithAccountForEditDto
    {
        public UserForEditDto User { get; set; }

        public AccountEditModel Account { get; set; }
    }

    public class UserForAnonymouseDto
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string UserName { get; set; }

        [DisplayName(nameof(Culture))]
        public string Culture { get; set; }
    }

    public class RmvIds
    {
        public List<int> Ids { get; set; }
    }
}

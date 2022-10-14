using Entities.CoreServicesModels.UserModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Areas.UserEntity.Models
{
    public class UserDto : UserModel
    {

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

    }

    public class UserFilter : DtParameters
    {
        public int Id { get; set; }

        public int Fk_Account { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DisplayName("HasAccount")]
        public bool? HasAccount { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

    }
}

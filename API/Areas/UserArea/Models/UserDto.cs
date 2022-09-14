using Entities.CoreServicesModels.UserModels;
using System.Text.Json.Serialization;

namespace API.Areas.UserArea.Models
{
    public class UserDto : UserModel
    {
        [JsonIgnore]
        public new int Id { get; set; }

        [JsonIgnore]
        public new string LastModifiedAt { get; set; }

        [JsonIgnore]
        public new string LastModifiedBy { get; set; }

        [JsonIgnore]
        public new string CreatedBy { get; set; }

        [JsonIgnore]
        public new string CreatedAt { get; set; }
    }
}

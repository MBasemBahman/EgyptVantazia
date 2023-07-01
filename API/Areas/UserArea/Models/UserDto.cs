using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.CoreServicesModels.UserModels;
using System.Text.Json.Serialization;

namespace API.Areas.UserArea.Models
{
    public class UserDto : UserModel
    {
        [JsonIgnore]
        public new int Id { get; set; }

        public int Fk_Account { get; set; }

        public string RefCode { get; set; }

        public int RefCodeCount { get; set; }

        public int Fk_AccountTeam { get; set; }
        public int Fk_Country { get; set; }

        public CountryModel Country { get; set; }

        public int Fk_FavouriteTeam { get; set; }

        public TeamModel FavouriteTeam { get; set; }

        [JsonIgnore]
        public new string LastModifiedAt { get; set; }

        [JsonIgnore]
        public new string LastModifiedBy { get; set; }

        [JsonIgnore]
        public new string CreatedBy { get; set; }

        [JsonIgnore]
        public new string CreatedAt { get; set; }

        public List<AccountTeamModel> AccountTeams { get; set; }
    }
}

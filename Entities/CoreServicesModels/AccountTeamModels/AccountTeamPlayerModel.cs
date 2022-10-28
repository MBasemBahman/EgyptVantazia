using Entities.CoreServicesModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamPlayerParameters : RequestParameters
    {
        public int Fk_Account { get; set; }
        public int Fk_AccountTeam { get; set; }
        public int Fk_Player { get; set; }
        public int Fk_Season { get; set; }
        public int Fk_GameWeak { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class AccountTeamPlayerModel : BaseEntity
    {
        [DisplayName(nameof(AccountTeam))]
        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public AccountTeamModel AccountTeam { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public PlayerModel Player { get; set; }

        public bool IsPrimary { get; set; }

    }

    public class AccountTeamPlayerCreateModel
    {
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public List<int> Fk_Players { get; set; }
    }
}

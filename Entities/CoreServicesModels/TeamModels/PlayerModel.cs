using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.TeamModels
{
    public class PlayerParameters : RequestParameters
    {
        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }
    }
    public class PlayerModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }

        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public TeamModel Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        [ForeignKey(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public PlayerPositionModel PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerNumber))]
        public string PlayerNumber { get; set; }
    }
}

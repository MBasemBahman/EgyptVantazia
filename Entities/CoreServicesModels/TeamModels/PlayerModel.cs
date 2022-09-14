using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PlayersTransfersModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.TeamModels
{
    public class PlayerParameters : RequestParameters
    {
        public int Fk_Team { get; set; }

        public int Fk_PlayerPosition { get; set; }
    }
    public class PlayerModel : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }

        public int Fk_Team { get; set; }

        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerNumber))]
        public string PlayerNumber { get; set; }

        public PlayerPositionLang PlayerPositionLang { get; set; }
    }
}

using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.NewsModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PlayersTransfersModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.SeasonModels
{
    public class GameWeakParameters : RequestParameters
    {
        public int Fk_Season { get; set; }

    }
    public class GameWeakModel : AuditEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_GameWeakId))]
        public string _365_GameWeakId { get; set; }

        public int Fk_Season { get; set; }

        public SeasonLang SeasonLang { get; set; }
    }
}

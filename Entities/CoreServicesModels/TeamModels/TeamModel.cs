using Entities.DBModels.AccountModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.StandingsModels;
using Entities.DBModels.TeamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.TeamModels
{
    public class TeamModel : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }

        public TeamLang TeamLang { get; set; }
    }
}

using Entities.DBModels.TeamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.TeamModels
{
    public class PlayerPositionModel : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_PositionId))]
        public string _365_PositionId { get; set; }

        public PlayerPositionLang PlayerPositionLang { get; set; }
    }
}

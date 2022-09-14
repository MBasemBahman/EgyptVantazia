using Entities.DBModels.PrivateLeagueModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.PrivateLeagueModels
{
    public class PrivateLeagueModel : AuditEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(UniqueCode))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string UniqueCode { get; set; }

    }
}

using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;

namespace API.Areas.SeasonArea.Models
{
    public class GameWeakDto : GameWeakModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_GameWeakId))]
        public new string _365_GameWeakId { get; set; }
    }
}

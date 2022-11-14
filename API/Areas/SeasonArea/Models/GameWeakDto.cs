using Entities.CoreServicesModels.SeasonModels;
using System.ComponentModel;

namespace API.Areas.SeasonArea.Models
{
    public class GameWeakDto : GameWeakModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_GameWeakId))]
        public new string _365_GameWeakId { get; set; }

        [DisplayName(nameof(Deadline))]
        public new string Deadline { get; set; }
    }
}

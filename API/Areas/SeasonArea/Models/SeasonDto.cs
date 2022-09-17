using Entities.CoreServicesModels.SeasonModels;
using System.ComponentModel;

namespace API.Areas.SeasonArea.Models
{
    public class SeasonDto : SeasonModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_SeasonId))]
        public new int _365_SeasonId { get; set; }
    }
}

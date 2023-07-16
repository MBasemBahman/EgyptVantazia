using Entities.CoreServicesModels.TeamModels;
using System.ComponentModel;
namespace Dashboard.Areas.TeamEntity.Models
{
    public class FormationPositionFilter : DtParameters
    {
        public int Id { get; set; }
    }
    public class FormationPositionDto : FormationPositionModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}

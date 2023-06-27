using System.ComponentModel;
using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.DBModels.PlayerMarkModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;

namespace Dashboard.Areas.PlayerMarkEntity.Models
{
    public class PlayerMarkFilter : DtParameters
    {
        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Mark))]
        public int Fk_Mark { get; set; }
    }
    public class PlayerMarkDto : PlayerMarkModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
    
}

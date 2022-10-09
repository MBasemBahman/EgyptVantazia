using Entities.CoreServicesModels.TeamModels;
using System.ComponentModel;
namespace Dashboard.Areas.TeamEntity.Models
{
    public class PlayerPriceDto : PlayerPriceModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Team))]
        public new TeamDto Team { get; set; }

        [DisplayName(nameof(Player))]
        public new PlayerDto Player { get; set; }
    }
}

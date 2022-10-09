using Entities.CoreServicesModels.TeamModels;
using System.ComponentModel;
namespace Dashboard.Areas.TeamEntity.Models
{
    public class PlayerFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_Team { get; set; }
    }
    public class PlayerDto : PlayerModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Team))]
        public new TeamDto Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public new PlayerPositionDto PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerPrices))]
        public List<PlayerPriceDto> PlayerPrices { get; set; }
    }

    public enum PlayerProfileItems
    {
        Details = 1
    }

    public enum PlayerReturnPage
    {
        Index = 1,
        PlayerProfile = 2,
        TeamProfile = 3
    }
}

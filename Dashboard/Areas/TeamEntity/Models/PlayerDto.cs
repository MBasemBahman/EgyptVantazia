using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using System.ComponentModel;

namespace Dashboard.Areas.TeamEntity.Models
{
    public class PlayerFilter : DtParameters
    {
        public int Id { get; set; }
        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }
        
        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool? IsActive { get; set; }
        
        [DisplayName(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }
        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        public int PointsFrom { get; set; }
        public int PointsTo { get; set; }

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
        Details = 1,
        PlayerGameWeak = 2,
        PlayerGameWeakScore = 3,
        PlayerGameWeakScoreStates = 4,
        PlayerSeasonScoreStates = 5,
        PlayerMark = 6,
    }

    public enum PlayerReturnPage
    {
        Index = 1,
        PlayerProfile = 2,
        TeamProfile = 3
    }
}

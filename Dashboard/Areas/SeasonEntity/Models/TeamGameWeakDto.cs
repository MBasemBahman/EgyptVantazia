using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;

namespace Dashboard.Areas.SeasonEntity.Models
{
    public class TeamGameWeakFilter : DtParameters
    {
        [DisplayName("Team")]
        public List<int> Fk_Teams { get; set; } = new();

        [DisplayName("HomeTeam")]
        public int Fk_Home { get; set; }
        
        [DisplayName("Fk_Team")]
        public int Fk_Team { get; set; }

        [DisplayName("AwayTeam")]
        public int Fk_Away { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(_365_MatchId))]
        public string _365_MatchId { get; set; }

        [DisplayName("StartTime")]
        public DateTime? FromTime { get; set; }

        [DisplayName(nameof(ToTime))]
        public DateTime? ToTime { get; set; }

        [DisplayName(nameof(IsEnded))]
        public bool? IsEnded { get; set; }

        public string DashboardSearch { get; set; }
    }

    public class TeamGameWeakDto : TeamGameWeakModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Home))]
        public new TeamDto Home { get; set; }


        [DisplayName(nameof(Away))]
        public new TeamDto Away { get; set; }

        [DisplayName(nameof(GameWeak))]
        public new GameWeakDto GameWeak { get; set; }

        [DisplayName(nameof(StartTime))]
        public new string StartTime { get; set; }
    }

    public enum TeamGameWeakProfileItems
    {
        Details = 1
    }
    
    public enum TeamGameWeakReturnPage
    {
        Index = 1,
        SeasonProfile = 2,
        TeamProfile = 3
    }
}

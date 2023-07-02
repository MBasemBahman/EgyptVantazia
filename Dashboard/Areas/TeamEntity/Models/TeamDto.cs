using Entities.CoreServicesModels.TeamModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Areas.TeamEntity.Models
{
    public class TeamFilter : DtParameters
    {
        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        [DisplayName("Season")]
        public int Fk_Season { get; set; }
    }
    public class TeamDto : TeamModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }

    public enum TeamProfileItems
    {
        Details = 1,
        Player = 2,
        HomeTeamGameWeak = 3,
        AwayTeamGameWeak = 4,
        TeamGameWeak = 5
    }
}

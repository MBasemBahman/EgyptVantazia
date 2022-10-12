using Entities.CoreServicesModels.TeamModels;
using System.ComponentModel;

namespace Dashboard.Areas.TeamEntity.Models
{
    public class TeamFilter : DtParameters
    {


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
        AwayTeamGameWeak = 4
    }
}

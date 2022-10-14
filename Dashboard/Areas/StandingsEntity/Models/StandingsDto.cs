using Entities.CoreServicesModels.StandingsModels;
using Dashboard.Areas.SeasonEntity.Models;
using Dashboard.Areas.TeamEntity.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Areas.StandingsEntity.Models
{
    public class StandingsFilter : DtParameters
    {
        [DisplayName("Season")]
        public int Fk_Season { get; set; }

        [DisplayName("Team")]
        public int Fk_Team { get; set; }

        public int _365_For { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }
    }
    public class StandingsDto : StandingsModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Season))]
        public new SeasonDto Season { get; set; }

        [DisplayName(nameof(Team))]
        public new TeamDto Team { get; set; }
    }
}

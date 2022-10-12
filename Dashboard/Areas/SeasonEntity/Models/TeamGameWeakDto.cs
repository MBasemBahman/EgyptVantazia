using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;

namespace Dashboard.Areas.SeasonEntity.Models
{
    public class TeamGameWeakFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_Home { get; set; }

        public int Fk_Away { get; set; }

        public int Fk_Season { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }
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

    public enum TeamGameWeakReturnPage
    {
        Index = 1,
        SeasonProfile = 2,
        TeamProfile = 3
    }
}

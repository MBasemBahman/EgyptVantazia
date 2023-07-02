using Dashboard.Areas.SeasonEntity.Models;
using Entities.CoreServicesModels.PrivateLeagueModels;
using System.ComponentModel;
using Entities.DBModels.SeasonModels;

namespace Dashboard.Areas.PrivateLeagueEntity.Models
{
    public class PrivateLeagueFilter : DtParameters
    {
        public int Fk_Account { get; set; }

        public bool? IsAdmin { get; set; }

        public string UniqueCode { get; set; }

        public bool? HaveMembers { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int? Fk_GameWeak { get; set; }
    }
    
    public class PrivateLeagueDto : PrivateLeagueModel
    {

        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(GameWeak))]
        public new GameWeakDto GameWeak { get; set; }
    }
}

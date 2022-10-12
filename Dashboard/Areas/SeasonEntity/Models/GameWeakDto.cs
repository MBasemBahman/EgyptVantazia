using Entities.CoreServicesModels.SeasonModels;
using System.ComponentModel;

namespace Dashboard.Areas.SeasonEntity.Models
{
    public class GameWeakFilter : DtParameters
    {
        public int Id { get; set; }

        public int Fk_Season { get; set; }

        [DisplayName(nameof(_365_GameWeakId))]
        public string _365_GameWeakId { get; set; }
    }
    public class GameWeakDto : GameWeakModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Season))]
        public new SeasonDto Season { get; set; }
    }
}

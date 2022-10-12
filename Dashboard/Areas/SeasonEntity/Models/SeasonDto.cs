using Entities.CoreServicesModels.SeasonModels;
using System.ComponentModel;

namespace Dashboard.Areas.SeasonEntity.Models
{
    public class SeasonFilter : DtParameters
    {
        public int Id { get; set; }

        [DisplayName(nameof(_365_SeasonId))]
        public string _365_SeasonId { get; set; }
    }
    public class SeasonDto : SeasonModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(GameWeaks))]
        public List<GameWeakDto> GameWeaks { get; set; }
    }

    public enum SeasonProfileItems
    {
        Details = 1,
        TeamGameWeak = 2
    }

    public enum SeasonReturnPage
    {
        Index = 1,
        SeasonProfile = 2
    }

}

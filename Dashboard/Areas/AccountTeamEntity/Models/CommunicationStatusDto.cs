using Entities.CoreServicesModels.AccountTeamModels;
using System.ComponentModel;

namespace Dashboard.Areas.AccountTeamEntity.Models
{
    public class CommunicationStatusFilter : DtParameters
    {

    }
    public class CommunicationStatusDto : CommunicationStatusModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public string LastModifiedAt { get; set; }
    }
}

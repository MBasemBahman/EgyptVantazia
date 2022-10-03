using Entities.CoreServicesModels.AppInfoModels;
using System.ComponentModel;

namespace Dashboard.Areas.AppInfoEntity.Models
{
    public class AppAboutDto : AppAboutModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

    }
}

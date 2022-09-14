using Entities.DBModels.SponsorModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.SponsorModels
{
    public class SponsorViewParameters : RequestParameters
    {
        public int Fk_Sponsor { get; set; }

    }
    public class SponsorViewModel : BaseEntity
    {
        public int Fk_Sponsor { get; set; }

        [DisplayName(nameof(AppViewEnum))]
        public AppViewEnum AppViewEnum { get; set; }
    }
}

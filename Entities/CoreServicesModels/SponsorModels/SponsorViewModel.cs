using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.SponsorModels
{
    public class SponsorViewParameters : RequestParameters
    {
        public int Fk_Sponsor { get; set; }

    }
    public class SponsorViewModel : BaseEntity
    {
        [DisplayName(nameof(AppViewEnum))]
        public AppViewEnum AppViewEnum { get; set; }
    }
}

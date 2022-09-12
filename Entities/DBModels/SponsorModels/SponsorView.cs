namespace Entities.DBModels.SponsorModels
{
    public class SponsorView : BaseEntity
    {
        [DisplayName(nameof(Sponsor))]
        [ForeignKey(nameof(Sponsor))]
        public int Fk_Sponsor { get; set; }

        [DisplayName(nameof(Sponsor))]
        public Sponsor Sponsor { get; set; }

        [DisplayName(nameof(AppViewEnum))]
        public int AppViewEnum { get; set; }
    }
}

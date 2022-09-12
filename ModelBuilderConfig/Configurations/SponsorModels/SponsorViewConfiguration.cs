
using Entities.DBModels.SponsorModels;

namespace ModelBuilderConfig.Configurations.SponsorModels
{
    public class SponsorViewConfiguration : IEntityTypeConfiguration<SponsorView>
    {
        public void Configure(EntityTypeBuilder<SponsorView> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Sponsor, a.AppViewEnum }).IsUnique();
        }
    }
}

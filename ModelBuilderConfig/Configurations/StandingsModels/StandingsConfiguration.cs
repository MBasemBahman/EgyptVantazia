using Entities.DBModels.StandingsModels;

namespace ModelBuilderConfig.Configurations.StandingsModels
{
    public class StandingsConfiguration : IEntityTypeConfiguration<Standings>
    {
        public void Configure(EntityTypeBuilder<Standings> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Season, a.Fk_Team }).IsUnique();
        }
    }
}

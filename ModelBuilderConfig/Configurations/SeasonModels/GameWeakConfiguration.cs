
using Entities.DBModels.SeasonModels;

namespace ModelBuilderConfig.Configurations.SeasonModels
{
    public class GameWeakConfiguration : IEntityTypeConfiguration<GameWeak>
    {
        public void Configure(EntityTypeBuilder<GameWeak> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Season, a.Name }).IsUnique();
        }
    }
}

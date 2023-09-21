
using Entities.DBModels.SeasonModels;

namespace ModelBuilderConfig.Configurations.SeasonModels
{
    public class GameWeakConfiguration : IEntityTypeConfiguration<GameWeak>
    {
        public void Configure(EntityTypeBuilder<GameWeak> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Season, a.Name }).IsUnique();

            // set _365_GameWeakIdValue default value 0
            //_ = builder.Property(a => a._365_GameWeakIdValue).HasDefaultValue(0);

            _ = builder
                .Property(a => a._365_GameWeakIdValue)
                .HasComputedColumnSql("CONVERT(int, _365_GameWeakId)");
        }
    }
}

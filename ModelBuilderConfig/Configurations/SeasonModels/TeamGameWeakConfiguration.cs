
using Entities.DBModels.SeasonModels;

namespace ModelBuilderConfig.Configurations.SeasonModels
{
    public class TeamGameWeakConfiguration : IEntityTypeConfiguration<TeamGameWeak>
    {
        public void Configure(EntityTypeBuilder<TeamGameWeak> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Away, a.Fk_Home, a.Fk_GameWeak }).IsUnique();

            _ = builder.HasOne(a => a.Home)
                   .WithMany(a => a.HomeGameWeaks)
                   .HasForeignKey(a => a.Fk_Home);

            _ = builder.HasOne(a => a.Away)
                   .WithMany(a => a.AwayGameWeaks)
                   .HasForeignKey(a => a.Fk_Away);
        }
    }
}

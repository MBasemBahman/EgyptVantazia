using Entities.DBModels.PlayerScoreModels;

namespace ModelBuilderConfig.Configurations.PlayerScoreModels
{
    public class PlayerGameWeakConfiguration : IEntityTypeConfiguration<PlayerGameWeak>
    {
        public void Configure(EntityTypeBuilder<PlayerGameWeak> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_TeamGameWeak, a.Fk_Player }).IsUnique();
        }
    }
}

using Entities.DBModels.PlayerScoreModels;

namespace ModelBuilderConfig.Configurations.PlayerScoreModels
{
    public class PlayerGameWeakScoreConfiguration : IEntityTypeConfiguration<PlayerGameWeakScore>
    {
        public void Configure(EntityTypeBuilder<PlayerGameWeakScore> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_PlayerGameWeak, a.Fk_ScoreType }).IsUnique();
        }
    }
}

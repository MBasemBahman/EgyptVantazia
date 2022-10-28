
using Entities.DBModels.PlayerStateModels;

namespace ModelBuilderConfig.Configurations.PlayerStateModels
{
    public class PlayerSeasonScoreStateConfiguration : IEntityTypeConfiguration<PlayerSeasonScoreState>
    {
        public void Configure(EntityTypeBuilder<PlayerSeasonScoreState> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Player, a.Fk_Season, a.Fk_ScoreState }).IsUnique();
        }
    }
}

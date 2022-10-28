
using Entities.DBModels.PlayerStateModels;

namespace ModelBuilderConfig.Configurations.PlayerStateModels
{
    public class PlayerGameWeakScoreStateConfiguration : IEntityTypeConfiguration<PlayerGameWeakScoreState>
    {
        public void Configure(EntityTypeBuilder<PlayerGameWeakScoreState> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Player, a.Fk_GameWeak, a.Fk_ScoreState }).IsUnique();
        }
    }
}

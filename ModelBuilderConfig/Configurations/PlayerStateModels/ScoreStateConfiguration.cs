using Entities.DBModels.PlayerStateModels;

namespace ModelBuilderConfig.Configurations.PlayerStateModels
{
    public class ScoreStateConfiguration : IEntityTypeConfiguration<ScoreState>
    {
        public void Configure(EntityTypeBuilder<ScoreState> builder)
        {
            foreach (ScoreStateEnum value in Enum.GetValues(typeof(ScoreStateEnum)))
            {
                _ = builder.HasData(new ScoreState
                {
                    Id = (int)value,
                    Name = value.ToString(),
                });
            }
        }
    }

    public class ScoreStateLangConfiguration : IEntityTypeConfiguration<ScoreStateLang>
    {
        public void Configure(EntityTypeBuilder<ScoreStateLang> builder)
        {
            foreach (ScoreStateEnum value in Enum.GetValues(typeof(ScoreStateEnum)))
            {
                _ = builder.HasData(new ScoreStateLang
                {
                    Id = (int)value,
                    Fk_Source = (int)value,
                    Name = value.ToString()
                });
            }
        }
    }
}

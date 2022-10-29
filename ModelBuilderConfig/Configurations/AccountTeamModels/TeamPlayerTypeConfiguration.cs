using Entities.DBModels.AccountTeamModels;

namespace ModelBuilderConfig.Configurations.AccountTeamModels
{
    public class TeamPlayerTypeConfiguration : IEntityTypeConfiguration<TeamPlayerType>
    {
        public void Configure(EntityTypeBuilder<TeamPlayerType> builder)
        {
            foreach (TeamPlayerTypeEnum value in Enum.GetValues(typeof(TeamPlayerTypeEnum)))
            {
                _ = builder.HasData(new TeamPlayerType
                {
                    Id = (int)value,
                    Name = value.ToString(),
                });
            }
        }
    }

    public class TeamPlayerTypeLangConfiguration : IEntityTypeConfiguration<TeamPlayerTypeLang>
    {
        public void Configure(EntityTypeBuilder<TeamPlayerTypeLang> builder)
        {
            foreach (TeamPlayerTypeEnum value in Enum.GetValues(typeof(TeamPlayerTypeEnum)))
            {
                _ = builder.HasData(new TeamPlayerTypeLang
                {
                    Id = (int)value,
                    Fk_Source = (int)value,
                    Name = value.ToString()
                });
            }
        }
    }
}

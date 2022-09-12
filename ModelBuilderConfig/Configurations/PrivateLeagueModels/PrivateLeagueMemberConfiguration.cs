using Entities.DBModels.PrivateLeagueModels;

namespace ModelBuilderConfig.Configurations.PrivateLeagueModels
{
    public class PrivateLeagueMemberConfiguration : IEntityTypeConfiguration<PrivateLeagueMember>
    {
        public void Configure(EntityTypeBuilder<PrivateLeagueMember> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Account, a.Fk_PrivateLeague }).IsUnique();
        }
    }
}

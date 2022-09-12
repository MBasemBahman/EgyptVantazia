using Entities.DBModels.AccountTeamModels;

namespace ModelBuilderConfig.Configurations.AccountTeamModels
{
    public class AccountTeamPlayerConfiguration : IEntityTypeConfiguration<AccountTeamPlayer>
    {
        public void Configure(EntityTypeBuilder<AccountTeamPlayer> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Player, a.Fk_AccountTeam }).IsUnique();
        }
    }
}

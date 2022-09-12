using Entities.DBModels.AccountTeamModels;

namespace ModelBuilderConfig.Configurations.AccountTeamModels
{
    public class AccountTeamGameWeakConfiguration : IEntityTypeConfiguration<AccountTeamGameWeak>
    {
        public void Configure(EntityTypeBuilder<AccountTeamGameWeak> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_AccountTeam, a.Fk_GameWeak }).IsUnique();
        }
    }
}

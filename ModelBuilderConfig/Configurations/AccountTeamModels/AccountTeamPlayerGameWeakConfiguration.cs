using Entities.DBModels.AccountTeamModels;

namespace ModelBuilderConfig.Configurations.AccountTeamModels
{
    public class AccountTeamPlayerGameWeakConfiguration : IEntityTypeConfiguration<AccountTeamPlayerGameWeak>
    {
        public void Configure(EntityTypeBuilder<AccountTeamPlayerGameWeak> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_GameWeak, a.Fk_TeamPlayerType, a.Fk_AccountTeamPlayer }).IsUnique();
        }
    }
}

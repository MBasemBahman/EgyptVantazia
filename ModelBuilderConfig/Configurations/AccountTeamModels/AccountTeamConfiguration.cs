using Entities.DBModels.AccountTeamModels;

namespace ModelBuilderConfig.Configurations.AccountTeamModels
{
    public class AccountTeamConfiguration : IEntityTypeConfiguration<AccountTeam>
    {
        public void Configure(EntityTypeBuilder<AccountTeam> builder)
        {
            _ = builder.HasIndex(a => new { a.Fk_Account, a.Fk_Season }).IsUnique();
        }
    }
}

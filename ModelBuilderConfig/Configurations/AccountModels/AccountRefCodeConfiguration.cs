
using Entities.DBModels.AccountModels;

namespace ModelBuilderConfig.Configurations.AccountModels
{
    public class AccountRefCodeConfiguration : IEntityTypeConfiguration<AccountRefCode>
    {
        public void Configure(EntityTypeBuilder<AccountRefCode> builder)
        {
            _ = builder.HasOne(a => a.NewAccount)
                       .WithOne(a => a.NewAccountRefCode);

            _ = builder.HasOne(a => a.RefAccount)
                   .WithMany(a => a.RefAccountsRefCode)
                   .HasForeignKey(a => a.Fk_RefAccount);

            _ = builder.HasIndex(a => new { a.Fk_NewAccount, a.Fk_RefAccount }).IsUnique();
        }
    }
}

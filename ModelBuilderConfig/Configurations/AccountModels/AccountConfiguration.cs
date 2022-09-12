
using Entities.DBModels.AccountModels;

namespace ModelBuilderConfig.Configurations.AccountModels
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            _ = builder.HasOne(a => a.Country)
                   .WithMany(a => a.Accounts)
                   .HasForeignKey(a => a.Fk_Country);

            _ = builder.HasOne(a => a.Nationality)
                   .WithMany(a => a.AccountNationalities)
                   .HasForeignKey(a => a.Fk_Nationality);
        }
    }
}

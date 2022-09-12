
using Entities.DBModels.AccountModels;
using Entities.DBModels.LocationModels;

namespace ModelBuilderConfig.Configurations.AccountModels
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasOne<Country>()
                   .WithMany(a => a.Accounts)
                   .HasForeignKey(a => a.Fk_Country);

            builder.HasOne<Country>()
                   .WithMany(a => a.AccountNationalities)
                   .HasForeignKey(a => a.Fk_Nationality);
        }
    }
}

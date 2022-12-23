
using Entities.DBModels.AccountModels;

namespace ModelBuilderConfig.Configurations.AccountModels
{
    public class AccountSubscriptionConfiguration : IEntityTypeConfiguration<AccountSubscription>
    {
        public void Configure(EntityTypeBuilder<AccountSubscription> builder)
        {
            //_ = builder.HasIndex(a => new { a.Fk_Account, a.Fk_Season, a.Fk_Subscription }).IsUnique();
        }
    }
}

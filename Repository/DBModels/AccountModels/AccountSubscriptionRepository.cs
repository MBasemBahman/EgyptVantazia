using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;

namespace Repository.DBModels.AccountModels
{
    public class AccountSubscriptionRepository : RepositoryBase<AccountSubscription>
    {
        public AccountSubscriptionRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountSubscription> FindAll(AccountSubscriptionParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                       parameters.Fk_Account,
                       parameters.Fk_Subscription,
                       parameters.Fk_Season);
        }

        public async Task<AccountSubscription> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }


    }

    public static class AccountSubscriptionRepositoryExtension
    {
        public static IQueryable<AccountSubscription> Filter(
            this IQueryable<AccountSubscription> AccountSubscriptions,
            int id,
            int Fk_Account,
            int Fk_Subscription,
            int Fk_Season)
        {
            return AccountSubscriptions.Where(a => (id == 0 || a.Id == id) &&
                                                 (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                                 (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                                 (Fk_Subscription == 0 || a.Fk_Subscription == Fk_Subscription));

        }

    }
}

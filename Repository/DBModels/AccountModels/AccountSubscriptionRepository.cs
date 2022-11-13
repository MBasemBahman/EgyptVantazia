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
                       parameters.Fk_Season,
                       parameters.IsAction,
                       parameters.IsActive);
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
            int Fk_Season,
            bool? isAction,
            bool? isActive)
        {
            return AccountSubscriptions.Where(a => (id == 0 || a.Id == id) &&
                                                 (isAction == null || a.IsAction == isAction) &&
                                                 (isActive == null || a.IsActive == isActive) &&
                                                 (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                                 (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                                 (Fk_Subscription == 0 || a.Fk_Subscription == Fk_Subscription));

        }

    }
}

using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.AccountModels;

namespace Repository.DBModels.AccountModels
{
    public class AccountSubscriptionRepository : RepositoryBase<AccountSubscription>
    {
        public AccountSubscriptionRepository(BaseContext context) : base(context)
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
                       parameters.IsActive,
                       parameters.Order_id,
                       parameters.NotEqualSubscriptionId,
                       parameters.CreatedAtFrom,
                       parameters.CreatedAtTo,
                       parameters.DashboardSearch);
        }

        public async Task<AccountSubscription> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
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
            bool? isActive,
            string order_id,
            int NotEqualSubscriptionId,
            DateTime? createdAtFrom,
            DateTime? createdAtTo,
            string dashboardSearch)
        {
            return AccountSubscriptions.Where(a => (id == 0 || a.Id == id) &&
                                                 
                                                 (string.IsNullOrEmpty(dashboardSearch) || 
                                                    a.Account.FullName.Contains(dashboardSearch) ||
                                                    a.Id.ToString().Contains(dashboardSearch) ||
                                                    a.Season.Name.Contains(dashboardSearch) ||
                                                    a.Subscription.Name.Contains(dashboardSearch) ||
                                                    a.Cost.ToString().Contains(dashboardSearch) ||
                                                    a.Order_id.Contains(dashboardSearch)) &&
                                                 
                                                 (NotEqualSubscriptionId == 0 || a.Fk_Subscription != NotEqualSubscriptionId) &&
                                                 (isAction == null || a.IsAction == isAction) &&
                                                 (string.IsNullOrWhiteSpace(order_id) || a.Order_id == order_id) &&
                                                 (isActive == null || a.IsActive == isActive) &&
                                                 (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                                 (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                                 (Fk_Subscription == 0 || a.Fk_Subscription == Fk_Subscription) &&
                                                 (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                                 (createdAtTo == null || a.CreatedAt <= createdAtTo));

        }

    }
}

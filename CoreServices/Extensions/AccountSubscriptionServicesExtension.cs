using Entities.CoreServicesModels.AccountModels;


namespace CoreServices.Extensions
{
    public static class AccountSubscriptionServicesSearchExtension
    {
        public static IQueryable<AccountSubscriptionModel> Search(this IQueryable<AccountSubscriptionModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AccountSubscriptionModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AccountSubscriptionModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class AccountSubscriptionServicesSortExtension
    {
        public static IQueryable<AccountSubscriptionModel> Sort(this IQueryable<AccountSubscriptionModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AccountSubscriptionModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

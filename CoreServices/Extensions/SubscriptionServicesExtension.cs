using Entities.CoreServicesModels.SubscriptionModels;

namespace CoreServices.Extensions
{
    public static class SubscriptionServicesSearchExtension
    {
        public static IQueryable<SubscriptionModel> Search(this IQueryable<SubscriptionModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<SubscriptionModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<SubscriptionModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class SubscriptionServicesSortExtension
    {
        public static IQueryable<SubscriptionModel> Sort(this IQueryable<SubscriptionModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<SubscriptionModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

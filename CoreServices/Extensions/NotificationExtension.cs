using Entities.CoreServicesModels.NotificationModels;
namespace CoreServices.Extensions
{
    public static class NotificationSearchExtension
    {
        public static IQueryable<NotificationModel> Search(this IQueryable<NotificationModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<NotificationModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<NotificationModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class NotificationSortExtension
    {
        public static IQueryable<NotificationModel> Sort(this IQueryable<NotificationModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<NotificationModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

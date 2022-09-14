using Entities.CoreServicesModels.LogModels;
namespace CoreServices.Extensions
{
    public static class LogServiceSearchExtension
    {
        public static IQueryable<LogModel> Search(this IQueryable<LogModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<LogModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<LogModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }


    }

    public static class LogServiceSortExtension
    {
        public static IQueryable<LogModel> Sort(this IQueryable<LogModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<LogModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }


    }
}

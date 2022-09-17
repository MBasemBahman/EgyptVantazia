using Entities.CoreServicesModels.StandingsModels;
namespace CoreServices.Extensions
{
    public static class StandingsServiceSearchExtension
    {
        public static IQueryable<StandingsModel> Search(this IQueryable<StandingsModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<StandingsModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<StandingsModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }


    }

    public static class StandingsServiceSortExtension
    {
        public static IQueryable<StandingsModel> Sort(this IQueryable<StandingsModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<StandingsModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }


    }
}

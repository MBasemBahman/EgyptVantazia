using Entities.CoreServicesModels.PrivateLeagueModels;

namespace CoreServices.Extensions
{
    public static class PrivateLeagueServiceSearchExtension
    {
        public static IQueryable<PrivateLeagueModel> Search(this IQueryable<PrivateLeagueModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PrivateLeagueModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PrivateLeagueModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PrivateLeagueMemberModel> Search(this IQueryable<PrivateLeagueMemberModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PrivateLeagueMemberModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PrivateLeagueMemberModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class PrivateLeagueServiceSortExtension
    {
        public static IQueryable<PrivateLeagueModel> Sort(this IQueryable<PrivateLeagueModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PrivateLeagueModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PrivateLeagueMemberModel> Sort(this IQueryable<PrivateLeagueMemberModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PrivateLeagueMemberModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

    }
}

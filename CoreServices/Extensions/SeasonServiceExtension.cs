using Entities.CoreServicesModels.SeasonModels;
namespace CoreServices.Extensions
{
    public static class SeasonServiceSearchExtension
    {
        public static IQueryable<SeasonModel> Search(this IQueryable<SeasonModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<SeasonModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<SeasonModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<GameWeakModel> Search(this IQueryable<GameWeakModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<GameWeakModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<GameWeakModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<TeamGameWeakModel> Search(this IQueryable<TeamGameWeakModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<TeamGameWeakModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<TeamGameWeakModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class SeasonServiceSortExtension
    {
        public static IQueryable<SeasonModel> Sort(this IQueryable<SeasonModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<SeasonModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<GameWeakModel> Sort(this IQueryable<GameWeakModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<GameWeakModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<TeamGameWeakModel> Sort(this IQueryable<TeamGameWeakModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<TeamGameWeakModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

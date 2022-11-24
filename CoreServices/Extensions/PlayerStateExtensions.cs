using Entities.CoreServicesModels.PlayerStateModels;

namespace CoreServices.Extensions
{
    public static class PlayerStateSearchExtension
    {
        public static IQueryable<PlayerGameWeakScoreStateModel> Search(this IQueryable<PlayerGameWeakScoreStateModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerGameWeakScoreStateModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerGameWeakScoreStateModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PlayerSeasonScoreStateModel> Search(this IQueryable<PlayerSeasonScoreStateModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerSeasonScoreStateModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerSeasonScoreStateModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<ScoreStateModel> Search(this IQueryable<ScoreStateModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<ScoreStateModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<ScoreStateModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class PlayerStateSortExtension
    {
        public static IQueryable<PlayerGameWeakScoreStateModel> Sort(this IQueryable<PlayerGameWeakScoreStateModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerGameWeakScoreStateModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PlayerSeasonScoreStateModel> Sort(this IQueryable<PlayerSeasonScoreStateModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data;
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerSeasonScoreStateModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<ScoreStateModel> Sort(this IQueryable<ScoreStateModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<ScoreStateModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

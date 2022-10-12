using Entities.CoreServicesModels.PlayerScoreModels;
namespace CoreServices.Extensions
{
    public static class PlayerScoreSearchExtension
    {
        public static IQueryable<ScoreTypeModel> Search(this IQueryable<ScoreTypeModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<ScoreTypeModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<ScoreTypeModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PlayerGameWeakModel> Search(this IQueryable<PlayerGameWeakModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerGameWeakModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerGameWeakModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PlayerGameWeakScoreModel> Search(this IQueryable<PlayerGameWeakScoreModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerGameWeakScoreModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerGameWeakScoreModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class PlayerScoreSortExtension
    {
        public static IQueryable<ScoreTypeModel> Sort(this IQueryable<ScoreTypeModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<ScoreTypeModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PlayerGameWeakModel> Sort(this IQueryable<PlayerGameWeakModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerGameWeakModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PlayerGameWeakScoreModel> Sort(this IQueryable<PlayerGameWeakScoreModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerGameWeakScoreModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

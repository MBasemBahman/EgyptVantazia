using Entities.CoreServicesModels.TeamModels;

namespace CoreServices.Extensions
{
    public static class TeamServiceSearchExtension
    {
        public static IQueryable<TeamModel> Search(this IQueryable<TeamModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<TeamModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<TeamModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PlayerPositionModel> Search(this IQueryable<PlayerPositionModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerPositionModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerPositionModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<FormationPositionModel> Search(this IQueryable<FormationPositionModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<FormationPositionModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<FormationPositionModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PlayerPriceModel> Search(this IQueryable<PlayerPriceModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerPriceModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerPriceModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PlayerModel> Search(this IQueryable<PlayerModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class TeamServiceSortExtension
    {
        public static IQueryable<TeamModel> Sort(this IQueryable<TeamModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<TeamModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PlayerPositionModel> Sort(this IQueryable<PlayerPositionModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerPositionModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<FormationPositionModel> Sort(this IQueryable<FormationPositionModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<FormationPositionModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PlayerModel> Sort(this IQueryable<PlayerModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PlayerPriceModel> Sort(this IQueryable<PlayerPriceModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerPriceModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

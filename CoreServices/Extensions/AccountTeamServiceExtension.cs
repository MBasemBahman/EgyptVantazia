using Entities.CoreServicesModels.AccountTeamModels;
namespace CoreServices.Extensions
{
    public static class AccountTeamServiceSearchExtension
    {
        public static IQueryable<AccountTeamGameWeakModel> Search(this IQueryable<AccountTeamGameWeakModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AccountTeamGameWeakModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AccountTeamGameWeakModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<AccountTeamModel> Search(this IQueryable<AccountTeamModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AccountTeamModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AccountTeamModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<AccountTeamPlayerGameWeakModel> Search(this IQueryable<AccountTeamPlayerGameWeakModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AccountTeamPlayerGameWeakModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AccountTeamPlayerGameWeakModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<AccountTeamPlayerModel> Search(this IQueryable<AccountTeamPlayerModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AccountTeamPlayerModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AccountTeamPlayerModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<TeamPlayerTypeModel> Search(this IQueryable<TeamPlayerTypeModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<TeamPlayerTypeModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<TeamPlayerTypeModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class AccountTeamServiceSortExtension
    {
        public static IQueryable<AccountTeamGameWeakModel> Sort(this IQueryable<AccountTeamGameWeakModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AccountTeamGameWeakModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<AccountTeamModel> Sort(this IQueryable<AccountTeamModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AccountTeamModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<AccountTeamPlayerGameWeakModel> Sort(this IQueryable<AccountTeamPlayerGameWeakModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AccountTeamPlayerGameWeakModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<AccountTeamPlayerModel> Sort(this IQueryable<AccountTeamPlayerModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AccountTeamPlayerModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<TeamPlayerTypeModel> Sort(this IQueryable<TeamPlayerTypeModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<TeamPlayerTypeModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

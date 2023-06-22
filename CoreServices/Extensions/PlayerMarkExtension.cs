using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.PlayerScoreModels;
namespace CoreServices.Extensions
{
    public static class PlayerMarkSearchExtension
    {
        public static IQueryable<MarkModel> Search(this IQueryable<MarkModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<MarkModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<MarkModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
        
        public static IQueryable<PlayerMarkModel> Search(this IQueryable<PlayerMarkModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerMarkModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerMarkModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
        
        public static IQueryable<PlayerMarkGameWeakModel> Search(this IQueryable<PlayerMarkGameWeakModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerMarkGameWeakModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerMarkGameWeakModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
        
        public static IQueryable<PlayerMarkGameWeakScoreModel> Search(this IQueryable<PlayerMarkGameWeakScoreModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerMarkGameWeakScoreModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerMarkGameWeakScoreModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
        
        public static IQueryable<PlayerMarkTeamGameWeakModel> Search(this IQueryable<PlayerMarkTeamGameWeakModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerMarkTeamGameWeakModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerMarkTeamGameWeakModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class PlayerMarkSortExtension
    {
        public static IQueryable<MarkModel> Sort(this IQueryable<MarkModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<MarkModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
        
        public static IQueryable<PlayerMarkModel> Sort(this IQueryable<PlayerMarkModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerMarkModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
        
        public static IQueryable<PlayerMarkGameWeakModel> Sort(this IQueryable<PlayerMarkGameWeakModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerMarkGameWeakModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
        
        public static IQueryable<PlayerMarkGameWeakScoreModel> Sort(this IQueryable<PlayerMarkGameWeakScoreModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerMarkGameWeakScoreModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
        
        public static IQueryable<PlayerMarkTeamGameWeakModel> Sort(this IQueryable<PlayerMarkTeamGameWeakModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerMarkTeamGameWeakModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

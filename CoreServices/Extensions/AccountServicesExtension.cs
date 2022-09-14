using Entities.CoreServicesModels.AccountModels;


namespace CoreServices.Extensions
{
    public static class AccountServicesSearchExtension
    {
        public static IQueryable<AccountModel> Search(this IQueryable<AccountModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AccountModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AccountModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class AccountServicesSortExtension
    {
        public static IQueryable<AccountModel> Sort(this IQueryable<AccountModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AccountModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

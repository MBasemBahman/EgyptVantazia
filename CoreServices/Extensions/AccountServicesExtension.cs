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

        public static IQueryable<AccountSubscriptionModel> Search(this IQueryable<AccountSubscriptionModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AccountSubscriptionModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AccountSubscriptionModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PaymentModel> Search(this IQueryable<PaymentModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PaymentModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PaymentModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<AccountRefCodeModel> Search(this IQueryable<AccountRefCodeModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AccountRefCodeModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AccountRefCodeModel>(searchColumns, searchTerm);

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

        public static IQueryable<AccountSubscriptionModel> Sort(this IQueryable<AccountSubscriptionModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AccountSubscriptionModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PaymentModel> Sort(this IQueryable<PaymentModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PaymentModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<AccountRefCodeModel> Sort(this IQueryable<AccountRefCodeModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AccountRefCodeModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

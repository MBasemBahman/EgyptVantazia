using Entities.CoreServicesModels.UserModels;

namespace CoreServices.Extensions
{
    public static class UserServiceSearchExtension
    {
        public static IQueryable<UserModel> Search(this IQueryable<UserModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<UserModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<UserModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<UserDeviceModel> Search(this IQueryable<UserDeviceModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<UserDeviceModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<UserDeviceModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<RefreshTokenModel> Search(this IQueryable<RefreshTokenModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<RefreshTokenModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<RefreshTokenModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<VerificationModel> Search(this IQueryable<VerificationModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<VerificationModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<VerificationModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }


    }

    public static class UserServiceSortExtension
    {
        public static IQueryable<UserModel> Sort(this IQueryable<UserModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<UserModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<UserDeviceModel> Sort(this IQueryable<UserDeviceModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<UserDeviceModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<RefreshTokenModel> Sort(this IQueryable<RefreshTokenModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<RefreshTokenModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<VerificationModel> Sort(this IQueryable<VerificationModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<VerificationModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

    }
}

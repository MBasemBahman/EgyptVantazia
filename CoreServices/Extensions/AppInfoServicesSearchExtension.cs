using Entities.CoreServicesModels.AppInfoModels;
using Entities.CoreServicesModels.LocationModels;


namespace CoreServices.Extensions
{
    public static class AppInfoServicesSearchExtension
    {
        public static IQueryable<AppAboutModel> Search(this IQueryable<AppAboutModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<CountryModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<CountryModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

    }

    public static class AppInfoServicesSortExtension
    {
        public static IQueryable<AppAboutModel> Sort(this IQueryable<AppAboutModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AppAboutModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

    }
}

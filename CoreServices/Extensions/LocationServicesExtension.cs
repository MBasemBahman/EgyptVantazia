using Entities.CoreServicesModels.LocationModels;


namespace CoreServices.Extensions
{
    public static class LocationServicesSearchExtension
    {
        public static IQueryable<CountryModel> Search(this IQueryable<CountryModel> data, string searchColumns, string searchTerm)
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

    public static class LocationServicesSortExtension
    {
        public static IQueryable<CountryModel> Sort(this IQueryable<CountryModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<CountryModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

    }
}

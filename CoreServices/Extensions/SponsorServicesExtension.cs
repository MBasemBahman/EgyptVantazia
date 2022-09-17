using Entities.CoreServicesModels.SponsorModels;

namespace CoreServices.Extensions
{
    public static class SponsorServicesSearchExtension
    {
        public static IQueryable<SponsorModel> Search(this IQueryable<SponsorModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<SponsorModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<SponsorModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<SponsorViewModel> Search(this IQueryable<SponsorViewModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<SponsorViewModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<SponsorViewModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }
    }

    public static class SponsorServicesSortExtension
    {
        public static IQueryable<SponsorModel> Sort(this IQueryable<SponsorModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<SponsorModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<SponsorViewModel> Sort(this IQueryable<SponsorViewModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<SponsorViewModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

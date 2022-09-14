using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.NewsModels;

namespace CoreServices.Extensions
{
    public static class NewsServicesSearchExtension
    {
        public static IQueryable<NewsModel> Search(this IQueryable<NewsModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<NewsModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<NewsModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<NewsAttachmentModel> Search(this IQueryable<NewsAttachmentModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<NewsAttachmentModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<NewsAttachmentModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

    }

    public static class NewsServicesSortExtension
    {
        public static IQueryable<NewsModel> Sort(this IQueryable<NewsModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<NewsModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
        public static IQueryable<NewsAttachmentModel> Sort(this IQueryable<NewsAttachmentModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<NewsAttachmentModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }
}

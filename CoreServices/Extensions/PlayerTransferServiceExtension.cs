using Entities.CoreServicesModels.PlayerTransfersModels;

namespace CoreServices.Extensions
{
    public static class PlayerTransferServiceSearchExtension
    {
        public static IQueryable<PlayerTransferModel> Search(this IQueryable<PlayerTransferModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PlayerTransferModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PlayerTransferModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }


    }

    public static class PlayerTransferServiceSortExtension
    {
        public static IQueryable<PlayerTransferModel> Sort(this IQueryable<PlayerTransferModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PlayerTransferModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }


    }
}

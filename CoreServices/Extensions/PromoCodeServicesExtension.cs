using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.PromoCodeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServices.Extensions
{
    public static class PromoCodeServicesSearchExtension
    {
        public static IQueryable<PromoCodeModel> Search(this IQueryable<PromoCodeModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PromoCodeModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PromoCodeModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<PromoCodeSubscriptionModel> Search(this IQueryable<PromoCodeSubscriptionModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<PromoCodeSubscriptionModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<PromoCodeSubscriptionModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

    }


    public static class PromoCodeServicesSortExtension
    {
        public static IQueryable<PromoCodeModel> Sort(this IQueryable<PromoCodeModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PromoCodeModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<PromoCodeSubscriptionModel> Sort(this IQueryable<PromoCodeSubscriptionModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<PromoCodeSubscriptionModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

    }
}

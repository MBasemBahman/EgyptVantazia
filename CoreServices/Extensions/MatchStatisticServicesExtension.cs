using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.MatchStatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServices.Extensions
{
    public static class MatchStatisticServicesSearchExtension
    {
        public static IQueryable<MatchStatisticScoreModel> Search(this IQueryable<MatchStatisticScoreModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<MatchStatisticScoreModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<MatchStatisticScoreModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<StatisticScoreModel> Search(this IQueryable<StatisticScoreModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<StatisticScoreModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<StatisticScoreModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<StatisticCategoryModel> Search(this IQueryable<StatisticCategoryModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<StatisticCategoryModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<StatisticCategoryModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

    }


    public static class MatchStatisticServicesSortExtension
    {
        public static IQueryable<MatchStatisticScoreModel> Sort(this IQueryable<MatchStatisticScoreModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<MatchStatisticScoreModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<StatisticScoreModel> Sort(this IQueryable<StatisticScoreModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<StatisticScoreModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }


        public static IQueryable<StatisticCategoryModel> Sort(this IQueryable<StatisticCategoryModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<StatisticCategoryModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

    }
}

using Entities.CoreServicesModels.DashboardAdministrationModels;

namespace CoreServices.Extensions
{

    public static class DashboardAdministrationServicesSearchExtension
    {
        public static IQueryable<DashboardViewModel> Search(this IQueryable<DashboardViewModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<DashboardViewModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<DashboardViewModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<DashboardAccessLevelModel> Search(this IQueryable<DashboardAccessLevelModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<DashboardAccessLevelModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<DashboardAccessLevelModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<DashboardAdministrationRoleModel> Search(this IQueryable<DashboardAdministrationRoleModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<DashboardAdministrationRoleModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<DashboardAdministrationRoleModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<DashboardAdministratorModel> Search(this IQueryable<DashboardAdministratorModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<DashboardAdministratorModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<DashboardAdministratorModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<AdministrationRolePremissionModel> Search(this IQueryable<AdministrationRolePremissionModel> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
            {
                return data;
            }

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<AdministrationRolePremissionModel, bool>> expression = SearchQueryBuilder.CreateSearchQuery<AdministrationRolePremissionModel>(searchColumns, searchTerm);

            return data.Where(expression);
        }

    }

    public static class DashboardAdministrationServicesSortExtension
    {
        public static IQueryable<DashboardViewModel> Sort(this IQueryable<DashboardViewModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<DashboardViewModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<DashboardAccessLevelModel> Sort(this IQueryable<DashboardAccessLevelModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<DashboardAccessLevelModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<DashboardAdministrationRoleModel> Sort(this IQueryable<DashboardAdministrationRoleModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<DashboardAdministrationRoleModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<DashboardAdministratorModel> Sort(this IQueryable<DashboardAdministratorModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<DashboardAdministratorModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }

        public static IQueryable<AdministrationRolePremissionModel> Sort(this IQueryable<AdministrationRolePremissionModel> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return data.OrderBy(a => a.Id);
            }

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<AdministrationRolePremissionModel>(orderByQueryString);

            return data.OrderBy(orderQuery);
        }
    }


}

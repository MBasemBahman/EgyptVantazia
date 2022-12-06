using Entities.CoreServicesModels.DashboardAdministrationModels;
using Entities.DBModels.DashboardAdministrationModels;

namespace Repository.DBModels.DashboardAdministrationModels
{
    public class AdministrationRolePremissionRepository : RepositoryBase<AdministrationRolePremission>
    {
        public AdministrationRolePremissionRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<AdministrationRolePremission> FindAll(AdministrationRolePremissionParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_DashboardAdministrationRole,
                           parameters.Fk_DashboardAccessLevel,
                           parameters.Fk_DashboardView);
        }

        public async Task<AdministrationRolePremission> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }
    }

    public static class AdministrationRolePremissionRepositoryExtension
    {
        public static IQueryable<AdministrationRolePremission> Filter(
            this IQueryable<AdministrationRolePremission> AdministrationRolePremissions,
            int id,
            int Fk_DashboardAdministrationRole,
            int Fk_DashboardAccessLevel,
            int Fk_DashboardView)
        {
            return AdministrationRolePremissions.Where(a => (id == 0 || a.Id == id) &&
                                                            (Fk_DashboardAdministrationRole == 0 || a.Fk_DashboardAdministrationRole == Fk_DashboardAdministrationRole) &&
                                                            (Fk_DashboardAccessLevel == 0 || a.Fk_DashboardAccessLevel == Fk_DashboardAccessLevel) &&
                                                            (Fk_DashboardView == 0 || a.Fk_DashboardView == Fk_DashboardView));
        }
    }
}

using Entities.CoreServicesModels.DashboardAdministrationModels;
using Entities.DBModels.DashboardAdministrationModels;
using static Contracts.EnumData.DBModelsEnum;

namespace Repository.DBModels.DashboardAdministrationModels
{
    public class DashboardAdministratorRepository : RepositoryBase<DashboardAdministrator>
    {
        public DashboardAdministratorRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<DashboardAdministrator> FindAll(DashboardAdministratorParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_DashboardAdministrationRole,
                           parameters.GetDevelopers);
        }

        public async Task<DashboardAdministrator> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public async Task<DashboardAdministrator> FindByUserId(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Fk_User == id, trackChanges)
                        .FirstOrDefaultAsync();
        }
    }

    public static class DashboardAdministratorRepositoryExtension
    {
        public static IQueryable<DashboardAdministrator> Filter(this IQueryable<DashboardAdministrator> DashboardAdministrators,
            int id,
            int Fk_DashboardAdministrationRole,
            bool getDevelopers = true)
        {
            return DashboardAdministrators.Where(a => (id == 0 || a.Id == id) &&
                                                      (Fk_DashboardAdministrationRole == 0 || a.Fk_DashboardAdministrationRole == Fk_DashboardAdministrationRole) &&
                                                      (getDevelopers == true || a.Fk_DashboardAdministrationRole != (int)DashboardAdministrationRoleEnum.Developer));
        }
    }
}

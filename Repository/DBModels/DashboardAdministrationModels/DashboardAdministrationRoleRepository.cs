using Entities.CoreServicesModels.DashboardAdministrationModels;
using Entities.DBModels.DashboardAdministrationModels;
using static Contracts.EnumData.DBModelsEnum;

namespace Repository.DBModels.DashboardAdministrationModels
{
    public class DashboardAdministrationRoleRepository : RepositoryBase<DashboardAdministrationRole>
    {
        public DashboardAdministrationRoleRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<DashboardAdministrationRole> FindAll(DashboardAdministrationRoleRequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.GetDeveloperRole);
        }

        public async Task<DashboardAdministrationRole> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.DashboardAdministrationRoleLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(DashboardAdministrationRole entity)
        {
            entity.DashboardAdministrationRoleLang ??= new DashboardAdministrationRoleLang
            {
                Name = entity.Name
            };
            base.Create(entity);
        }
    }

    public static class DashboardAdministrationRoleRepositoryExtension
    {
        public static IQueryable<DashboardAdministrationRole> Filter(this IQueryable<DashboardAdministrationRole> DashboardAdministrationRoles,
            int id,
            bool getDeveloperRole = true)
        {
            return DashboardAdministrationRoles.Where(a => (id == 0 || a.Id == id) &&
                                                           (getDeveloperRole == true || a.Id != (int)DashboardAdministrationRoleEnum.Developer));
        }
    }
}

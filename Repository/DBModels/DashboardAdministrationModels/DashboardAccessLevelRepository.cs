using Entities.CoreServicesModels.DashboardAdministrationModels;
using Entities.DBModels.DashboardAdministrationModels;

namespace Repository.DBModels.DashboardAdministrationModels
{
    public class DashboardAccessLevelRepository : RepositoryBase<DashboardAccessLevel>
    {
        public DashboardAccessLevelRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<DashboardAccessLevel> FindAll(DashboardAccessLevelParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id, parameters.Fk_DashboardAdministrationRole,
                           parameters.Fk_DashboardView);
        }

        public async Task<DashboardAccessLevel> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.DashboardAccessLevelLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(DashboardAccessLevel entity)
        {
            entity.DashboardAccessLevelLang ??= new DashboardAccessLevelLang
            {
                Name = entity.Name
            };
            base.Create(entity);
        }
    }

    public static class DashboardAccessLevelRepositoryExtension
    {
        public static IQueryable<DashboardAccessLevel> Filter(this IQueryable<DashboardAccessLevel> DashboardAccessLevels,
            int id, int fk_DashboardAdministrationRole,
            int fk_DashboardView)
        {
            return DashboardAccessLevels.Where(a => (id == 0 || a.Id == id) &&
                                               (fk_DashboardAdministrationRole == 0 || a.Premissions.Any(b => b.Fk_DashboardAdministrationRole == fk_DashboardAdministrationRole)) &&
                                               (fk_DashboardAdministrationRole == 0 || a.Premissions.Any(b => b.Fk_DashboardView == fk_DashboardView)));
        }
    }
}

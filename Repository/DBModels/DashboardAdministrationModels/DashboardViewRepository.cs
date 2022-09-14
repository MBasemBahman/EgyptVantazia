using Entities.CoreServicesModels.DashboardAdministrationModels;
using Entities.DBModels.DashboardAdministrationModels;

namespace Repository.DBModels.DashboardAdministrationModels
{
    public class DashboardViewRepository : RepositoryBase<DashboardView>
    {
        public DashboardViewRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<DashboardView> FindAll(DashboardViewParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id, parameters.Fk_Role);
        }

        public async Task<DashboardView> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.DashboardViewLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(DashboardView entity)
        {
            entity.DashboardViewLang ??= new DashboardViewLang
            {
                Name = entity.Name
            };
            base.Create(entity);
        }
    }

    public static class DashboardViewRepositoryExtension
    {
        public static IQueryable<DashboardView> Filter(
            this IQueryable<DashboardView> DashboardViews,
            int id,
            int fk_Role)
        {
            return DashboardViews.Where(a => (id == 0 || a.Id == id) &&
                                             (fk_Role == 0 || a.Premissions.Any(b => b.Fk_DashboardAdministrationRole == fk_Role)));
        }
    }
}

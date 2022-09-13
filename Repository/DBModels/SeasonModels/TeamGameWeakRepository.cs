using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.SeasonModels
{
    public class TeamGameWeakRepository : RepositoryBase<TeamGameWeak>
    {
        public TeamGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<TeamGameWeak> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<TeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(TeamGameWeak entity)
        {
            base.Create(entity);
        }

        public new void Delete(TeamGameWeak entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class TeamGameWeakRepositoryExtension
    {
        public static IQueryable<TeamGameWeak> Filter(this IQueryable<TeamGameWeak> TeamGameWeaks, int id)
        {
            return TeamGameWeaks.Where(a => id == 0 || a.Id == id);
        }

    }
}

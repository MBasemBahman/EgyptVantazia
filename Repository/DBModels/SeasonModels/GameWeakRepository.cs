using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.SeasonModels
{
    public class GameWeakRepository : RepositoryBase<GameWeak>
    {
        public GameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<GameWeak> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<GameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(GameWeak entity)
        {
            base.Create(entity);
        }

        public new void Delete(GameWeak entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class GameWeakRepositoryExtension
    {
        public static IQueryable<GameWeak> Filter(this IQueryable<GameWeak> GameWeaks, int id)
        {
            return GameWeaks.Where(a => id == 0 || a.Id == id);
        }

    }
}

using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.PlayerScoreModels
{
    public class PlayerGameWeakRepository : RepositoryBase<PlayerGameWeak>
    {
        public PlayerGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeak> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<PlayerGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PlayerGameWeak entity)
        {
            base.Create(entity);
        }

        public new void Delete(PlayerGameWeak entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class PlayerGameWeakRepositoryExtension
    {
        public static IQueryable<PlayerGameWeak> Filter(this IQueryable<PlayerGameWeak> PlayerGameWeaks, int id)
        {
            return PlayerGameWeaks.Where(a => id == 0 || a.Id == id);
        }

    }
}

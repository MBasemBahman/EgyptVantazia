using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.PlayerScoreModels
{
    public class PlayerGameWeakScoreRepository : RepositoryBase<PlayerGameWeakScore>
    {
        public PlayerGameWeakScoreRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeakScore> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<PlayerGameWeakScore> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PlayerGameWeakScore entity)
        {
            base.Create(entity);
        }

        public new void Delete(PlayerGameWeakScore entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class PlayerGameWeakScoreRepositoryExtension
    {
        public static IQueryable<PlayerGameWeakScore> Filter(this IQueryable<PlayerGameWeakScore> PlayerGameWeakScores, int id)
        {
            return PlayerGameWeakScores.Where(a => id == 0 || a.Id == id);
        }

    }
}

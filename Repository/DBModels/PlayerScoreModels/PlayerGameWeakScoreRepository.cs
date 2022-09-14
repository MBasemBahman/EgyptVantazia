using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.PlayerScoreModels
{
    public class PlayerGameWeakScoreRepository : RepositoryBase<PlayerGameWeakScore>
    {
        public PlayerGameWeakScoreRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeakScore> FindAll(PlayerGameWeakScoreParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_PlayerGameWeak,
                           parameters.Fk_ScoreType);
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
        public static IQueryable<PlayerGameWeakScore> Filter
            (this IQueryable<PlayerGameWeakScore> PlayerGameWeakScores,
             int id,
             int Fk_PlayerGameWeak,
             int Fk_ScoreType)

        {
            return PlayerGameWeakScores.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_PlayerGameWeak == 0 || a.Fk_PlayerGameWeak == Fk_PlayerGameWeak) &&
                                                   (Fk_ScoreType == 0 || a.Fk_ScoreType == Fk_ScoreType));

        }

    }
}

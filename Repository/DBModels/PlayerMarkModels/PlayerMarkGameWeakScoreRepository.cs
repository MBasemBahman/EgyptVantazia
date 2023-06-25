using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.DBModels.PlayerMarkModels;

namespace Repository.DBModels.PlayerMarkModels
{
    public class PlayerMarkGameWeakScoreRepository : RepositoryBase<PlayerMarkGameWeakScore>
    {
        public PlayerMarkGameWeakScoreRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerMarkGameWeakScore> FindAll(PlayerMarkGameWeakScoreParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_PlayerMark,
                           parameters.Fk_PlayerGameWeakScore);
        }

        public async Task<PlayerMarkGameWeakScore> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PlayerMarkGameWeakScore entity)
        {
            base.Create(entity);
        }
    }

    public static class PlayerMarkGameWeakScoreRepositoryExtension
    {
        public static IQueryable<PlayerMarkGameWeakScore> Filter(
            this IQueryable<PlayerMarkGameWeakScore> PlayerMarkGameWeakScores,
            int id,
            int fk_PlayerMark,
            int fk_PlayerGameWeakScore)
        {
            return PlayerMarkGameWeakScores.Where(a => (id == 0 || a.Id == id) &&
                                    (fk_PlayerMark == 0 || a.Fk_PlayerMark == fk_PlayerMark) &&
                                    (fk_PlayerGameWeakScore == 0 || a.Fk_PlayerGameWeakScore == fk_PlayerGameWeakScore) );
        }

    }
}

using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;


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
                           parameters.Fk_ScoreType,
                           parameters.Fk_Player,
                           parameters.Fk_GameWeak);
        }

        public async Task<PlayerGameWeakScore> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }
    }

    public static class PlayerGameWeakScoreRepositoryExtension
    {
        public static IQueryable<PlayerGameWeakScore> Filter
            (this IQueryable<PlayerGameWeakScore> PlayerGameWeakScores,
             int id,
             int Fk_PlayerGameWeak,
             int Fk_ScoreType,
             int Fk_Player,
             int Fk_GameWeak)

        {
            return PlayerGameWeakScores.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_PlayerGameWeak == 0 || a.Fk_PlayerGameWeak == Fk_PlayerGameWeak) &&
                                                   (Fk_Player == 0 || a.PlayerGameWeak.Fk_Player == Fk_Player) &&
                                                   (Fk_GameWeak == 0 || a.PlayerGameWeak.TeamGameWeak.Fk_GameWeak == Fk_GameWeak) &&
                                                   (Fk_ScoreType == 0 || a.Fk_ScoreType == Fk_ScoreType));

        }

    }
}

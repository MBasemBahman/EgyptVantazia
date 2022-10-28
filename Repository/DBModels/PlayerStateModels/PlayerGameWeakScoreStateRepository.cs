using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PlayerStateModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.PlayerStateModels
{
    public class PlayerGameWeakScoreStateRepository : RepositoryBase<PlayerGameWeakScoreState>
    {
        public PlayerGameWeakScoreStateRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeakScoreState> FindAll(PlayerGameWeakScoreStateParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Player,
                           parameters.Fk_ScoreState,
                           parameters.Fk_Players,
                           parameters.Fk_ScoreStates,
                           parameters.Fk_GameWeaks);

        }

        public async Task<PlayerGameWeakScoreState> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

    }

    public static class PlayerGameWeakScoreStateRepositoryExtension
    {
        public static IQueryable<PlayerGameWeakScoreState> Filter(
            this IQueryable<PlayerGameWeakScoreState> PlayerGameWeakScoreStates,
            int id,
            int fk_GameWeak,
            int fk_Player,
            int fk_ScoreState,
            List<int> fk_Players,
            List<int> fk_ScoreStates,
            List<int> fk_GameWeaks)
        {
            return PlayerGameWeakScoreStates.Where(a => (id == 0 || a.Id == id) &&
                                                  (fk_GameWeak == 0 || a.Fk_GameWeak == fk_GameWeak) &&
                                                  (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                                  (fk_ScoreState == 0 || a.Fk_ScoreState == fk_ScoreState) &&
                                                  (fk_Players == null || !fk_Players.Any() ||
                                                fk_Players.Contains(a.Fk_Player)) &&
                                                  (fk_ScoreStates == null || !fk_ScoreStates.Any() ||
                                                fk_ScoreStates.Contains(a.Fk_ScoreState)) &&
                                                  (fk_GameWeaks == null || !fk_GameWeaks.Any() ||
                                                fk_GameWeaks.Contains(a.Fk_GameWeak)));


        }

    }
}





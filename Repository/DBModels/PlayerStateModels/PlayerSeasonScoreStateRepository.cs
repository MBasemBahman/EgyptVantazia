using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.PlayerStateModels
{
    public class PlayerSeasonScoreStateRepository : RepositoryBase<PlayerSeasonScoreState>
    {
        public PlayerSeasonScoreStateRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerSeasonScoreState> FindAll(PlayerSeasonScoreStateParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Player,
                           parameters.Fk_ScoreState,
                           parameters.Fk_Season,
                           parameters.Fk_Players,
                           parameters.Fk_ScoreStates,
                           parameters.Fk_Seasons);

        }

        public async Task<PlayerSeasonScoreState> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

    }

    public static class PlayerSeasonScoreStateRepositoryExtension
    {
        public static IQueryable<PlayerSeasonScoreState> Filter(
            this IQueryable<PlayerSeasonScoreState> PlayerSeasonScoreStates,
            int id,
            int fk_Player,
            int fk_ScoreState,
            int fk_Season,
            List<int> fk_Players,
            List<int> fk_ScoreStates,
            List<int> fk_Seasons)
        {
            return PlayerSeasonScoreStates.Where(a => (id == 0 || a.Id == id) &&

                                                  (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                                  (fk_ScoreState == 0 || a.Fk_ScoreState == fk_ScoreState) &&
                                                  (fk_Season == 0 || a.Fk_Season == fk_Season) &&
                                                  (fk_Players == null || !fk_Players.Any() ||
                                                fk_Players.Contains(a.Fk_Player)) &&
                                                  (fk_ScoreStates == null || !fk_ScoreStates.Any() ||
                                                fk_ScoreStates.Contains(a.Fk_ScoreState)) &&
                                                  (fk_Seasons == null || !fk_Seasons.Any() ||
                                                fk_Seasons.Contains(a.Fk_Season)));


        }

    }
}

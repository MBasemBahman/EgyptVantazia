using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.PlayerStateModels;
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

        public new void Create(PlayerGameWeakScoreState entity)
        {
            if (FindByCondition(a => a.Fk_Player == entity.Fk_Player && a.Fk_GameWeak == entity.Fk_GameWeak && a.Fk_ScoreState == entity.Fk_ScoreState, trackChanges: false).Any())
            {
                PlayerGameWeakScoreState oldEntity = FindByCondition(a => a.Fk_Player == entity.Fk_Player && a.Fk_GameWeak == entity.Fk_GameWeak && a.Fk_ScoreState == entity.Fk_ScoreState, trackChanges: true).First();

                oldEntity.Points = entity.Points;
                oldEntity.PositionByPoints = entity.PositionByPoints;
                oldEntity.Value = entity.Value;
                oldEntity.PositionByValue = entity.PositionByValue;
                oldEntity.Percent = entity.Percent;
                oldEntity.PositionByPercent = entity.PositionByPercent;
            }
            else
            {
                base.Create(entity);
            }
        }

        public void UpdatePlayerGameWeakScoreStatePosition(int fk_GameWeak, int fk_ScoreState)
        {
            List<PlayerGameWeakScoreState> scores = FindAll(new PlayerGameWeakScoreStateParameters
            {
                Fk_GameWeak = fk_GameWeak,
                Fk_ScoreState = fk_ScoreState
            }, trackChanges: true).ToList();

            double[] pointsRanks = scores.Select(a => a.Points).Distinct().OrderByDescending(a => a).ToArray();
            double[] valueRanks = scores.Select(a => a.Value).Distinct().OrderByDescending(a => a).ToArray();
            double[] percentRanks = scores.Select(a => a.Percent).Distinct().OrderByDescending(a => a).ToArray();

            scores.ForEach(a =>
            {
                a.PositionByPoints = Array.IndexOf(pointsRanks, a.Points) + 1;
                a.PositionByValue = Array.IndexOf(valueRanks, a.Value) + 1;
                a.PositionByPercent = Array.IndexOf(percentRanks, a.Percent) + 1;
            });
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





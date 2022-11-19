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
                           parameters.Fk_Season,
                           parameters.Fk_Player,
                           parameters.Fk_ScoreState,
                           parameters.Fk_Players,
                           parameters.Fk_ScoreStates,
                           parameters.Fk_GameWeaks,
                           parameters.Fk_PlayerPosition,
                           parameters.PointsFrom,
                           parameters.PointsTo,
                           parameters.PercentFrom,
                           parameters.PercentTo,
                           parameters.ValueFrom,
                           parameters.ValueTo,
                           parameters.IsTop15);

        }

        public async Task<PlayerGameWeakScoreState> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public void ResetTop15(int fk_GameWeak)
        {
            var players = FindAll(new PlayerGameWeakScoreStateParameters
            {
                Fk_GameWeak = fk_GameWeak,
                IsTop15 = true
            }, trackChanges: true).ToList();

            players.ForEach(a => a.Top15 = null);
        }

        public new void Create(PlayerGameWeakScoreState entity)
        {
            if (entity.Points == 0 && entity.Value == 0)
            {
                return;
            }
            if (FindByCondition(a => a.Fk_Player == entity.Fk_Player && a.Fk_GameWeak == entity.Fk_GameWeak && a.Fk_ScoreState == entity.Fk_ScoreState, trackChanges: false).Any())
            {
                PlayerGameWeakScoreState oldEntity = FindByCondition(a => a.Fk_Player == entity.Fk_Player && a.Fk_GameWeak == entity.Fk_GameWeak && a.Fk_ScoreState == entity.Fk_ScoreState, trackChanges: true).First();

                oldEntity.Points = entity.Points;
                oldEntity.Value = entity.Value;
                oldEntity.Percent = entity.Percent;
            }
            else
            {
                base.Create(entity);
            }
        }

    }

    public static class PlayerGameWeakScoreStateRepositoryExtension
    {
        public static IQueryable<PlayerGameWeakScoreState> Filter(
            this IQueryable<PlayerGameWeakScoreState> PlayerGameWeakScoreStates,
            int id,
            int? fk_GameWeak,
            int? Fk_Season,
            int fk_Player,
            int fk_ScoreState,
            List<int> fk_Players,
            List<int> fk_ScoreStates,
            List<int> fk_GameWeaks,
            int fk_PlayerPosition,
            double? pointsFrom,
            double? pointsTo,
            double? percentFrom,
            double? percentTo,
            double? valueFrom,
            double? valueTo,
            bool? IsTop15)
        {
            return PlayerGameWeakScoreStates.Where(a => (id == 0 || a.Id == id) &&
                                                  (IsTop15 == null || (IsTop15 == true ? a.Top15 != null : a.Top15 == null)) &&
                                                  (Fk_Season == null || a.GameWeak.Fk_Season == Fk_Season) &&
                                                  (fk_GameWeak == null || a.Fk_GameWeak == fk_GameWeak) &&
                                                  (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                                  (fk_PlayerPosition == 0 || a.Player.Fk_PlayerPosition == fk_PlayerPosition) &&
                                                  (fk_ScoreState == 0 || a.Fk_ScoreState == fk_ScoreState) &&
                                                  (fk_Players == null || !fk_Players.Any() ||
                                                   fk_Players.Contains(a.Fk_Player)) &&
                                                  (fk_ScoreStates == null || !fk_ScoreStates.Any() ||
                                                   fk_ScoreStates.Contains(a.Fk_ScoreState)) &&
                                                  (fk_GameWeaks == null || !fk_GameWeaks.Any() ||
                                                   fk_GameWeaks.Contains(a.Fk_GameWeak)) &&
                                                  (pointsFrom == null || a.Points >= pointsFrom) &&
                                                  (pointsTo == null || a.Points <= pointsTo) &&
                                                  (percentFrom == null || a.Percent >= percentFrom) &&
                                                  (percentTo == null || a.Percent <= percentTo) &&
                                                  (valueFrom == null || a.Value >= valueFrom) &&
                                                  (valueTo == null || a.Value <= valueTo));


        }

    }
}





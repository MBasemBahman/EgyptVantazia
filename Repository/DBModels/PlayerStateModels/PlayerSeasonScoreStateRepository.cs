using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.PlayerStateModels;
using Microsoft.Data.SqlClient;

namespace Repository.DBModels.PlayerStateModels
{
    public class PlayerSeasonScoreStateRepository : RepositoryBase<PlayerSeasonScoreState>
    {
        public PlayerSeasonScoreStateRepository(BaseContext context) : base(context)
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
                           parameters.Fk_Seasons,
                           parameters.Fk_PlayerPosition,
                           parameters.PointsFrom,
                           parameters.PointsTo,
                           parameters.PercentFrom,
                           parameters.PercentTo,
                           parameters.IsTop15,
                           parameters.DashboardSearch);

        }

        public async Task<PlayerSeasonScoreState> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public void UpdateTop15(int fk_Season)
        {
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdatePlayerSeasonScoreStateTop15] @SeasonId", new SqlParameter("@SeasonId", fk_Season));
        }

        public new void Create(PlayerSeasonScoreState entity)
        {
            if (entity.Points == 0 && entity.Value == 0)
            {
                return;
            }
            if (FindByCondition(a => a.Fk_Player == entity.Fk_Player && a.Fk_Season == entity.Fk_Season && a.Fk_ScoreState == entity.Fk_ScoreState, trackChanges: false).Any())
            {
                PlayerSeasonScoreState oldEntity = FindByCondition(a => a.Fk_Player == entity.Fk_Player && a.Fk_Season == entity.Fk_Season && a.Fk_ScoreState == entity.Fk_ScoreState, trackChanges: true).First();

                oldEntity.Points = entity.Points;
                oldEntity.Value = entity.Value;
                oldEntity.Percent = entity.Percent;
            }
            else
            {
                base.Create(entity);
            }
        }

        public void DeleteOldPlayerScores(int fk_Player, int fk_Season)
        {
            List<PlayerSeasonScoreState> data = FindByCondition(a => a.Fk_Player == fk_Player && a.Fk_Season == fk_Season, trackChanges: true).ToList();
            DBContext.PlayerSeasonScoreStates.RemoveRange(data);
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
            List<int> fk_Seasons,
            int fk_PlayerPosition,
            double? pointsFrom,
            double? pointsTo,
            double? percentFrom,
            double? percentTo,
            bool? IsTop15,
            string dashboardSearch)
        {
            return PlayerSeasonScoreStates.Where(a => (id == 0 || a.Id == id) &&

                                                  (string.IsNullOrEmpty(dashboardSearch) ||
                                                       a.Id.ToString().Contains(dashboardSearch) ||
                                                       a.Player.Name.Contains(dashboardSearch) ||
                                                       a.Season.Name.Contains(dashboardSearch) ||
                                                       a.ScoreState.Name.Contains(dashboardSearch)) &&

                                                  (IsTop15 == null || (IsTop15 == true ? a.Top15 != null : a.Top15 == null)) &&
                                                  (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                                  (fk_ScoreState == 0 || a.Fk_ScoreState == fk_ScoreState) &&
                                                  (fk_PlayerPosition == 0 || a.Player.Fk_PlayerPosition == fk_PlayerPosition) &&
                                                  (fk_Season == 0 || a.Fk_Season == fk_Season) &&
                                                  (fk_Players == null || !fk_Players.Any() ||
                                                   fk_Players.Contains(a.Fk_Player)) &&
                                                  (fk_ScoreStates == null || !fk_ScoreStates.Any() ||
                                                   fk_ScoreStates.Contains(a.Fk_ScoreState)) &&
                                                  (fk_Seasons == null || !fk_Seasons.Any() ||
                                                   fk_Seasons.Contains(a.Fk_Season)) &&
                                                  (pointsFrom == null || a.Points >= pointsFrom) &&
                                                  (pointsTo == null || a.Points <= pointsTo) &&
                                                  (percentFrom == null || a.Percent >= percentFrom) &&
                                                  (percentTo == null || a.Percent <= percentTo));


        }

    }
}

using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;
using Microsoft.Data.SqlClient;

namespace Repository.DBModels.PlayerScoreModels
{
    public class PlayerGameWeakRepository : RepositoryBase<PlayerGameWeak>
    {
        public PlayerGameWeakRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeak> FindAll(PlayerGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_TeamGameWeak,
                           parameters.Fk_Home,
                           parameters.Fk_Away,
                           parameters.Fk_Players,
                           parameters.Fk_Teams,
                           parameters.RateFrom,
                           parameters.RateTo,
                           parameters.PointsFrom,
                           parameters.PointsTo,
                           parameters.Fk_Player,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Season,
                           parameters.IsEnded,
                           parameters.IsCanNotEdit,
                           parameters.DashboardSearch);
        }

        public async Task<PlayerGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PlayerGameWeak entity)
        {
            if (FindByCondition(a => a.Fk_TeamGameWeak == entity.Fk_TeamGameWeak && a.Fk_Player == entity.Fk_Player, trackChanges: false).Any())
            {
                PlayerGameWeak oldEntity = FindByCondition(a => a.Fk_TeamGameWeak == entity.Fk_TeamGameWeak && a.Fk_Player == entity.Fk_Player, trackChanges: true).First();

                if (oldEntity.IsCanNotEdit == false)
                {
                    oldEntity.Ranking = entity.Ranking;
                }
                oldEntity._365_PlayerId = entity._365_PlayerId;
            }
            else
            {
                base.Create(entity);
            }
        }

        public void ResetPlayerGameWeak(int fk_TeamGameWeak, int fk_Player, int fk_GameWeak, int fk_Team)
        {
            if (fk_TeamGameWeak > 0 ||
                (fk_GameWeak > 0 && fk_Player > 0) ||
                (fk_GameWeak > 0 && fk_Team > 0))
            {
                List<PlayerGameWeak> data = FindByCondition(a => (fk_TeamGameWeak == 0 || a.Fk_TeamGameWeak == fk_TeamGameWeak) &&
                                           (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                           (fk_GameWeak == 0 || a.TeamGameWeak.Fk_GameWeak == fk_GameWeak) &&
                                           (fk_Team == 0 || a.Player.Fk_Team == fk_Team),
                                           trackChanges: true).ToList();
                Delete(data);
            }
        }

        public void UpdatePlayerGameWeakTotalPoints(int fk_PlayerGameWeak)
        {
            _ = DBContext.Database.ExecuteSqlRaw(@"UPDATE pgw
                                                   SET pgw.TotalPoints = (
                                                       SELECT SUM(pgs.Points)
                                                       FROM [dbo].[PlayerGameWeakScores] pgs
                                                       WHERE pgs.fk_PlayerGameWeak = pgw.Id
                                                   )
                                                   FROM [dbo].[PlayerGameWeaks] pgw
                                                   WHERE pgw.Id = @fkPlayerGameWeakId", new SqlParameter("@fkPlayerGameWeakId", fk_PlayerGameWeak));
        }
    }

    public static class PlayerGameWeakRepositoryExtension
    {
        public static IQueryable<PlayerGameWeak> Filter(
            this IQueryable<PlayerGameWeak> PlayerGameWeaks,
            int id,
            int fk_TeamGameWeak,
            int fk_Home,
            int fk_Away,
            List<int> fk_Players,
            List<int> fk_Teams,
            double rateFrom,
            double rateTo,
            int pointsFrom,
            int pointsTo,
            int fk_Player,
            int fk_GameWeak,
            int fk_Season,
            bool? isEnded,
            bool? isCanNotEdit,
            string dashboardSearch)
        {
            return PlayerGameWeaks.Where(a => (id == 0 || a.Id == id) &&

                                              (string.IsNullOrEmpty(dashboardSearch) ||
                                                   a.Id.ToString().Contains(dashboardSearch) ||
                                                   a.Player.Name.Contains(dashboardSearch)) &&

                                              (isCanNotEdit == null || a.IsCanNotEdit == isCanNotEdit) &&

                                              (fk_TeamGameWeak == 0 || a.Fk_TeamGameWeak == fk_TeamGameWeak) &&
                                              (fk_Home == 0 || a.TeamGameWeak.Fk_Home == fk_Home) &&
                                              (fk_Away == 0 || a.TeamGameWeak.Fk_Away == fk_Away) &&
                                              (fk_Teams == null || !fk_Teams.Any() ||
                                                fk_Teams.Contains(a.TeamGameWeak.Fk_Home) || fk_Teams.Contains(a.TeamGameWeak.Fk_Away)) &&
                                              (fk_Players == null || !fk_Players.Any() ||
                                                fk_Players.Contains(a.Fk_Player)) &&
                                              (rateFrom == 0 || a.Ranking >= rateFrom) &&
                                              (rateTo == 0 || a.Ranking <= rateTo) &&
                                              (pointsFrom == 0 || a.TotalPoints >= pointsFrom) &&
                                              (pointsTo == 0 || a.TotalPoints <= pointsTo) &&
                                              (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                              (fk_GameWeak == 0 || a.TeamGameWeak.Fk_GameWeak == fk_GameWeak) &&
                                              (fk_Season == 0 || a.TeamGameWeak.GameWeak.Fk_Season == fk_Season) &&
                                              (isEnded == null || a.TeamGameWeak.IsEnded == isEnded));


        }

    }
}

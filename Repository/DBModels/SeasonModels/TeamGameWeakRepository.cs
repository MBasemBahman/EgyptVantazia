using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;

namespace Repository.DBModels.SeasonModels
{
    public class TeamGameWeakRepository : RepositoryBase<TeamGameWeak>
    {
        public TeamGameWeakRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<TeamGameWeak> FindAll(TeamGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Teams,
                           parameters.Fk_Team,
                           parameters.Fk_Home,
                           parameters.Fk_Away,
                           parameters.Fk_GameWeak,
                           parameters.Fk_GameWeak_Ignored,
                           parameters.Fk_Season,
                           parameters._365_MatchId,
                           parameters.FromTime,
                           parameters.ToTime,
                           parameters.IsEnded,
                           parameters.IsDelayed,
                           parameters.CurrentSeason,
                           parameters.CurrentGameWeak,
                           parameters.DashboardSearch);
        }

        public async Task<TeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public async Task<TeamGameWeak> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_MatchId == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(TeamGameWeak entity)
        {
            if (entity._365_MatchId.IsExisting() && FindByCondition(a => a.Fk_Away == entity.Fk_Away && a.Fk_Home == entity.Fk_Home, trackChanges: false).Any())
            {
                TeamGameWeak oldEntity = FindByCondition(a => a.Fk_Away == entity.Fk_Away && a.Fk_Home == entity.Fk_Home, trackChanges: true).First();

                oldEntity.Fk_Away = entity.Fk_Away;
                oldEntity.Fk_Home = entity.Fk_Home;
                oldEntity.Fk_GameWeak = entity.Fk_GameWeak;
                oldEntity.StartTime = entity.StartTime;
                oldEntity.IsEnded = entity.IsEnded;
                oldEntity._365_MatchId = entity._365_MatchId;
                oldEntity.AwayScore = entity.AwayScore;
                oldEntity.HomeScore = entity.HomeScore;
                oldEntity.IsDelayed = entity.IsDelayed;
            }
            else
            {
                base.Create(entity);
            }
        }

        public void DeleteDuplicattion()
        {
            List<TeamGameWeak> data = FindByCondition(a => a.Fk_GameWeak == 45 && a.IsDelayed, trackChanges: true)
                        .Include(a => a.PlayerGameWeaks)
                        .ToList();
            foreach (TeamGameWeak item in data)
            {
                DBContext.PlayerGameWeaks.RemoveRange(item.PlayerGameWeaks);
                Delete(item);
            }
        }
    }

    public static class TeamGameWeakRepositoryExtension
    {
        public static IQueryable<TeamGameWeak> Filter(
            this IQueryable<TeamGameWeak> TeamGameWeaks,
            int id,
            List<int> fk_Teams,
            int fk_Team,
            int fk_Home,
            int fk_Away,
            int fk_GameWeak,
            int fk_GameWeak_Ignored,
            int fk_Season,
            string _365_MatchId,
            DateTime? fromTime,
            DateTime? toTime,
            bool? isEnded,
            bool? isDelayed,
            bool currentSeason,
            bool currentGameWeak,
            string dashboardSearch)
        {
            return TeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&

                                            (string.IsNullOrEmpty(dashboardSearch) ||
                                             a.Id.ToString().Contains(dashboardSearch) ||
                                             a.Home.Name.Contains(dashboardSearch) ||
                                             a.Away.Name.Contains(dashboardSearch)) &&

                                            (isEnded == null || a.IsEnded == isEnded) &&
                                            (isDelayed == null || a.IsDelayed == isDelayed) &&
                                            (fk_Teams == null || !fk_Teams.Any() ||
                                              fk_Teams.Contains(a.Fk_Home) || fk_Teams.Contains(a.Fk_Away)) &&
                                            (fk_Team == 0 || a.Fk_Home == fk_Team || a.Fk_Away == fk_Team) &&
                                            (fk_Home == 0 || a.Fk_Home == fk_Home) &&
                                            (fk_Away == 0 || a.Fk_Away == fk_Away) &&
                                            (fk_Season == 0 || a.GameWeak.Fk_Season == fk_Season) &&
                                            (currentSeason == false || a.GameWeak.Season.IsCurrent) &&
                                            (currentGameWeak == false || a.GameWeak.IsCurrent) &&
                                            (fk_GameWeak == 0 || a.Fk_GameWeak == fk_GameWeak) &&
                                            (fk_GameWeak_Ignored == 0 || a.Fk_GameWeak != fk_GameWeak_Ignored) &&
                                            (string.IsNullOrWhiteSpace(_365_MatchId) || a._365_MatchId == _365_MatchId) &&
                                            (fromTime == null || a.StartTime >= fromTime) &&
                                            (toTime == null || a.StartTime <= toTime));

        }

    }
}

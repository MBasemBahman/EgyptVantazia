using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;

namespace Repository.DBModels.SeasonModels
{
    public class TeamGameWeakRepository : RepositoryBase<TeamGameWeak>
    {
        public TeamGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<TeamGameWeak> FindAll(TeamGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Home,
                           parameters.Fk_Away,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Season,
                           parameters._365_MatchId,
                           parameters.FromTime,
                           parameters.ToTime,
                           parameters.IsEnded,
                           parameters.CurrentSeason,
                           parameters.CurrentGameWeak);
        }

        public async Task<TeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public async Task<TeamGameWeak> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_MatchId == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(TeamGameWeak entity)
        {
            if (entity._365_MatchId.IsExisting() && FindByCondition(a => a._365_MatchId == entity._365_MatchId, trackChanges: false).Any())
            {
                TeamGameWeak oldEntity = FindByCondition(a => a._365_MatchId == entity._365_MatchId, trackChanges: false).First();

                oldEntity.Fk_Away = entity.Fk_Away;
                oldEntity.Fk_Home = entity.Fk_Home;
                oldEntity.Fk_GameWeak = entity.Fk_GameWeak;
                oldEntity.StartTime = entity.StartTime;
                oldEntity.IsEnded = entity.IsEnded;
                oldEntity._365_MatchId = entity._365_MatchId;
                oldEntity.AwayScore = entity.AwayScore;
                oldEntity.HomeScore = entity.HomeScore;
            }
            else
            {
                base.Create(entity);
            }
        }
    }

    public static class TeamGameWeakRepositoryExtension
    {
        public static IQueryable<TeamGameWeak> Filter(
            this IQueryable<TeamGameWeak> TeamGameWeaks,
            int id,
            int Fk_Home,
            int Fk_Away,
            int Fk_GameWeak,
            int fk_Season,
            string _365_MatchId,
            DateTime? fromTime,
            DateTime? toTime,
            bool? IsEnded,
            bool currentSeason,
            bool currentGameWeak)
        {
            return TeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                            (IsEnded == null || a.IsEnded == IsEnded) &&
                                            (Fk_Home == 0 || a.Fk_Home == Fk_Home) &&
                                            (Fk_Away == 0 || a.Fk_Away == Fk_Away) &&
                                            (fk_Season == 0 || a.GameWeak.Fk_Season == fk_Season) &&
                                            (currentSeason == false || a.GameWeak.Season.IsCurrent) &&
                                            (currentGameWeak == false || a.GameWeak.IsCurrent) &&
                                            (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak) &&
                                            (string.IsNullOrWhiteSpace(_365_MatchId) || a._365_MatchId == _365_MatchId) &&
                                            (fromTime == null || a.StartTime >= fromTime) &&
                                            (toTime == null || a.StartTime <= toTime));

        }

    }
}

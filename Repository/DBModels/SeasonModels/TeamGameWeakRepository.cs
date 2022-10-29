﻿using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;

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
                           parameters.Fk_Teams,
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
            if (entity._365_MatchId.IsExisting() && FindByCondition(a => a.Fk_GameWeak == entity.Fk_GameWeak && a.Fk_Away == entity.Fk_Away && a.Fk_Home == entity.Fk_Home, trackChanges: false).Any())
            {
                TeamGameWeak oldEntity = FindByCondition(a => a.Fk_GameWeak == entity.Fk_GameWeak && a.Fk_Away == entity.Fk_Away && a.Fk_Home == entity.Fk_Home, trackChanges: true).First();

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
            List<int> fk_Teams,
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
            bool currentGameWeak)
        {
            return TeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                            (isEnded == null || a.IsEnded == isEnded) &&
                                            (isDelayed == null || a.IsDelayed == isDelayed) &&
                                            (fk_Teams == null || !fk_Teams.Any() ||
                                              fk_Teams.Contains(a.Fk_Home) || fk_Teams.Contains(a.Fk_Away)) &&
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

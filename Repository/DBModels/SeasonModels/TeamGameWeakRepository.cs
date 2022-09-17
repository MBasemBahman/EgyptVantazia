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
                           parameters.Fk_Home,
                           parameters.Fk_Away,
                           parameters.Fk_GameWeak,
                           parameters._365_MatchId,
                           parameters._365_MatchUpId,
                           parameters.FromTime,
                           parameters.ToTime);
        }

        public async Task<TeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
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
            string _365_MatchId,
            string _365_MatchUpId,
            DateTime? fromTime,
            DateTime? toTime)
        {
            return TeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                            (Fk_Home == 0 || a.Fk_Home == Fk_Home) &&
                                            (Fk_Away == 0 || a.Fk_Away == Fk_Away) &&
                                            (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak) &&
                                            (string.IsNullOrWhiteSpace(_365_MatchId) || a._365_MatchId == _365_MatchId) &&
                                            (string.IsNullOrWhiteSpace(_365_MatchUpId) || a._365_MatchUpId == _365_MatchUpId) &&
                                            (fromTime == null || a.StartTime >= fromTime) &&
                                            (toTime == null || a.StartTime <= toTime));

        }

    }
}

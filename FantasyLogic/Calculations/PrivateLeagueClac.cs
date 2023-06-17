using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;

namespace FantasyLogic.Calculations
{
    public class PrivateLeagueClac
    {
        private readonly UnitOfWork _unitOfWork;

        public PrivateLeagueClac(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void RunPrivateLeaguesRanking(int? fk_GameWeak, int id, bool indebug = false)
        {
            int season = _unitOfWork.Season.GetCurrentSeasonId();

            if (indebug)
            {
                UpdatePrivateLeaguesRanking(season, fk_GameWeak, id, indebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => UpdatePrivateLeaguesRanking(season, fk_GameWeak, id, indebug));
            }
        }

        public void UpdatePrivateLeaguesRanking(int fk_Season, int? fk_GameWeak, int id, bool indebug)
        {
            var privateLeagues = _unitOfWork.PrivateLeague.GetPrivateLeagues(new PrivateLeagueParameters
            {
                HaveMembers = true,
                Fk_Season = fk_Season,
                Fk_GameWeak = fk_GameWeak,
                Id = id,
            }).Select(a => new
            {
                a.Id,
                _365_GameWeakIdValue = a.Fk_GameWeak > 0 ? a.GameWeak._365_GameWeakIdValue : 0
            }).ToList();

            string jobId = "";
            foreach (var privateLeague in privateLeagues)
            {
                if (indebug)
                {
                    _ = UpdatePrivateLeaguesRanking(fk_Season, privateLeague.Id, privateLeague._365_GameWeakIdValue, jobId);
                }
                else
                {
                    jobId = jobId.IsExisting()
                            ? BackgroundJob.ContinueJobWith(jobId, () => UpdatePrivateLeaguesRanking(fk_Season, privateLeague.Id, privateLeague._365_GameWeakIdValue, jobId))
                            : BackgroundJob.Enqueue(() => UpdatePrivateLeaguesRanking(fk_Season, privateLeague.Id, privateLeague._365_GameWeakIdValue, jobId));
                }
            }
        }

        public string UpdatePrivateLeaguesRanking(int fk_Season, int fk_PrivateLeague, int _365_GameWeakIdValue, string jobId)
        {
            int ranking = 1;

            List<AccountTeamModel> accountTeams = _unitOfWork.AccountTeam.GetPrivateLeaguesPoints(fk_Season, fk_PrivateLeague, _365_GameWeakIdValue);

            foreach (AccountTeamModel accountTeam in accountTeams)
            {
                _unitOfWork.PrivateLeague.CreatePrivateLeagueMember(new PrivateLeagueMember
                {
                    Fk_Account = accountTeam.Fk_Account,
                    Fk_PrivateLeague = fk_PrivateLeague,
                    Ranking = ranking++,
                    Points = accountTeam.TotalPoints
                });
            }

            _unitOfWork.Save().Wait();

            return jobId;
        }
    }
}

using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.CoreServicesModels.SeasonModels;
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

        public void RunPrivateLeaguesRanking(bool indebug = false)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            if (indebug)
            {
                UpdatePrivateLeaguesRanking(season.Id, indebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => UpdatePrivateLeaguesRanking(season.Id, indebug));
            }
        }

        public void UpdatePrivateLeaguesRanking(int fk_Season, bool indebug)
        {
            var privateLeagues = _unitOfWork.PrivateLeague.GetPrivateLeagues(new PrivateLeagueParameters
            {
                HaveMembers = true,
                Fk_Season = fk_Season,
            }, false).Select(a => new
            {
                a.Id,
                _365_GameWeakIdValue = a.Fk_GameWeak > 0 ? a.GameWeak._365_GameWeakIdValue : 0
            }).ToList();

            string jobId = "";
            foreach (var privateLeague in privateLeagues)
            {
                if (indebug)
                {
                    UpdatePrivateLeaguesRanking(fk_Season, privateLeague.Id, privateLeague._365_GameWeakIdValue, jobId);
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

            var accountTeams = _unitOfWork.AccountTeam.GetPrivateLeaguesPoints(fk_Season, fk_PrivateLeague, _365_GameWeakIdValue);

            foreach (var accountTeam in accountTeams)
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

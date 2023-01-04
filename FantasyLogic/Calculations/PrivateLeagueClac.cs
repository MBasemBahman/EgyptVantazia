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

        public void RunPrivateLeaguesRanking()
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            //UpdatePrivateLeaguesRanking(season.Id);
            _ = BackgroundJob.Enqueue(() => UpdatePrivateLeaguesRanking(season.Id));
        }

        public void UpdatePrivateLeaguesRanking(int fk_Season)
        {
            List<int> privateLeagues = _unitOfWork.PrivateLeague.GetPrivateLeagues(new PrivateLeagueParameters
            {
                HaveMembers = true,
                Fk_Season = fk_Season,
            }, false).Select(a => a.Id).ToList();

            string jobId = "";
            foreach (int privateLeagueId in privateLeagues)
            {
                //UpdatePrivateLeaguesRanking(fk_Season, privateLeagueId, jobId);

                jobId = jobId.IsExisting()
                            ? BackgroundJob.ContinueJobWith(jobId, () => UpdatePrivateLeaguesRanking(fk_Season, privateLeagueId, jobId))
                            : BackgroundJob.Enqueue(() => UpdatePrivateLeaguesRanking(fk_Season, privateLeagueId, jobId));
            }
        }

        public string UpdatePrivateLeaguesRanking(int fk_Season, int fk_PrivateLeague, string jobId)
        {
            int ranking = 1;

            var accountTeams = _unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters
            {
                Fk_Season = fk_Season,
                Fk_PrivateLeague = fk_PrivateLeague
            }, otherLang: false)
                .OrderByDescending(a => a.TotalPoints)
                .Select(a => new
                {
                    a.Fk_Account,
                })
                .ToList();

            foreach (var accountTeam in accountTeams)
            {
                _unitOfWork.PrivateLeague.CreatePrivateLeagueMember(new PrivateLeagueMember
                {
                    Fk_Account = accountTeam.Fk_Account,
                    Fk_PrivateLeague = fk_PrivateLeague,
                    Ranking = ranking++
                });
            }

            _unitOfWork.Save().Wait();

            return jobId;
        }
    }
}

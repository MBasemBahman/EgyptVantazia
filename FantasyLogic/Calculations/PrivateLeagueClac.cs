using Entities.CoreServicesModels.PrivateLeagueModels;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.Calculations
{
    public class PrivateLeagueClac
    {
        private readonly UnitOfWork _unitOfWork;

        public PrivateLeagueClac(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void RunPrivateLeaguesRanking(_365CompetitionsEnum _365CompetitionsEnum, int? fk_GameWeak, int id, bool indebug = false)
        {
            int season = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);

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
                a.Id
            }).ToList();

            string jobId = "";
            foreach (var privateLeague in privateLeagues)
            {
                if (indebug)
                {
                    _ = UpdatePrivateLeaguesRanking(privateLeague.Id, jobId);
                }
                else
                {
                    jobId = jobId.IsExisting()
                            ? BackgroundJob.ContinueJobWith(jobId, () => UpdatePrivateLeaguesRanking(privateLeague.Id, jobId))
                            : BackgroundJob.Enqueue(() => UpdatePrivateLeaguesRanking(privateLeague.Id, jobId));
                }
            }
        }

        public string UpdatePrivateLeaguesRanking(int fk_PrivateLeague, string jobId)
        {
            _unitOfWork.PrivateLeague.UpdatePrivateLeagueMembersPointsAndRanking(fk_PrivateLeague);

            return jobId;
        }
    }
}

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

            foreach (var privateLeague in privateLeagues)
            {
                if (indebug)
                {
                    UpdatePrivateLeaguesRanking(privateLeague.Id);
                }
                else
                {
                    BackgroundJob.Enqueue(() => UpdatePrivateLeaguesRanking(privateLeague.Id));
                }
            }
        }

        public void UpdatePrivateLeaguesRanking(int fk_PrivateLeague)
        {
            _unitOfWork.PrivateLeague.UpdatePrivateLeagueMembersPointsAndRanking(fk_PrivateLeague);
        }
    }
}

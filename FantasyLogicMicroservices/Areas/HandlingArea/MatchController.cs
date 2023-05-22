using Entities.CoreServicesModels.SeasonModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace FantasyLogicMicroservices.Areas.HandlingArea
{
    [ApiVersion("1.0")]
    [Area("Handling")]
    [ApiExplorerSettings(GroupName = "Handling")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class MatchController : ExtendControllerBase
    {
        public MatchController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }


        [HttpPost]
        [Route(nameof(ResetMatch))]
        public IActionResult ResetMatch(
            [FromQuery] int fk_TeamGameWeak,
            [FromQuery] int fk_GameWeak,
            [FromQuery] List<int> fk_Teams)
        {
            if (fk_TeamGameWeak > 0)
            {
                TeamGameWeakModel match = _unitOfWork.Season.GetTeamGameWeakbyId(fk_TeamGameWeak, otherLang: false);

                _unitOfWork.PlayerScore.ResetPlayerGameWeak(fk_TeamGameWeak, 0, 0, 0);
                _unitOfWork.PlayerState.ResetPlayerGameWeakScoreState(0, match.Fk_GameWeak, match.Fk_Home);
                _unitOfWork.PlayerState.ResetPlayerGameWeakScoreState(0, match.Fk_GameWeak, match.Fk_Away);
                _unitOfWork.Save().Wait();

                fk_Teams = new()
                    {
                        match.Fk_Home,
                        match.Fk_Away
                    };

                _fantasyUnitOfWork.PlayerStateCalc.RunPlayersStateCalculations(0, null, null, fk_Teams, true, false);

                _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(match.Fk_GameWeak, 0, null, fk_Teams, false);
            }
            else if (fk_GameWeak > 0 &&
                     fk_Teams.Any())
            {

                foreach (var fk_Team in fk_Teams)
                {
                    _unitOfWork.PlayerScore.ResetPlayerGameWeak(0, 0, fk_GameWeak, fk_Team);
                    _unitOfWork.PlayerState.ResetPlayerGameWeakScoreState(0, fk_GameWeak, fk_Team);
                }

                _unitOfWork.Save().Wait();

                _fantasyUnitOfWork.PlayerStateCalc.RunPlayersStateCalculations(0, null, null, fk_Teams, true, false);

                _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak, 0, null, fk_Teams, false);
            }

            return Ok();
        }
    }
}

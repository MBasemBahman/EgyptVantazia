﻿using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;

namespace FantasyLogicMicroservices.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamController : ExtendControllerBase
    {
        public AccountTeamController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamsPoints))]
        public IActionResult UpdateAccountTeamsPoints(
            [FromQuery] int fk_GameWeak,
            [FromQuery] int fk_AccountTeam,
            [FromQuery] List<int> fk_Players,
            [FromQuery] bool inDebug)
        {
            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak, fk_AccountTeam, fk_Players, null, inDebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak, fk_AccountTeam, fk_Players, null, inDebug));
            }

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamGameWeakRanking))]
        public IActionResult UpdateAccountTeamGameWeakRanking(
            [FromQuery] int fk_GameWeak,
            [FromQuery] bool inDebug)
        {
            GameWeakModel gameWeek = _unitOfWork.Season.GetGameWeakbyId(fk_GameWeak, otherLang: false);

            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(gameWeek, gameWeek.Fk_Season);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(gameWeek, gameWeek.Fk_Season));
            }
            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamRanking))]
        public IActionResult UpdateAccountTeamRanking(
           [FromQuery] int fk_GameWeak,
           [FromQuery] bool inDebug)
        {
            GameWeakModel gameWeek = _unitOfWork.Season.GetGameWeakbyId(fk_GameWeak, otherLang: false);

            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamRanking(gameWeek.Fk_Season);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamRanking(gameWeek.Fk_Season));
            }
            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdatePrivateLeaguesRanking))]
        public IActionResult UpdatePrivateLeaguesRanking(
            [FromQuery] bool indebug,
            [FromQuery] int? fk_GameWeak,
            [FromQuery] int id)
        {
            if (indebug)
            {
                _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(fk_GameWeak, id, indebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(fk_GameWeak, id, indebug));
            }

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamUpdateCards))]
        public IActionResult UpdateAccountTeamUpdateCards(
            [FromQuery] bool indebug,
            [FromBody] AccountTeamsUpdateCards model)
        {
            //if (indebug)
            //{
            //    _unitOfWork.AccountTeam.UpdateAccountTeamGameWeakRank
            //}
            //else
            //{
            //    _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(fk_GameWeak, id, indebug));
            //}

            return Ok();
        }
    }
}

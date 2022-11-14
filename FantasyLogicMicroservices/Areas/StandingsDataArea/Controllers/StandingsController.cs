﻿using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;

namespace FantasyLogicMicroservices.Areas.StandingsDataArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Standings")]
    [ApiExplorerSettings(GroupName = "Standings")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class StandingsController : ExtendControllerBase
    {
        public StandingsController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateStandings))]
        public IActionResult UpdateStandings()
        {
            //_fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings();

            _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings());

            RecurringJob.AddOrUpdate("UpdateStandings", () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(), "0 4 * * *", TimeZoneInfo.Utc);

            return Ok();
        }
    }
}

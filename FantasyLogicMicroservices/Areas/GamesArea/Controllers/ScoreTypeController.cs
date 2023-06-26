﻿using FantasyLogic;
using FantasyLogicMicroservices.Controllers;

namespace FantasyLogicMicroservices.Areas.GamesArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Games")]
    [ApiExplorerSettings(GroupName = "Games")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class ScoreTypeController : ExtendControllerBase
    {
        public ScoreTypeController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateScoreTypes))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateScoreTypes(bool inDebug)
        {
            _fantasyUnitOfWork.ScoreTypeDataHelper.RunUpdateStates(1, inDebug);

            return Ok();
        }
    }
}

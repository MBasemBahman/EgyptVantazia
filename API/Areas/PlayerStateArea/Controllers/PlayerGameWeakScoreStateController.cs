using API.Controllers;
using Entities.CoreServicesModels.PlayerStateModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Contracts.EnumData.DBModelsEnum;

namespace API.Areas.PlayerStateArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerState")]
    [ApiExplorerSettings(GroupName = "PlayerState")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerGameWeakScoreStateController : ExtendControllerBase
    {
        public PlayerGameWeakScoreStateController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayerGameWeakScoreStates))]
        public async Task<IEnumerable<PlayerGameWeakScoreStateModel>> GetPlayerGameWeakScoreStates(
        [FromQuery] PlayerGameWeakScoreStateParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (parameters.Fk_ScoreState == (int)ScoreStateEnum.CleanSheet)
            {
                parameters.Fk_PlayerPosition = (int)PlayerPositionEnum.Goalkeeper;
            }

            PagedList<PlayerGameWeakScoreStateModel> data = await _unitOfWork.PlayerState.GetPlayerGameWeakScoreStatePaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetPlayerGameWeakScoreStateById))]
        public PlayerGameWeakScoreStateModel GetPlayerGameWeakScoreStateById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakScoreStateModel data = _unitOfWork.PlayerState.GetPlayerGameWeakScoreStatebyId(id, otherLang);

            return data;
        }
    }
}

using API.Controllers;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.PlayerStateModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Contracts.EnumData.DBModelsEnum;

namespace API.Areas.PlayerStateArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerState")]
    [ApiExplorerSettings(GroupName = "PlayerState")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerSeasonScoreStateController : ExtendControllerBase
    {
        public PlayerSeasonScoreStateController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayerSeasonScoreStates))]
        public async Task<IEnumerable<PlayerSeasonScoreStateModel>> GetPlayerSeasonScoreStates(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
        [FromQuery] PlayerSeasonScoreStateParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (parameters.Fk_ScoreState == (int)ScoreStateEnum.CleanSheet)
            {
                parameters.Fk_PlayerPosition = (int)PlayerPositionEnum.Goalkeeper;
            }

            parameters.Fk_Season = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);
            parameters.OrderBy = "value desc,";

            if (parameters.Fk_ScoreState == (int)ScoreStateEnum.Total)
            {
                parameters.OrderBy = "points desc,";
            }

            if (parameters.GetMonthPlayer)
            {
                MontlyGameWeakFromToModel fromTo = GetMontlyGameWeakFromTo(_unitOfWork.Season.GetCurrentGameWeak(_365CompetitionsEnum)._365_GameWeakIdValue);

                if (fromTo != null)
                {
                    parameters.From_365_GameWeakIdValue = fromTo.From_365_GameWeakIdValue;
                    parameters.To_365_GameWeakIdValue = fromTo.To_365_GameWeakIdValue;
                }
            }

            PagedList<PlayerSeasonScoreStateModel> data = await _unitOfWork.PlayerState.GetPlayerSeasonScoreStatePaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetPlayerSeasonScoreStateById))]
        public PlayerSeasonScoreStateModel GetPlayerSeasonScoreStateById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerSeasonScoreStateModel data = _unitOfWork.PlayerState.GetPlayerSeasonScoreStatebyId(id, otherLang);

            return data;
        }

        private MontlyGameWeakFromToModel GetMontlyGameWeakFromTo(int _365_GameWeakIdValue)
        {
            int duration = 4;

            MontlyGameWeakFromToModel data = new();

            int current = ((_365_GameWeakIdValue < duration) ? 1 : (_365_GameWeakIdValue / duration));
            data.From_365_GameWeakIdValue = --current * duration;
            data.To_365_GameWeakIdValue = data.From_365_GameWeakIdValue + duration;

            return data;
        }
    }
}

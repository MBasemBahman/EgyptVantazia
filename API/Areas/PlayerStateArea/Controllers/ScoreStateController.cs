﻿using API.Areas.PlayerStateArea.Models;
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
    public class ScoreStateController : ExtendControllerBase
    {
        public ScoreStateController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetScoreStates))]
        public async Task<IEnumerable<ScoreStateModel>> GetScoreStates(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
        [FromQuery] ScoreStateParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            parameters.Fk_Season = auth.Fk_Season;

            if (parameters.IncludeBestPlayer && parameters.Fk_Season == 0 && parameters.Fk_GameWeak == 0)
            {
                parameters.Fk_Season = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);
            }

            PagedList<ScoreStateModel> data = await _unitOfWork.PlayerState.GetScoreStatePaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<ScoreStateDto> dataDto = _mapper.Map<IEnumerable<ScoreStateDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetScoreStateById))]
        public ScoreStateModel GetScoreStateById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ScoreStateModel data = _unitOfWork.PlayerState.GetScoreStatebyId(id, otherLang);

            return data;
        }
    }
}

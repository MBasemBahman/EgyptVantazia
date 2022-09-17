﻿using API.Areas.PlayerScoreArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.PlayerScoreModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.PlayerScoreArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerScore")]
    [ApiExplorerSettings(GroupName = "PlayerScore")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerGameWeakController : ExtendControllerBase
    {
        public PlayerGameWeakController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayerGameWeaks))]
        public async Task<IEnumerable<PlayerGameWeakModel>> GetPlayerGameWeaks(
        [FromQuery] PlayerGameWeakParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<PlayerGameWeakModel> data = await _unitOfWork.PlayerScore.GetPlayerGameWeakPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetPlayerGameWeakById))]
        public PlayerGameWeakModel GetPlayerGameWeakById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakModel data = _unitOfWork.PlayerScore.GetPlayerGameWeakbyId(id, otherLang);

            return data;
        }
    }
}

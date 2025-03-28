﻿using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using static Contracts.EnumData.DBModelsEnum;

namespace API.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamGameWeakController : ExtendControllerBase
    {
        public AccountTeamGameWeakController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetAccountTeamGameWeaks))]
        public async Task<IEnumerable<AccountTeamGameWeakModel>> GetAccountTeamGameWeaks(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] AccountTeamGameWeakParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            _365CompetitionsEnum = (_365CompetitionsEnum)auth.Season._365_CompetitionsId.ParseToInt();

            if (parameters.GetCurrentGameWeak)
            {
                parameters.Fk_GameWeak = _unitOfWork.Season.GetCurrentGameWeakId(_365CompetitionsEnum);
            }

            if (parameters.GetPrevGameWeak)
            {
                parameters.Fk_GameWeak = _unitOfWork.Season.GetPrevGameWeakId(_365CompetitionsEnum);
            }

            if (parameters.GetNextGameWeak)
            {
                parameters.Fk_GameWeak = _unitOfWork.Season.GetNextGameWeakId(_365CompetitionsEnum);
            }

            parameters.Fk_Season = auth.Fk_Season;

            PagedList<AccountTeamGameWeakModel> data = await _unitOfWork.AccountTeam.GetAccountTeamGameWeakPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}

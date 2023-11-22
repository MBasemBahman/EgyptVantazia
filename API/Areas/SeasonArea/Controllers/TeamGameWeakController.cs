using API.Areas.SeasonArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.SeasonModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Contracts.EnumData.DBModelsEnum;

namespace API.Areas.SeasonArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Season")]
    [ApiExplorerSettings(GroupName = "Season")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class TeamGameWeakController : ExtendControllerBase
    {
        public TeamGameWeakController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetTeamGameWeaks))]
        public async Task<IEnumerable<TeamGameWeakDto>> GetTeamGameWeaks(
        [FromQuery] TeamGameWeakParameters parameters)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            _365CompetitionsEnum _365CompetitionsEnum = (_365CompetitionsEnum)auth.Season._365_CompetitionsId.ParseToInt();
            parameters.Fk_Season = auth.Fk_Season;
            parameters.IsDelayed = false;

            if (parameters.NextGameWeak)
            {
                int gameWeakId = _unitOfWork.Season.GetNextGameWeakId(_365CompetitionsEnum);
                parameters.Fk_GameWeak = gameWeakId;
            }

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            parameters.IsActive = true;

            PagedList<TeamGameWeakModel> data = await _unitOfWork.Season.GetTeamGameWeakPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<TeamGameWeakDto> dataDto = _mapper.Map<IEnumerable<TeamGameWeakDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetTeamGameWeakById))]
        public TeamGameWeakDto GetTeamGameWeakById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamGameWeakModel data = _unitOfWork.Season.GetTeamGameWeakbyId(id, otherLang);

            TeamGameWeakDto dataDto = _mapper.Map<TeamGameWeakDto>(data);

            return dataDto;
        }
    }
}

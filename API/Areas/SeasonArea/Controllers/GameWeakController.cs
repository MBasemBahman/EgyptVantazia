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
    public class GameWeakController : ExtendControllerBase
    {
        public GameWeakController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetGameWeaks))]
        public async Task<IEnumerable<GameWeakDto>> GetGameWeaks(
        [FromQuery] GameWeakParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            parameters.Fk_Season = auth.Fk_Season;

            PagedList<GameWeakModel> data = await _unitOfWork.Season.GetGameWeakPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<GameWeakDto> dataDto = _mapper.Map<IEnumerable<GameWeakDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetGameWeakById))]
        public GameWeakDto GetGameWeakById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            GameWeakModel data = _unitOfWork.Season.GetGameWeakbyId(id, otherLang);

            GameWeakDto dataDto = _mapper.Map<GameWeakDto>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetNextGameWeakDeadLine))]
        public string GetNextGameWeakDeadLine(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            _365CompetitionsEnum = (_365CompetitionsEnum)auth.Season._365_CompetitionsId.ParseToInt();

            string dataDto = _mapper.Map<string>(_unitOfWork.Season.GetFirstTeamGameWeakMatchDate(_365CompetitionsEnum).Value.AddHours(2));
            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetCurrentGameWeak))]
        public GameWeakDto GetCurrentGameWeak([FromQuery] _365CompetitionsEnum _365CompetitionsEnum)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            _365CompetitionsEnum = (_365CompetitionsEnum)auth.Season._365_CompetitionsId.ParseToInt();

            GameWeakModel data = _unitOfWork.Season.GetCurrentGameWeak(_365CompetitionsEnum, otherLang);

            GameWeakDto dataDto = _mapper.Map<GameWeakDto>(data);

            return dataDto;
        }
    }
}

using API.Areas.SeasonArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [Route(nameof(GetCurrentSeason))]
        public GameWeakDto GetCurrentSeason()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            GameWeakModel data = _unitOfWork.Season.GetCurrentGameWeak(otherLang);

            GameWeakDto dataDto = _mapper.Map<GameWeakDto>(data);

            return dataDto;
        }
    }
}

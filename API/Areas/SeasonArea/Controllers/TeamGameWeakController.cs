using API.Areas.SeasonArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.SeasonModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

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

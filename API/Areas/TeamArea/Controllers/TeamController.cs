using API.Areas.TeamArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.TeamModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.TeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Team")]
    [ApiExplorerSettings(GroupName = "Team")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class TeamController : ExtendControllerBase
    {
        public TeamController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetTeams))]
        [AllowAnonymous]
        public async Task<IEnumerable<TeamDto>> GetTeams(
        [FromQuery] TeamParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<TeamModel> data = await _unitOfWork.Team.GetTeamPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<TeamDto> dataDto = _mapper.Map<IEnumerable<TeamDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetTeamById))]
        public TeamDto GetTeamById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamModel data = _unitOfWork.Team.GetTeambyId(id, otherLang);

            TeamDto dataDto = _mapper.Map<TeamDto>(data);

            return dataDto;
        }
    }
}

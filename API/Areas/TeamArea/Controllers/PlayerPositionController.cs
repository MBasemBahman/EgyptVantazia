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
    public class PlayerPositionController : ExtendControllerBase
    {
        public PlayerPositionController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayerPositions))]
        public async Task<IEnumerable<PlayerPositionDto>> GetPlayerPositions(
        [FromQuery] PlayerPositionParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<PlayerPositionModel> data = await _unitOfWork.Team.GetPlayerPositionPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<PlayerPositionDto> dataDto = _mapper.Map<IEnumerable<PlayerPositionDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetPlayerPositionById))]
        public PlayerPositionDto GetPlayerPositionById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerPositionModel data = _unitOfWork.Team.GetPlayerPositionbyId(id, otherLang);

            PlayerPositionDto dataDto = _mapper.Map<PlayerPositionDto>(data);

            return dataDto;
        }
    }
}

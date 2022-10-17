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
    public class PlayerController : ExtendControllerBase
    {
        public PlayerController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayers))]
        public async Task<IEnumerable<PlayerDto>> GetPlayers(
        [FromQuery] PlayerParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (parameters.IncludeScore && parameters.Fk_Season == 0)
            {
                parameters.Fk_Season = _unitOfWork.Season.GetCurrentSeason().Id;
            }

            PagedList<PlayerModel> data = await _unitOfWork.Team.GetPlayerPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<PlayerDto> dataDto = _mapper.Map<IEnumerable<PlayerDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetPlayerById))]
        public PlayerDto GetPlayerById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerModel data = _unitOfWork.Team.GetPlayerbyId(id, otherLang);

            PlayerDto dataDto = _mapper.Map<PlayerDto>(data);

            return dataDto;
        }
    }
}

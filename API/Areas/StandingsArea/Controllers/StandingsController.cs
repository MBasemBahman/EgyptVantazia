using API.Areas.StandingsArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.StandingsModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.StandingsArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Standings")]
    [ApiExplorerSettings(GroupName = "Standings")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class StandingsController : ExtendControllerBase
    {
        public StandingsController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetStandings))]
        public async Task<IEnumerable<StandingsDto>> GetStandings(
        [FromQuery] StandingsParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<StandingsModel> data = await _unitOfWork.Standings.GetStandingsPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<StandingsDto> dataDto = _mapper.Map<IEnumerable<StandingsDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetStandingsById))]
        public StandingsDto GetStandingsById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            StandingsModel data = _unitOfWork.Standings.GetStandingsbyId(id, otherLang);

            StandingsDto dataDto = _mapper.Map<StandingsDto>(data);

            return dataDto;
        }
    }
}

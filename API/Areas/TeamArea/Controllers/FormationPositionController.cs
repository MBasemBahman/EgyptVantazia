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
    public class FormationPositionController : ExtendControllerBase
    {
        public FormationPositionController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetFormationPositions))]
        public async Task<IEnumerable<FormationPositionDto>> GetFormationPositions(
        [FromQuery] FormationPositionParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<FormationPositionModel> data = await _unitOfWork.Team.GetFormationPositionPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<FormationPositionDto> dataDto = _mapper.Map<IEnumerable<FormationPositionDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetFormationPositionById))]
        public FormationPositionDto GetFormationPositionById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            FormationPositionModel data = _unitOfWork.Team.GetFormationPositionbyId(id, otherLang);

            FormationPositionDto dataDto = _mapper.Map<FormationPositionDto>(data);

            return dataDto;
        }
    }
}

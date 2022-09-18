using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class TeamPlayerTypeController : ExtendControllerBase
    {
        public TeamPlayerTypeController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetTeamPlayerTypes))]
        public async Task<IEnumerable<TeamPlayerTypeModel>> GetTeamPlayerTypes(
        [FromQuery] RequestParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<TeamPlayerTypeModel> data = await _unitOfWork.AccountTeam.GetTeamPlayerTypePaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetTeamPlayerTypeById))]
        public TeamPlayerTypeModel GetTeamPlayerTypeById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamPlayerTypeModel data = _unitOfWork.AccountTeam.GetTeamPlayerTypebyId(id, otherLang);

            return data;
        }
    }
}

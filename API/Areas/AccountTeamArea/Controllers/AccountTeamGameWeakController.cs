using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;

namespace API.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamGameWeakController : ExtendControllerBase
    {
        public AccountTeamGameWeakController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetAccountTeamGameWeaks))]
        public async Task<IEnumerable<AccountTeamGameWeakModel>> GetAccountTeamGameWeaks(
        [FromQuery] AccountTeamGameWeakParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<AccountTeamGameWeakModel> data = await _unitOfWork.AccountTeam.GetAccountTeamGameWeakPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}

using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;

namespace API.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamPlayerGameWeakController : ExtendControllerBase
    {
        public AccountTeamPlayerGameWeakController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetAccountTeamPlayerGameWeaks))]
        public async Task<IEnumerable<AccountTeamPlayerGameWeakModel>> GetAccountTeamPlayerGameWeaks(
        [FromQuery] AccountTeamPlayerGameWeakParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            parameters.Fk_Season = auth.Fk_Season;

            PagedList<AccountTeamPlayerGameWeakModel> data = await _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeakPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}

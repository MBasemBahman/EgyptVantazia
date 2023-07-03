using API.Controllers;
using Entities.CoreServicesModels.MatchStatisticModels;

namespace API.Areas.MatchStatisticArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerState")]
    [ApiExplorerSettings(GroupName = "MatchStatistic")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class MatchStatisticScoreController : ExtendControllerBase
    {
        public MatchStatisticScoreController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetMatchStatistics))]
        public async Task<IEnumerable<MatchStatisticScoreModel>> GetMatchStatistics(
        [FromQuery] MatchStatisticScoreParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            parameters.Fk_Season = auth.Fk_Season;

            PagedList<MatchStatisticScoreModel> data = await _unitOfWork.MatchStatistic.GetMatchStatisticScoresPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}

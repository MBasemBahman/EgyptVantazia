using API.Controllers;
using Entities.CoreServicesModels.MatchStatisticModels;

namespace API.Areas.MatchStatisticArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerState")]
    [ApiExplorerSettings(GroupName = "MatchStatistic")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class StatisticScoreController : ExtendControllerBase
    {
        public StatisticScoreController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetStatisticScores))]
        public async Task<IEnumerable<StatisticScoreModel>> GetStatisticScores(
        [FromQuery] StatisticScoreParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<StatisticScoreModel> data = await _unitOfWork.MatchStatistic.GetStatisticScoresPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}

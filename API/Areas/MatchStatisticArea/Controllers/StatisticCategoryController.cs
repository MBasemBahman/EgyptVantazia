using API.Controllers;
using Entities.CoreServicesModels.MatchStatisticModels;

namespace API.Areas.MatchStatisticArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerState")]
    [ApiExplorerSettings(GroupName = "MatchStatistic")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class StatisticCategoryController : ExtendControllerBase
    {
        public StatisticCategoryController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetStatisticCategories))]
        public async Task<IEnumerable<StatisticCategoryModel>> GetStatisticCategories(
        [FromQuery] StatisticCategoryParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<StatisticCategoryModel> data = await _unitOfWork.MatchStatistic.GetStatisticCategorysPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}

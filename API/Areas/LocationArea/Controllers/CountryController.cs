using API.Controllers;
using Entities.CoreServicesModels.LocationModels;

namespace API.Areas.LocationArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Location")]
    [ApiExplorerSettings(GroupName = "Location")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class CountryController : ExtendControllerBase
    {
        public CountryController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetCountries))]
        [AllowAnonymous]
        public async Task<IEnumerable<CountryModel>> GetCountries(
       [FromQuery] RequestParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<CountryModel> data = await _unitOfWork.Location.GetCountrysPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}

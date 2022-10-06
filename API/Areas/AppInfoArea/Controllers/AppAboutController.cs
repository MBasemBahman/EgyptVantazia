using API.Controllers;
using Entities.CoreServicesModels.AppInfoModels;
using IntegrationWith365;

namespace API.Areas.AppInfoArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AppInfo")]
    [ApiExplorerSettings(GroupName = "AppInfo")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AppAboutController : ExtendControllerBase
    {
        private readonly _365Services _365Services;
        public AppAboutController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        _365Services _365Services,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
            this._365Services = _365Services;
        }

        [HttpGet]
        [Route(nameof(GetAppAbout))]
        [AllowAll]
        public async Task<AppAboutModel> GetAppAbout()
        {
            await _365Services.GetStandings(new IntegrationWith365.Parameters._365StandingsParameters
            {
                SeasonNum = 26,
                IsArabic = true,
            });

            await _365Services.GetSquads(new IntegrationWith365.Parameters._365SquadsParameters
            {
                Competitors = 8201,
                IsArabic = true,
            });

            await _365Services.GetGames(new IntegrationWith365.Parameters._365GamesParameters
            {
                Aftergame = 3555948,
                IsArabic = true,
            });

            await _365Services.GetGame(new IntegrationWith365.Parameters._365GameParameters
            {
                GameId = 3555923,
                MatchupId = "8300-8306-552",
                IsArabic = true,
            });

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AppAboutModel data = await _unitOfWork.AppInfo.GetAppAbouts(new RequestParameters(), otherLang).FirstOrDefaultAsync();

            return data;
        }
    }
}

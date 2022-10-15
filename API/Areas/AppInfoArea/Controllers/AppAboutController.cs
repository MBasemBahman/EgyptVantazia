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
            DataMigration dataMigration = new(_unitOfWork, _365Services);

            //await dataMigration.InsertTeams();
            //await dataMigration.InsertPositions();
            //await dataMigration.InsertPlayers();
            //await dataMigration.InsertStandings();
            //await dataMigration.InsertRounds();
            //await dataMigration.InsertGames();
           await dataMigration.InsertGameResult();

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AppAboutModel data = await _unitOfWork.AppInfo.GetAppAbouts(new RequestParameters(), otherLang).FirstOrDefaultAsync();

            return data;
        }
    }
}

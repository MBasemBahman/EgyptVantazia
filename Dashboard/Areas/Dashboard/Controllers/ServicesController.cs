using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;

namespace Dashboard.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class ServicesController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public ServicesController(
            ILoggerManager logger,
            IMapper mapper,
            UnitOfWork unitOfWork,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        [HttpGet]
        public JsonResult GetGameWeak(int fk_Season)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            var result = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
                Fk_Season = fk_Season
            }, otherLang).OrderBy(a => a._365_GameWeakIdValue).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetPlayers(int fk_Team)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            var result = _unitOfWork.Team.GetPlayers(new Entities.CoreServicesModels.TeamModels.PlayerParameters
            {
                Fk_Team = fk_Team
            }, otherLang).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return Json(result);
        }
        
        [HttpGet]
        public JsonResult GetTeams(int fk_Season)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            var result = _unitOfWork.Team.GetTeams(new TeamParameters
            {
                Fk_Season = fk_Season
            }, otherLang).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetTeamGameWeak(int fk_Team, int fk_GameWeak)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            var result = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
                Fk_Home = fk_Team,
                Fk_GameWeak = fk_GameWeak
            }, otherLang).Select(a => new
            {
                a.Id,
                Name = $"{a.Home.Name} - {a.Away.Name}"
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetStatisticScores(int Fk_StatisticCategory)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            var result = _unitOfWork.MatchStatistic.GetStatisticScores(new StatisticScoreParameters
            {
                Fk_StatisticCategory = Fk_StatisticCategory,
            }, otherLang).Select(a => new
            {
                a.Id,
                Name = a.Name
            }).ToList();

            return Json(result);
        }
        
        [HttpPost]
        public ActionResult<Dictionary<string, string>> GetPlayersByName(List<int> fk_Teams, string name)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            return _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                SearchTerm = name,
                SearchColumns = "Name,ShortName",
                Fk_Teams = fk_Teams
            }, otherLang).Take(10).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }
    }
}

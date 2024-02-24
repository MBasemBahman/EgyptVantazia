using BaseDB;
using Dashboard.Areas.AccountTeamEntity.Models;
using Dashboard.Areas.Dashboard.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.DashboardAdministrationModels;
using Entities.DBModels.SeasonModels;
using NLog.Filters;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(DashboardViewEnum.Home, AccessLevelEnum.View)]
    public class HomeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;
        private readonly BaseContext _dBContext;
        private readonly ILocalizationManager _Localizer;
        private readonly UpdateResultsUtils _updateResultsUtils;

        public HomeController(
            ILoggerManager logger,
            IMapper mapper,
            UnitOfWork unitOfWork,
            IWebHostEnvironment environment,
            BaseContext dBContext,
            ILocalizationManager localizer,
            UpdateResultsUtils updateResultsUtils)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _environment = environment;
            _dBContext = dBContext;
            _Localizer = localizer;
            _updateResultsUtils = updateResultsUtils;
        }

        public IActionResult Index()
        {
            List<ChartDto> charts = new()
            {
                //new ChartDto
                //{
                //    Key = nameof(Accounts),
                //    Text = nameof(Accounts),
                //    Url = "/",
                //    Type = ChartTypeEnum.Donut
                //}
            };
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["CurrentGameWeak"] = _unitOfWork.Season.GetCurrentGameWeakId(_365CompetitionsEnum.Egypt);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);

            ViewData["auth"] = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            return View(charts);
        }

        [AllowAnonymous]
        public IActionResult SetSettings()
        {
            _ = Culture(ViewDataConstants.Arabic);
            _ = Theme(ViewDataConstants.Light);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Culture(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture: ViewDataConstants.English, uiCulture: culture)));

            return Request.Headers.Referer.Any() ? Redirect(Request.Headers.Referer) : RedirectToAction(nameof(Index));
        }

        public IActionResult Theme(string theme)
        {
            Response.Cookies.Append(ViewDataConstants.Theme, theme);

            return Request.Headers.Referer.Any() ? Redirect(Request.Headers.Referer) : RedirectToAction(nameof(Index));
        }

        [Route("Error")]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        #region Charts

        [HttpPost]
        public JsonResult Accounts()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            int accountsCount = _unitOfWork.Account.GetAccountsCount();
            int accountTeamCount = _unitOfWork.AccountTeam.GetAccountTeamCount();

            List<ChartItemDto> data = new()
            {
                new()
                {
                    label = _Localizer.Get("Accounts"),
                    value = accountsCount,
                    Id = "AccountEntity/Account/Index"
                },
                new()
                {
                    label = _Localizer.Get("AccountTeams"),
                    value = accountTeamCount,
                    Id = "AccountTeamEntity/AccountTeam/Index"
                },
            };


            return Json(new DonutChartDto
            {
                Key = nameof(Accounts),
                Labels = data.Select(a => a.label).ToList(),
                Series = data.Select(a => a.value).ToList(),
                Total = data.Select(a => a.value).Sum(),
                Urls = data.Select(a => a.Id).ToList()
            });
        }

        #endregion

        #region Update Results

        [HttpPost]
        public IActionResult UpdateStandings()
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            DashboardAdministratorModel admin = _unitOfWork.DashboardAdministration
                .GetAdministratorbyId(auth.Fk_DashboardAdministrator, otherLang: false);

            if (admin.CanDeploy)
            {
                _updateResultsUtils.UpdateStandings();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateGames()
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            DashboardAdministratorModel admin = _unitOfWork.DashboardAdministration
                .GetAdministratorbyId(auth.Fk_DashboardAdministrator, otherLang: false);

            if (admin.CanDeploy)
            {
                _updateResultsUtils.UpdateGames();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateGameResult(int fk_GameWeak, string _365_MatchId, bool runBonus, int fk_TeamGameWeak)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            DashboardAdministratorModel admin = _unitOfWork.DashboardAdministration
                .GetAdministratorbyId(auth.Fk_DashboardAdministrator, otherLang: false);

            if (admin.CanDeploy)
            {
                _updateResultsUtils.UpdateGameResult(fk_GameWeak, fk_TeamGameWeak, _365_MatchId, runBonus);
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateAccountTeamGameWeakRanking(int fk_GameWeak)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            DashboardAdministratorModel admin = _unitOfWork.DashboardAdministration
                .GetAdministratorbyId(auth.Fk_DashboardAdministrator, otherLang: false);

            if (admin.CanDeploy)
            {
                _updateResultsUtils.UpdateAccountTeamGameWeakRanking(fk_GameWeak);
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdatePrivateLeagueRanking(int fk_GameWeak, int id)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            DashboardAdministratorModel admin = _unitOfWork.DashboardAdministration
                .GetAdministratorbyId(auth.Fk_DashboardAdministrator, otherLang: false);

            if (admin.CanDeploy)
            {
                _updateResultsUtils.UpdatePrivateLeagueRanking(fk_GameWeak, id);
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateAccountTeamPoints(
            int fk_GameWeak,
            int fk_AccountTeamGameWeak,
            int fk_Player,
            int fk_TeamGameWeak)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            DashboardAdministratorModel admin = _unitOfWork.DashboardAdministration
                .GetAdministratorbyId(auth.Fk_DashboardAdministrator, otherLang: false);

            if (admin.CanDeploy)
            {
                List<int> fk_Players = new();
                if (fk_TeamGameWeak > 0)
                {
                    fk_Players = _unitOfWork.Team.GetPlayers(new PlayerParameters
                    {
                        Fk_TeamGameWeak = fk_TeamGameWeak
                    }, false).Select(a => a.Id).ToList();
                }
                if (fk_Player > 0)
                {
                    fk_Players.Add(fk_Player);
                }

                _updateResultsUtils.UpdateAccountTeamPoints(fk_GameWeak, fk_AccountTeamGameWeak, fk_Players);
            }

            return Ok();
        }

        #endregion
    }
}
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

        public HomeController(
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

        public IActionResult Index()
        {
            //var players = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new Entities.CoreServicesModels.AccountTeamModels.AccountTeamPlayerGameWeakParameters
            //{
            //    Fk_GameWeak = 43,
            //    IsTransfer = false
            //}, otherLang: false).ToList();

            //var teams = players.GroupBy(a => a.AccountTeamPlayer.AccountTeam.Id)
            //                   .Where(a => a.Count() != 15)
            //                   .Select(a => new
            //                   {
            //                       a.Key,
            //                       count = a.Count()
            //                   })
            //                   .ToList();

            return View();
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
    }
}
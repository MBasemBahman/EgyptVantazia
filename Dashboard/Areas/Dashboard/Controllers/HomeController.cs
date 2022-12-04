namespace Dashboard.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(DashboardViewEnum.Home, AccessLevelEnum.View)]
    public class HomeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly DbContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeController(
            ILoggerManager logger,
            IMapper mapper,
            UnitOfWork unitOfWork,
            DbContext context,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {

            //var accounteams = _unitOfWork.AccountTeam.GetAccountTeams(new )

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
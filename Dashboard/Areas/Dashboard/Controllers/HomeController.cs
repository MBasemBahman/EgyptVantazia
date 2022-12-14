﻿namespace Dashboard.Areas.Dashboard.Controllers
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
            //_unitOfWork.Season.DeleteDuplicattion();
            //_unitOfWork.Save().Wait();

            //AddBulkData(192, new List<int> { 46, 43 });
            //var teams = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new Entities.CoreServicesModels.AccountTeamModels.AccountTeamPlayerGameWeakParameters
            //{
            //    Fk_GameWeak = 52,
            //    IsTransfer = false,
            //    IsPrimary = true,
            //}, false).GroupBy(a => a.AccountTeamPlayer.AccountTeam.Id)
            //         .Where(a => a.Count() != 11)
            //         .Select(a => new
            //         {
            //             a.Key,
            //             count = a.Count()
            //         })
            //         .ToList();

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

        private void AddBulkData(int fk_AccountTeam, List<int> fk_GameWeeks)
        {
            Entities.CoreServicesModels.AccountTeamModels.AccountTeamGameWeakModel accountTeamGame = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new Entities.CoreServicesModels.AccountTeamModels.AccountTeamGameWeakParameters
            {
                Fk_AccountTeam = fk_AccountTeam
            }, false).FirstOrDefault();
            if (accountTeamGame != null)
            {
                List<Entities.CoreServicesModels.AccountTeamModels.AccountTeamPlayerGameWeakModel> players = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new Entities.CoreServicesModels.AccountTeamModels.AccountTeamPlayerGameWeakParameters
                {
                    Fk_AccountTeam = fk_AccountTeam,
                    IsTransfer = false
                }, false).ToList();

                if (players.Any() && players.Count == 15)
                {
                    foreach (int fk_GameWeek in fk_GameWeeks)
                    {
                        _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new Entities.DBModels.AccountTeamModels.AccountTeamGameWeak
                        {
                            Fk_AccountTeam = fk_AccountTeam,
                            Fk_GameWeak = fk_GameWeek,
                        });

                        foreach (Entities.CoreServicesModels.AccountTeamModels.AccountTeamPlayerGameWeakModel player in players)
                        {
                            _unitOfWork.AccountTeam.CreateAccountTeamPlayerGameWeak(new Entities.DBModels.AccountTeamModels.AccountTeamPlayerGameWeak
                            {
                                Fk_AccountTeamPlayer = player.Fk_AccountTeamPlayer,
                                Fk_GameWeak = fk_GameWeek,
                                Fk_TeamPlayerType = player.Fk_TeamPlayerType,
                                IsPrimary = player.IsPrimary,
                                Order = player.Order,
                            });
                        }
                    }
                    _unitOfWork.Save().Wait();
                }
            }

        }
    }
}
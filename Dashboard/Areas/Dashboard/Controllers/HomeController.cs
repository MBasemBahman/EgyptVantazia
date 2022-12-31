using BaseDB;
using Dashboard.Areas.AccountTeamEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;

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

        public HomeController(
            ILoggerManager logger,
            IMapper mapper,
            UnitOfWork unitOfWork,
            IWebHostEnvironment environment,
            BaseContext dBContext)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _environment = environment;
            _dBContext = dBContext;
        }

        public IActionResult Index()
        {


            return View();
        }

        public IActionResult Test(int fk_GameWeak)
        {
            //var players = _dBContext.AccountTeamPlayerGameWeaks
            //                        .Where(a => a.AccountTeamPlayer.Fk_AccountTeam == 112 &&
            //                                    a.Fk_GameWeak == 42)
            //                        .ToList();
            //players.ForEach(a =>
            //{
            //    a.Points = null;
            //});
            //_dBContext.SaveChanges();

            var accountTeamGameWeaks = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
            {
                Fk_GameWeak = fk_GameWeak,
            }, otherLang: false)
                   .Select(a => new
                   {
                       a.Id,
                       a.Fk_AccountTeam
                   })
                   .ToList();

            foreach (var accountTeamGameWeak in accountTeamGameWeaks)
            {
                var players = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                {
                    Fk_AccountTeam = accountTeamGameWeak.Fk_AccountTeam,
                    Fk_GameWeak = fk_GameWeak,
                    IsTransfer = false
                }, otherLang: false).Select(a => new
                {
                    Fk_Player = a.AccountTeamPlayer.Fk_Player,
                    Fk_PlayerPosition = a.AccountTeamPlayer.Player.Fk_PlayerPosition,
                    Fk_AccountTeamPlayer = a.Fk_AccountTeamPlayer,
                    Fk_TeamPlayerType = a.Fk_TeamPlayerType,
                    Order = a.Order,
                    IsPrimary = a.IsPrimary,
                    IsParticipate = a.IsParticipate,
                    IsPlayed = a.IsPlayed,
                    Points = a.Points,
                    PlayerName = a.AccountTeamPlayer.Player.Name
                }).ToList();

                if (players.Count > 0 && players.Count != 15)
                {
                    int count = players.Count;
                }
            }

            return Ok();
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
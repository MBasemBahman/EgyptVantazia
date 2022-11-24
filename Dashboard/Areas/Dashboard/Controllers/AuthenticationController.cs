namespace Dashboard.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IAuthenticationManager _authManager;
        private readonly UnitOfWork _unitOfWork;

        public AuthenticationController(ILoggerManager logger, IMapper mapper,
        IAuthenticationManager authManager, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            var accounTeamsGameWeaks = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new Entities.CoreServicesModels.AccountTeamModels.AccountTeamGameWeakParameters
            {
                Fk_Account = 66
            }, false).ToList();

            foreach (var accounTeamsGameWeak in accounTeamsGameWeaks)
            {
                var gameweaks = _unitOfWork.Season.GetGameWeaks(new Entities.CoreServicesModels.SeasonModels.GameWeakParameters
                {
                    GameWeakTo = (accounTeamsGameWeak.GameWeak._365_GameWeakId_Parsed - 1).Value
                }, false).ToList();

                var players = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new Entities.CoreServicesModels.AccountTeamModels.AccountTeamPlayerGameWeakParameters
                {
                    Fk_AccountTeam = accounTeamsGameWeak.Fk_AccountTeam,
                    Fk_GameWeak = accounTeamsGameWeak.Fk_GameWeak,
                    IsTransfer = false
                }, false).ToList();

                if (players.Any() && players.Count == 15)
                {
                    foreach (var gameweak in gameweaks)
                    {
                        _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new Entities.DBModels.AccountTeamModels.AccountTeamGameWeak
                        {
                            Fk_AccountTeam = accounTeamsGameWeak.Fk_AccountTeam,
                            Fk_GameWeak = gameweak.Id
                        });

                        foreach (var player in players)
                        {
                            _unitOfWork.AccountTeam.CreateAccountTeamPlayerGameWeak(new Entities.DBModels.AccountTeamModels.AccountTeamPlayerGameWeak
                            {
                                Fk_AccountTeamPlayer = player.Fk_AccountTeamPlayer,
                                Fk_GameWeak = gameweak.Id,
                                Fk_TeamPlayerType = player.Fk_TeamPlayerType,
                                IsPrimary = player.IsPrimary,
                                Order = player.Order,
                            });
                        }
                    }
                    _unitOfWork.Save().Wait();
                }
            }

            string refreshToken = Request.Cookies[HeadersConstants.SetRefresh];

            try
            {
                if (!string.IsNullOrWhiteSpace(refreshToken))
                {
                    UserAuthenticatedDto auth = await _authManager.Authenticate(refreshToken, IpAddress());

                    RemoveCookies();

                    SetAccount(auth, auth.RefreshTokenResponse.Expires);
                    SetViews(auth.Fk_DashboardAdministrationRole, auth.RefreshTokenResponse.Expires);

                    SetToken(auth.TokenResponse);
                    SetRefresh(auth.RefreshTokenResponse);
                    SetAdminData(auth.Fk_DashboardAdministrator, auth.RefreshTokenResponse.Expires);


                    return Request.Headers.Referer.Any()
                        ? Redirect(Request.Headers.Referer)
                        : RedirectToAction("SetSettings", "Home", new { area = "Dashboard" });
                }
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            RemoveCookies();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserForAuthenticationDto model)
        {
            try
            {
                UserAuthenticatedDto auth = await _authManager.Authenticate(model, IpAddress());

                RemoveCookies();

                SetAccount(auth, auth.RefreshTokenResponse.Expires);
                SetViews(auth.Fk_DashboardAdministrationRole, auth.RefreshTokenResponse.Expires);

                SetToken(auth.TokenResponse);
                SetRefresh(auth.RefreshTokenResponse);
                SetAdminData(auth.Fk_DashboardAdministrator, auth.RefreshTokenResponse.Expires);

                return RedirectToAction("SetSettings", "Home", new { area = "Dashboard" });
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            try
            {
                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items["User"];

                await _unitOfWork.User.ChangePassword(auth.Id, model);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Logout));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string refresh = Request.Cookies[HeadersConstants.SetRefresh];

                if (!string.IsNullOrWhiteSpace(refresh))
                {
                    await _authManager.RevokeToken(refresh, IpAddress());
                }

                RemoveCookies();
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            return RedirectToAction(nameof(Login));
        }

        // Helper Methods
        private void SetToken(TokenResponse response)
        {
            CookieOptions option = new()
            {
                Expires = DateTime.Parse(response.Expires, CultureInfo.InvariantCulture),
            };
            Response.Cookies.Append(HeadersConstants.AuthorizationToken, response.RefreshToken, option);
        }

        private void SetRefresh(TokenResponse response)
        {
            CookieOptions option = new()
            {
                Expires = DateTime.Parse(response.Expires, CultureInfo.InvariantCulture),
            };
            Response.Cookies.Append(HeadersConstants.SetRefresh, response.RefreshToken, option);
        }

        private void SetAccount(UserAuthenticatedDto auth, string expires)
        {
            CookieOptions option = new()
            {
                Expires = DateTime.Parse(expires, CultureInfo.InvariantCulture),
            };

            Response.Cookies.Append(ViewDataConstants.AccountName, auth.Name, option);
            if (!string.IsNullOrWhiteSpace(auth.EmailAddress))
            {
                Response.Cookies.Append(ViewDataConstants.AccountEmail, auth.EmailAddress, option);
            }
        }

        private void SetViews(int fk_Role, string expires)
        {
            List<int> views = _unitOfWork.DashboardAdministration.GetViewsByRoleId(fk_Role);
            string listString = string.Join(",", views);

            CookieOptions option = new()
            {
                Expires = DateTime.Parse(expires, CultureInfo.InvariantCulture),
            };
            Response.Cookies.Append(ViewDataConstants.Views, listString, option);
        }

        private void SetAdminData(int fk_admin, string expires)
        {
            DashboardAdministratorModel Admin = _unitOfWork.DashboardAdministration
                                                    .GetAdministratorbyId(fk_admin, otherLang: false);


            CookieOptions option = new()
            {
                Expires = DateTime.Parse(expires, CultureInfo.InvariantCulture),
            };
            Response.Cookies.Append(AdminCookiesDataConstants.Role, Admin.Fk_DashboardAdministrationRole.ToString(), option);


        }

        private void RemoveCookies()
        {
            foreach (string cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
        }

        private string IpAddress()
        {
            // get source ip address for the current request
            return Request.Headers.ContainsKey("x-Forwarded-For")
                ? (string)Request.Headers["x-Forwarded-For"]
                : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}

﻿using Dashboard.Areas.AccountTeamEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.AccountTeamEntity.Controllers
{
    [Area("AccountTeamEntity")]
    [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.View)]
    public class AccountTeamController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;
        private readonly UpdateResultsUtils _updateResultsUtils;

        public AccountTeamController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork,
                 LinkGenerator linkGenerator,
                 IWebHostEnvironment environment, UpdateResultsUtils updateResultsUtils)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _linkGenerator = linkGenerator;
            _environment = environment;
            _updateResultsUtils = updateResultsUtils;
        }

        public IActionResult Index(int Fk_Account, bool ProfileLayOut = false)
        {
            AccountTeamFilter filter = new()
            {
                Fk_Account = Fk_Account
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(ProfileLayOut);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] AccountTeamFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountTeamParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<AccountTeamModel> data = await _unitOfWork.AccountTeam.GetAccountTeamPaged(parameters, otherLang);

            List<AccountTeamDto> resultDto = _mapper.Map<List<AccountTeamDto>>(data);

            DataTable<AccountTeamDto> dataTableManager = new();

            DataTableResult<AccountTeamDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.AccountTeam.GetAccountTeamCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.CreateOrEdit)]
        public IActionResult EditAccountTeamsCards([FromQuery] string fk_AccountTeams)
        {
            List<int> fk_AccountTeamsIds = !string.IsNullOrEmpty(fk_AccountTeams) ?
                fk_AccountTeams.Split(",").Select(int.Parse).ToList() : new List<int>();

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (fk_AccountTeamsIds.Any())
            {
                ViewData["AccountTeams"] = _unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters
                {
                    Fk_AccountTeams = fk_AccountTeamsIds
                }, otherLang).ToList();
            }

            AccountTeamsUpdateCards model = new()
            {
                BenchBoost = 0,
                FreeHit = 0,
                WildCard = 0,
                DoubleGameWeak = 0,
                Top_11 = 0,
                FreeTransfer = 0,
                TwiceCaptain = 0,
                TripleCaptain = 0,
                Fk_AccounTeams = fk_AccountTeamsIds
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.CreateOrEdit)]
        public IActionResult EditAccountTeamsCards(AccountTeamsUpdateCards updateCards)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            DashboardAdministratorModel admin = _unitOfWork.DashboardAdministration
                .GetAdministratorbyId(auth.Fk_DashboardAdministrator, otherLang: false);

            if (admin.CanDeploy)
            {
                if (updateCards.BenchBoost > 0 ||
                    updateCards.FreeHit > 0 ||
                    updateCards.WildCard > 0 ||
                    updateCards.DoubleGameWeak > 0 ||
                    updateCards.Top_11 > 0 ||
                    updateCards.FreeTransfer > 0 ||
                    updateCards.TwiceCaptain > 0 ||
                    updateCards.TripleCaptain > 0)
                {
                    _updateResultsUtils.UpdateAccountTeamUpdateCards(updateCards);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Profile(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountTeamDto data = _mapper.Map<AccountTeamDto>(_unitOfWork.AccountTeam
                                                           .GetAccountTeambyId(id, otherLang));


            data.AccountTeamGameWeaks = _mapper.Map<List<AccountTeamGameWeakDto>>
                (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_AccountTeam = id
                }, otherLang));

            data.PlayerTransfers = _mapper.Map<List<PlayerTransferDto>>
                (_unitOfWork.PlayerTransfers.GetPlayerTransfers(new PlayerTransferParameters
                {
                    Fk_AccountTeam = id
                }, otherLang));

            data.AccountTeamPlayers = _mapper.Map<List<AccountTeamPlayerDto>>
                (_unitOfWork.AccountTeam.GetAccountTeamPlayers(new AccountTeamPlayerParameters
                {
                    Fk_AccountTeam = id
                }, otherLang));

            ViewData["auth"] = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            return View(data);
        }

        [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int fk_Account, int id = 0)
        {
            AccountTeamCreateOrEditModel model = new()
            {
                Fk_Account = fk_Account
            };

            if (id > 0)
            {
                AccountTeam accountTeamDB = await _unitOfWork.AccountTeam.FindAccountTeambyId(id, trackChanges: false);
                model = _mapper.Map<AccountTeamCreateOrEditModel>(accountTeamDB);
                model.ImageUrl = accountTeamDB.StorageUrl + accountTeamDB.ImageUrl;
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "calendar-date.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }

            SetViewData(ProfileLayOut: true, model.Fk_Season);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, AccountTeamCreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                SetViewData(ProfileLayOut: false, model.Fk_Season);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                AccountTeam accountTeamDB = new();
                if (id == 0)
                {
                    accountTeamDB = _mapper.Map<AccountTeam>(model);

                    accountTeamDB.CreatedBy = auth.UserName;

                    _unitOfWork.AccountTeam.CreateAccountTeam(accountTeamDB);

                }
                else
                {
                    accountTeamDB = await _unitOfWork.AccountTeam.FindAccountTeambyId(id, trackChanges: true);

                    _ = _mapper.Map(model, accountTeamDB);

                    accountTeamDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    accountTeamDB.ImageUrl = await _unitOfWork.AccountTeam.UploadAccountTeamImage(_environment.WebRootPath, imageFile);
                    accountTeamDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                return RedirectToAction("Profile", "Account", new { area = "AccountEntity", id = model.Fk_Account });
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(ProfileLayOut: false, model.Fk_Season);

            return View(model);
        }

        [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            AccountTeam data = await _unitOfWork.AccountTeam.FindAccountTeambyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.AccountTeam.DeleteAccountTeam(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool ProfileLayOut = false, int fk_Season = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            ViewData["CommunicationStatus"] = _unitOfWork.AccountTeam.GetCommunicationStatusesLookUp(new RequestParameters(), otherLang);
            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new TeamParameters
            {
                Fk_Season = fk_Season
            }, otherLang);
            ViewData["ProfileLayOut"] = ProfileLayOut;
        }

    }
}

using API.Controllers;
using CoreServices;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.AccountTeamModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamController : ExtendControllerBase
    {
        public AccountTeamController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetAccountTeams))]
        public async Task<IEnumerable<AccountTeamModel>> GetAccountTeams(
        [FromQuery] AccountTeamParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<AccountTeamModel> data = await _unitOfWork.AccountTeam.GetAccountTeamPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetAccountTeamById))]
        public AccountTeamModel GetAccountTeamById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountTeamModel data = _unitOfWork.AccountTeam.GetAccountTeambyId(id, otherLang);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetMyAccountTeam))]
        public AccountTeamModel GetMyAccountTeam()
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            if (currentSeason == null)
            {
                throw new Exception("Season not started yet!");
            }

            AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(auth.Fk_Account, currentSeason.Id);

            return currentTeam ?? throw new Exception("Please create your team!");
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<AccountTeamModel> Create([FromForm] AccountTeamCreateModel model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            GameWeakModel currentGameWeak = _unitOfWork.Season.GetCurrentGameWeak();

            AccountTeam accountTeam = _mapper.Map<AccountTeam>(model);
            accountTeam.CreatedBy = auth.Name;
            accountTeam.Fk_Account = auth.Fk_Account;
            accountTeam.Fk_Season = currentSeason.Id;
            accountTeam.TotalMoney = 100;
            accountTeam.AccountTeamGameWeaks = new List<AccountTeamGameWeak>
            {
                new AccountTeamGameWeak
                {
                    Fk_GameWeak = currentGameWeak.Id
                }
            };

            if (model.ImageFile != null)
            {
                accountTeam.ImageUrl = await _unitOfWork.AccountTeam.UploadAccountTeamImage(_environment.WebRootPath, model.ImageFile);
                accountTeam.StorageUrl = GetBaseUri();
            }

            _unitOfWork.AccountTeam.CreateAccountTeam(accountTeam);
            await _unitOfWork.Save();

            return _unitOfWork.AccountTeam.GetAccountTeambyId(accountTeam.Id, otherLang);
        }

        [HttpPut]
        [Route(nameof(Edit))]
        public async Task<AccountTeamModel> Edit(
            [FromQuery, BindRequired] int id,
            [FromForm] AccountTeamCreateModel model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(id, trackChanges: true);

            _ = _mapper.Map(model, accountTeam);
            accountTeam.LastModifiedBy = auth.Name;

            if (model.ImageFile != null)
            {
                accountTeam.ImageUrl = await _unitOfWork.AccountTeam.UploadAccountTeamImage(_environment.WebRootPath, model.ImageFile);
                accountTeam.StorageUrl = GetBaseUri();
            }

            await _unitOfWork.Save();

            return _unitOfWork.AccountTeam.GetAccountTeambyId(accountTeam.Id, otherLang);
        }

    }
}

using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Contracts.EnumData.DBModelsEnum;

namespace API.Areas.PrivateLeagueArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PrivateLeague")]
    [ApiExplorerSettings(GroupName = "PrivateLeague")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PrivateLeagueMemberController : ExtendControllerBase
    {
        public PrivateLeagueMemberController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPrivateLeagueMembers))]
        public async Task<IEnumerable<PrivateLeagueMemberModel>> GetPrivateLeagueMembers(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
        [FromQuery] PrivateLeagueMemberParameters parameters)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            if (!(parameters.Fk_PrivateLeague > 0 || parameters.Fk_Account > 0))
            {
                throw new Exception("Not Valid!");
            }

            _365CompetitionsEnum = (_365CompetitionsEnum)auth.Season._365_CompetitionsId.ParseToInt();

            int season = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);
            parameters.Fk_Season = season;
            parameters.HaveTeam = true;
            parameters.IgnoreZeroPoints = true;

            if (parameters.Fk_PrivateLeague == (int)PrivateLeagueEnum.OfficialLeague)
            {
                parameters.IgnoreGoldSubscription = true;
            }

            PagedList<PrivateLeagueMemberModel> data = await _unitOfWork.PrivateLeague.GetPrivateLeagueMemberPaged(parameters);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<bool> Delete([FromQuery, BindRequired] int id)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            if (!_unitOfWork.PrivateLeague.GetPrivateLeagueMembers(new PrivateLeagueMemberParameters
            {
                Fk_Account = auth.Fk_Account,
                Fk_PrivateLeague = id,
                IsAdmin = true
            }).Any())
            {
                throw new Exception("Not Auth!");
            }
            await _unitOfWork.PrivateLeague.DeletePrivateLeagueMember(id);
            await _unitOfWork.Save();

            return true;
        }

        [HttpPost]
        [Route(nameof(JoinPrivateLeague))]
        public async Task<PrivateLeagueModel> JoinPrivateLeague(
        [FromQuery]_365CompetitionsEnum _365CompetitionsEnum,
        [FromQuery, BindRequired] string uniqueCode)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            int currentSeason = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);
            if (currentSeason < 0)
            {
                throw new Exception("Season not started yet!");
            }

            AccountTeamModelForCalc currentTeam = _unitOfWork.AccountTeam.GetAccountTeamsForCalc(new AccountTeamParameters
            {
                Fk_Account = auth.Fk_Account,
                Fk_Season = currentSeason
            }).FirstOrDefault();
            if (currentTeam == null)
            {
                throw new Exception("Please create your team!");
            }

            int fk_PrivateLeague = _unitOfWork.PrivateLeague
                                              .GetPrivateLeagues(new PrivateLeagueParameters { UniqueCode = uniqueCode }, otherLang)
                                              .Select(a => a.Id)
                                              .FirstOrDefault();
            if (fk_PrivateLeague == 0)
            {
                throw new Exception("The code is incorrect!");
            }

            _unitOfWork.PrivateLeague.CreatePrivateLeagueMember(new PrivateLeagueMember
            {
                Fk_PrivateLeague = fk_PrivateLeague,
                Fk_Account = auth.Fk_Account
            });
            await _unitOfWork.Save();

            PrivateLeagueModel data = _unitOfWork.PrivateLeague.GetPrivateLeagues(new PrivateLeagueParameters { Id = fk_PrivateLeague }, otherLang).FirstOrDefault();

            return data;
        }
    }
}

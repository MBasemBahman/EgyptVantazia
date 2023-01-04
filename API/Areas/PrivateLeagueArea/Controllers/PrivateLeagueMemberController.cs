﻿using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.PrivateLeagueModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [FromQuery] PrivateLeagueMemberParameters parameters)
        {
            if (!(parameters.Fk_PrivateLeague > 0 || parameters.Fk_Account > 0))
            {
                throw new Exception("Not Valid!");
            }

            Entities.CoreServicesModels.SeasonModels.SeasonModel season = _unitOfWork.Season.GetCurrentSeason();
            parameters.Fk_Season = season.Id;

            PagedList<PrivateLeagueMemberModel> data = await _unitOfWork.PrivateLeague.GetPrivateLeagueMemberPaged(parameters);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<bool> Create([FromBody] PrivateLeagueMemberCreateModel model)
        {
            if (model.Fk_PrivateLeague <= 0)
            {
                throw new Exception("Not Valid!");
            }

            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            if (!_unitOfWork.PrivateLeague.GetPrivateLeagueMembers(new PrivateLeagueMemberParameters
            {
                Fk_Account = auth.Fk_Account,
                Fk_PrivateLeague = model.Fk_PrivateLeague,
                IsAdmin = true
            }).Any())
            {
                throw new Exception("Not Auth!");
            }

            if (model.Fk_Accounts != null && model.Fk_Accounts.Any())
            {
                foreach (int fk_Account in model.Fk_Accounts)
                {
                    _unitOfWork.PrivateLeague.CreatePrivateLeagueMember(new PrivateLeagueMember
                    {
                        Fk_Account = fk_Account,
                        Fk_PrivateLeague = model.Fk_PrivateLeague,
                        CreatedBy = auth.Name
                    });
                }
            }

            await _unitOfWork.Save();

            return true;
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
        [FromQuery, BindRequired] string uniqueCode)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            if (currentSeason == null)
            {
                throw new Exception("Season not started yet!");
            }

            AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(auth.Fk_Account, currentSeason.Id);
            if (currentTeam == null)
            {
                throw new Exception("Please create your team!");
            }

            int fk_PrivateLeague = _unitOfWork.PrivateLeague.GetPrivateLeagues(new PrivateLeagueParameters { UniqueCode = uniqueCode }, otherLang)
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

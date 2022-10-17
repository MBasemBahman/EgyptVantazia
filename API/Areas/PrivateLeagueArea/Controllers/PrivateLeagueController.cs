﻿using API.Controllers;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.PrivateLeagueArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PrivateLeague")]
    [ApiExplorerSettings(GroupName = "PrivateLeague")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PrivateLeagueController : ExtendControllerBase
    {
        public PrivateLeagueController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPrivateLeagues))]
        public async Task<IEnumerable<PrivateLeagueModel>> GetPrivateLeagues(
        [FromQuery] PrivateLeagueParameters parameters)
        {
            PagedList<PrivateLeagueModel> data = await _unitOfWork.PrivateLeague.GetPrivateLeaguePaged(parameters);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetPrivateLeagueById))]
        public PrivateLeagueModel GetPrivateLeagueById(
        [FromQuery, BindRequired] int id)
        {
            PrivateLeagueModel data = _unitOfWork.PrivateLeague.GetPrivateLeaguebyId(id);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetPrivateLeagueByUniqueCode))]
        public PrivateLeagueModel GetPrivateLeagueByUniqueCode(
        [FromQuery, BindRequired] string uniqueCode)
        {
            PrivateLeagueModel data = _unitOfWork.PrivateLeague.GetPrivateLeagues(new PrivateLeagueParameters { UniqueCode = uniqueCode }).FirstOrDefault();

            if (data == null)
            {
                throw new Exception("الكود غير صحيح!");
            }

            return data;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<PrivateLeagueModel> Create([FromBody] PrivateLeagueCreateModel model)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            PrivateLeague privateLeague = _mapper.Map<PrivateLeague>(model);
            privateLeague.CreatedBy = auth.Name;
            privateLeague.PrivateLeagueMembers = new List<PrivateLeagueMember>
            {
                new PrivateLeagueMember
                {
                    Fk_Account = auth.Fk_Account,
                    CreatedBy = auth.Name,
                    IsAdmin = true
                }
            };

            if (model.Fk_Accounts != null && model.Fk_Accounts.Any())
            {
                foreach (int fk_Account in model.Fk_Accounts)
                {
                    privateLeague.PrivateLeagueMembers.Add(new PrivateLeagueMember
                    {
                        Fk_Account = fk_Account,
                        CreatedBy = auth.Name
                    });
                }
            }

            _unitOfWork.PrivateLeague.CreatePrivateLeague(privateLeague);
            await _unitOfWork.Save();

            return _unitOfWork.PrivateLeague.GetPrivateLeaguebyId(privateLeague.Id);
        }

        [HttpPut]
        [Route(nameof(Edit))]
        public async Task<PrivateLeagueModel> Edit(
            [FromQuery, BindRequired] int id,
            [FromBody] PrivateLeagueCreateModel model)
        {
            if (id <= 0)
            {
                throw new Exception("Not Valid!");
            }

            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            PrivateLeague privateLeague = await _unitOfWork.PrivateLeague.FindPrivateLeaguebyId(id, trackChanges: true);

            _ = _mapper.Map(model, privateLeague);

            privateLeague.LastModifiedBy = auth.Name;

            await _unitOfWork.Save();

            return _unitOfWork.PrivateLeague.GetPrivateLeaguebyId(privateLeague.Id);
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
            await _unitOfWork.PrivateLeague.DeletePrivateLeague(id);
            await _unitOfWork.Save();

            return true;
        }
    }
}

﻿using API.Controllers;
using CoreServices;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayersTransfersModels;
using static Entities.EnumData.LogicEnumData;

namespace API.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamPlayerController : ExtendControllerBase
    {
        public AccountTeamPlayerController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetAccountTeamPlayers))]
        public async Task<IEnumerable<AccountTeamPlayerModel>> GetAccountTeamPlayers(
        [FromQuery] AccountTeamPlayerParameters parameters)
        {
            if (parameters.IncludeNextMatch)
            {
                GameWeakModel gameWeak = _unitOfWork.Season.GetNextGameWeak();
                parameters.Fk_NextGameWeak = gameWeak.Id;
            }

            if (parameters.IncludeScore && parameters.Fk_SeasonForScore == 0)
            {
                if (parameters.Fk_GameWeakForScore == 0)
                {
                    parameters.Fk_SeasonForScore = _unitOfWork.Season.GetCurrentSeason().Id;
                    parameters.Fk_GameWeakForScore = _unitOfWork.Season.GetCurrentGameWeak().Id;
                }
            }

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<AccountTeamPlayerModel> data = await _unitOfWork.AccountTeam.GetAccountTeamPlayerPaged(parameters, otherLang);

            if (parameters.IsCurrent == true && !data.Any())
            {
                parameters.IsCurrent = false;
                GameWeakModel gameWeak = _unitOfWork.Season.GetPrevGameWeak();
                parameters.Fk_GameWeak = gameWeak.Id;

                data = await _unitOfWork.AccountTeam.GetAccountTeamPlayerPaged(parameters, otherLang);
            }

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<bool> Create([FromBody] AccountTeamPlayerBulkCreateModel model)
        {
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            if (currentSeason == null)
            {
                throw new Exception("Season not started yet!");
            }

            GameWeakModel currentGameWeak = _unitOfWork.Season.GetCurrentGameWeak();

            if (currentGameWeak == null)
            {
                throw new Exception("Game Weak not started yet!");
            }

            AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(auth.Fk_Account, currentSeason.Id);

            if (currentTeam == null)
            {
                throw new Exception("Please create your team!");
            }

            AccountTeamGameWeakModel currentTeamGameWeak = _unitOfWork.AccountTeam.GetCurrentTeamGameWeak(auth.Fk_Account, currentSeason.Id);

            if (currentTeamGameWeak == null)
            {
                _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new AccountTeamGameWeak
                {
                    Fk_AccountTeam = currentTeam.Id,
                    Fk_GameWeak = currentGameWeak.Id
                });
            }

            if (model.Players != null && model.Players.Any())
            {
                var prices = _unitOfWork.Team.GetPlayers(new PlayerParameters
                {
                    Fk_Players = model.Players.Select(a => a.Fk_Player).ToList(),
                }, otherLang: false)
                    .Select(a => new
                    {
                        a.Id,
                        a.BuyPrice
                    }).ToList();

                int totalPrice = 0;

                foreach (AccountTeamPlayerCreateModel player in model.Players)
                {
                    int price = (int)prices.Where(a => a.Id == player.Fk_Player).Select(a => a.BuyPrice).FirstOrDefault();
                    _unitOfWork.PlayerTransfers.CreatePlayerTransfer(new PlayerTransfer
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_GameWeak = currentGameWeak.Id,
                        Fk_Player = player.Fk_Player,
                        TransferTypeEnum = TransferTypeEnum.Buying,
                        Cost = price
                    });

                    _unitOfWork.AccountTeam.CreateAccountTeamPlayer(new AccountTeamPlayer
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_Player = player.Fk_Player,
                        AccountTeamPlayerGameWeaks = new List<AccountTeamPlayerGameWeak>
                        {
                            new AccountTeamPlayerGameWeak
                            {
                                Fk_GameWeak = currentGameWeak.Id,
                                Fk_TeamPlayerType = player.Fk_TeamPlayerType,
                                IsPrimary = player.IsPrimary,
                                Order = player.Order
                            }
                        }
                    });

                    totalPrice += price;
                }
                await _unitOfWork.Save();

                AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);
                accountTeam.TotalMoney -= totalPrice;

                await _unitOfWork.Save();
            }
            return true;
        }
    }
}

using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayersTransfersModels;
using static Contracts.EnumData.DBModelsEnum;
using static Entities.EnumData.LogicEnumData;

namespace API.Areas.PlayerTransferArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerTransfer")]
    [ApiExplorerSettings(GroupName = "PlayerTransfer")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerTransferController : ExtendControllerBase
    {
        public PlayerTransferController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayerTransfers))]
        public async Task<IEnumerable<PlayerTransferModel>> GetPlayerTransfers(
        [FromQuery] PlayerTransferParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            parameters.Fk_Season = auth.Fk_Season;

            PagedList<PlayerTransferModel> data = await _unitOfWork.PlayerTransfers.GetPlayerTransferPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<bool> Create(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromBody] PlayerTransferBulkCreateModel model)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            _365CompetitionsEnum = (_365CompetitionsEnum)auth.Season._365_CompetitionsId.ParseToInt();

            int currentSeason = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);
            if (currentSeason < 0)
            {
                throw new Exception("Season not started yet!");
            }

            int nextGameWeakId = _unitOfWork.Season.GetNextGameWeakId(_365CompetitionsEnum);
            if (nextGameWeakId < 0)
            {
                throw new Exception("Game Weak not started yet!");
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

            AccountTeamGameWeakModelForCalc teamGameWeak = _unitOfWork.AccountTeam.GetAccountTeamGameWeaksForCalc(new AccountTeamGameWeakParameters
            {
                Fk_Account = auth.Fk_Account,
                Fk_AccountTeam = currentTeam.Id,
                Fk_GameWeak = nextGameWeakId
            }).FirstOrDefault();
            if (teamGameWeak == null)
            {
                _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new AccountTeamGameWeak
                {
                    Fk_AccountTeam = currentTeam.Id,
                    Fk_GameWeak = nextGameWeakId
                });
            }

            if (model.SellPlayers.Count != model.BuyPlayers.Count)
            {
                throw new Exception("The transfer is not successful. The number of players sold is not the same as the number of players bought!");
            }

            List<int> sellPlayers = model.SellPlayers.Select(a => a.Fk_Player).ToList();
            List<int> buyPlayers = model.BuyPlayers.Select(a => a.Fk_Player).ToList();

            List<int> fk_Players = new();
            fk_Players.AddRange(sellPlayers);
            fk_Players.AddRange(buyPlayers);

            if (sellPlayers.Distinct().Count() != sellPlayers.Count ||
                buyPlayers.Distinct().Count() != buyPlayers.Count)
            {
                throw new Exception("The players must be distinct!");
            }

            if (sellPlayers.Any(buyPlayers.Contains) ||
                buyPlayers.Any(sellPlayers.Contains))
            {
                throw new Exception("The players must be distinct!");
            }

            var prices = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_Players = fk_Players,
                Fk_AccountTeam = currentTeam.Id,
                CheckLastTransfer = true,
            }, otherLang: false)
                   .Select(a => new
                   {
                       a.Id,
                       a.BuyPrice,
                       a.SellPrice,
                       a.LastTransferTypeEnum
                   }).ToList();

            double sellMoney = prices.Where(a => sellPlayers.Contains(a.Id)).Select(a => a.SellPrice).Sum();
            double buyMoney = prices.Where(a => buyPlayers.Contains(a.Id)).Select(a => a.BuyPrice).Sum();

            if ((currentTeam.TotalMoney + sellMoney) < buyMoney)
            {
                throw new Exception("You not have money for transfers!");
            }

            List<SellPlayerModel> sellPlayersList = new();
            AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);

            if (model.SellPlayers != null && model.SellPlayers.Any())
            {
                double totalPrice = 0;

                List<SellPlayerModel> accountTeamPlayers = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                {
                    Fk_AccountTeam = currentTeam.Id,
                    Fk_Players = sellPlayers,
                    Fk_Season = currentSeason,
                    Fk_GameWeak = nextGameWeakId,
                    IsTransfer = false,
                }).Select(a => new SellPlayerModel
                {
                    Id = a.Id,
                    Fk_Player = a.AccountTeamPlayer.Fk_Player,
                    Fk_TeamPlayerType = a.Fk_TeamPlayerType,
                    IsPrimary = a.IsPrimary,
                    Order = a.Order
                })
                  .ToList();

                foreach (PlayerTransferSellModel player in model.SellPlayers)
                {
                    SellPlayerModel accountTeamPlayer = accountTeamPlayers.Where(a => a.Fk_Player == player.Fk_Player).FirstOrDefault();

                    if (accountTeamPlayer == null)
                    {
                        throw new Exception("Please select correct players!");
                    }

                    if (accountTeamPlayer != null)
                    {
                        sellPlayersList.Add(accountTeamPlayer);

                        AccountTeamPlayerGameWeak accountTeamPlayerGameWeak = await _unitOfWork.AccountTeam.FindAccountTeamPlayerGameWeakbyId(accountTeamPlayer.Id, trackChanges: true);
                        accountTeamPlayerGameWeak.IsTransfer = true;

                        double price = prices.Where(a => a.Id == player.Fk_Player)
                                             .Select(a => a.SellPrice)
                                             .FirstOrDefault();
                        _unitOfWork.PlayerTransfers.CreatePlayerTransfer(new PlayerTransfer
                        {
                            Fk_AccountTeam = currentTeam.Id,
                            Fk_GameWeak = nextGameWeakId,
                            Fk_Player = player.Fk_Player,
                            Cost = price,
                            TransferTypeEnum = TransferTypeEnum.Selling
                        });

                        totalPrice += price;
                    }
                }

                accountTeam.TotalMoney += totalPrice;
            }
            if (model.BuyPlayers != null && model.BuyPlayers.Any())
            {
                double totalPrice = 0;
                int index = 0;
                int freeTransfer = currentTeam.FreeTransfer;
                foreach (PlayerTransferBuyModel player in model.BuyPlayers)
                {
                    double price = prices.Where(a => a.Id == player.Fk_Player).Select(a => a.BuyPrice).FirstOrDefault();

                    _unitOfWork.PlayerTransfers.CreatePlayerTransfer(new PlayerTransfer
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_GameWeak = nextGameWeakId,
                        Fk_Player = player.Fk_Player,
                        TransferTypeEnum = TransferTypeEnum.Buying,
                        Cost = price,
                        IsFree = freeTransfer-- > 0
                    });

                    SellPlayerModel sellPlayer = sellPlayersList[index++];

                    _unitOfWork.AccountTeam.CreateAccountTeamPlayer(new AccountTeamPlayer
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_Player = player.Fk_Player,
                        AccountTeamPlayerGameWeaks = new List<AccountTeamPlayerGameWeak>
                        {
                            new AccountTeamPlayerGameWeak
                            {
                                Fk_GameWeak = nextGameWeakId,
                                Fk_TeamPlayerType = sellPlayer.Fk_TeamPlayerType,
                                IsPrimary = sellPlayer.IsPrimary,
                                Order = sellPlayer.Order,
                            }
                        }
                    });

                    totalPrice += price;
                }

                accountTeam.TotalMoney -= totalPrice;
                accountTeam.FreeTransfer = freeTransfer;

                if (teamGameWeak.WildCard == false &&
                    teamGameWeak.FreeHit == false)
                {
                    if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                    {
                        Fk_AccountTeam = currentTeam.Id,
                    }, otherLang: false).Count() > 1)
                    {
                        AccountTeamGameWeak accountTeamGameWeak = await _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);
                        accountTeamGameWeak.TansfarePoints = accountTeam.FreeTransfer >= 0 ? 0 : accountTeam.FreeTransfer * 4;
                    }
                }
                else
                {
                    accountTeam.FreeTransfer = 0;

                    if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                    {
                        Fk_AccountTeam = currentTeam.Id,
                    }, otherLang: false).Count() > 1)
                    {
                        AccountTeamGameWeak accountTeamGameWeak = await _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);
                        accountTeamGameWeak.TansfarePoints = 0;
                    }
                }
            }

            await _unitOfWork.Save();

            _unitOfWork.AccountTeam.UpdateAccountTeamTotalTeamPrice(currentTeam.Id, nextGameWeakId);

            return true;
        }
    }
}

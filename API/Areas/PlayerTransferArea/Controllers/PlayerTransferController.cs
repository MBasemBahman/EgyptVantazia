using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayersTransfersModels;
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

            PagedList<PlayerTransferModel> data = await _unitOfWork.PlayerTransfers.GetPlayerTransferPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<bool> Create([FromBody] PlayerTransferBulkCreateModel model)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            if (currentSeason == null)
            {
                throw new Exception("Season not started yet!");
            }

            GameWeakModel nextGameWeak = _unitOfWork.Season.GetNextGameWeak();
            if (nextGameWeak == null)
            {
                throw new Exception("Game Weak not started yet!");
            }

            AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(auth.Fk_Account, currentSeason.Id);
            if (currentTeam == null)
            {
                throw new Exception("Please create your team!");
            }

            AccountTeamGameWeakModel teamGameWeak = _unitOfWork.AccountTeam.GetTeamGameWeak(auth.Fk_Account, nextGameWeak.Id);
            if (teamGameWeak == null)
            {
                throw new Exception("Game Weak not started yet!");
            }

            if (currentTeam.TotalMoney <= 0)
            {
                throw new Exception("You not have money for transfers!");
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

            var prices = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_Players = fk_Players,
            }, otherLang: false)
                   .Select(a => new
                   {
                       a.Id,
                       a.BuyPrice,
                       a.SellPrice
                   }).ToList();

            double sellMoney = prices.Where(a => sellPlayers.Contains(a.Id)).Select(a => a.SellPrice).Sum();
            double buyMoney = prices.Where(a => buyPlayers.Contains(a.Id)).Select(a => a.BuyPrice).Sum();

            if ((currentTeam.TotalMoney + sellMoney) < buyMoney)
            {
                throw new Exception("You not have money for transfers!");
            }

            List<SellPlayerModel> sellPlayersList = new();

            if (model.SellPlayers != null && model.SellPlayers.Any())
            {
                double totalPrice = 0;
                foreach (PlayerTransferSellModel player in model.SellPlayers)
                {
                    SellPlayerModel accountTeamPlayer = _unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_Player = player.Fk_Player,
                        Fk_Season = currentSeason.Id,
                        Fk_GameWeak = teamGameWeak.Fk_GameWeak,
                        IsTransfer = false,
                    }, otherLang: false)
                        .OrderByDescending(a => a.Id)
                        .Select(a => new SellPlayerModel
                        {
                            Id = a.Id,
                            Fk_TeamPlayerType = a.Fk_TeamPlayerType,
                            IsPrimary = a.IsPrimary,
                            Order = a.Order
                        })
                        .FirstOrDefault();

                    if (accountTeamPlayer == null)
                    {
                        throw new Exception("Please select correct players!");
                    }

                    if (accountTeamPlayer != null)
                    {
                        sellPlayersList.Add(accountTeamPlayer);

                        AccountTeamPlayerGameWeak accountTeamPlayerGameWeak = await _unitOfWork.AccountTeam.FindAccountTeamPlayerGameWeakbyId(accountTeamPlayer.Id, trackChanges: true);
                        accountTeamPlayerGameWeak.IsTransfer = true;

                        double price = prices.Where(a => a.Id == player.Fk_Player).Select(a => a.SellPrice).FirstOrDefault();
                        _unitOfWork.PlayerTransfers.CreatePlayerTransfer(new PlayerTransfer
                        {
                            Fk_AccountTeam = currentTeam.Id,
                            Fk_GameWeak = nextGameWeak.Id,
                            Fk_Player = player.Fk_Player,
                            Cost = price,
                            TransferTypeEnum = TransferTypeEnum.Selling
                        });

                        totalPrice += price;
                    }
                }

                AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);
                accountTeam.TotalMoney += totalPrice;

                //_unitOfWork.Save().Wait();
            }
            if (model.BuyPlayers != null && model.BuyPlayers.Any())
            {
                if (teamGameWeak == null)
                {
                    _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new AccountTeamGameWeak
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_GameWeak = nextGameWeak.Id
                    });
                }

                int totalPrice = 0;
                int index = 0;
                int freeTransfer = currentTeam.FreeTransfer;
                foreach (PlayerTransferBuyModel player in model.BuyPlayers)
                {
                    int price = (int)prices.Where(a => a.Id == player.Fk_Player).Select(a => a.BuyPrice).FirstOrDefault();

                    _unitOfWork.PlayerTransfers.CreatePlayerTransfer(new PlayerTransfer
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_GameWeak = nextGameWeak.Id,
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
                                Fk_GameWeak = nextGameWeak.Id,
                                Fk_TeamPlayerType = sellPlayer.Fk_TeamPlayerType,
                                IsPrimary = sellPlayer.IsPrimary,
                                Order = sellPlayer.Order,
                            }
                        }
                    });

                    totalPrice += price;
                }
                //await _unitOfWork.Save();

                AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);
                accountTeam.TotalMoney -= totalPrice;
                accountTeam.FreeTransfer = freeTransfer;

                teamGameWeak ??= _unitOfWork.AccountTeam.GetTeamGameWeak(auth.Fk_Account, nextGameWeak.Id);

                if (teamGameWeak.WildCard == false &&
                    _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                    {
                        Fk_AccountTeam = currentTeam.Id,
                    }, otherLang: false).Count() > 1)
                {
                    AccountTeamGameWeak accountTeamGameWeak = await _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);
                    accountTeamGameWeak.TansfarePoints = accountTeam.FreeTransfer >= 0 ? 0 : accountTeam.FreeTransfer * 4;
                }

            }

            await _unitOfWork.Save();

            return true;
        }
    }
}

using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayersTransfersModels;
using FantasyLogic;
using static Contracts.EnumData.DBModelsEnum;
using static Entities.EnumData.LogicEnumData;

namespace API.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamPlayerController : ExtendControllerBase
    {
        private readonly FantasyUnitOfWork _fantasyUnitOfWork;

        public AccountTeamPlayerController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings,
        FantasyUnitOfWork fantasyUnitOfWork) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
            _fantasyUnitOfWork = fantasyUnitOfWork;
        }

        [HttpGet]
        [Route(nameof(GetAccountTeamPlayers))]
        public async Task<IEnumerable<AccountTeamPlayerModel>> GetAccountTeamPlayers(
        [FromQuery] AccountTeamPlayerParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            GameWeakModel currentGamWeak = null;
            GameWeakModel nextGameWeak = null;

            if (parameters.Fk_GameWeak > 0)
            {
                currentGamWeak = _unitOfWork.Season.GetGameWeakbyId(parameters.Fk_GameWeak, otherLang);

                nextGameWeak = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
                {
                    _365_GameWeakId = (currentGamWeak._365_GameWeakIdValue + 1).ToString()
                }, otherLang).FirstOrDefault();
            }
            else
            {
                currentGamWeak = _unitOfWork.Season.GetCurrentGameWeak();
                nextGameWeak = _unitOfWork.Season.GetNextGameWeak();
            }

            parameters.IsTransfer = false;

            if (parameters.IncludeNextMatch)
            {
                parameters.FromDeadLine = currentGamWeak.Deadline;
                parameters.ToDeadLine = nextGameWeak.Deadline;

                if (parameters.ToDeadLine == null)
                {
                    parameters.ToDeadLine = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
                    {
                        Fk_GameWeak = currentGamWeak.Id
                    }, otherLang: false).OrderByDescending(a => a.StartTime)
                                        .Select(a => a.StartTime.AddHours(2))
                                        .FirstOrDefault();
                }
            }

            if (parameters.IsCurrent == true)
            {
                parameters.Fk_GameWeak = currentGamWeak.Id;
            }
            else if (parameters.IsNextGameWeak == true)
            {
                parameters.Fk_GameWeak = nextGameWeak.Id;

                if (parameters.IncludeNextMatch)
                {
                    GameWeakModel nextNextGameWeak = _unitOfWork.Season.GetNextNextGameWeak();

                    parameters.FromDeadLine = nextGameWeak.Deadline;
                    parameters.ToDeadLine = nextNextGameWeak.Deadline;

                    if (parameters.ToDeadLine == null)
                    {
                        parameters.ToDeadLine = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
                        {
                            Fk_GameWeak = nextGameWeak.Id
                        }, otherLang: false).OrderByDescending(a => a.StartTime)
                                            .Select(a => a.StartTime.AddHours(2))
                                            .FirstOrDefault();
                    }
                }
            }

            if (parameters.IncludeScore && parameters.Fk_SeasonForScore == 0)
            {
                if (parameters.Fk_GameWeakForScore == 0)
                {
                    parameters.Fk_SeasonForScore = currentGamWeak.Fk_Season;
                    parameters.Fk_GameWeakForScore = currentGamWeak.Id;
                }
            }

            PagedList<AccountTeamPlayerModel> data = await _unitOfWork.AccountTeam.GetAccountTeamPlayerPaged(parameters, otherLang);

            if (parameters.Fk_AccountTeam > 0 && parameters.IsNextGameWeak != true)
            {
                var accountTeam = _unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters
                {
                    Id = parameters.Fk_AccountTeam,
                    Fk_GameWeak = parameters.Fk_GameWeak
                }, otherLang).Select(a => new
                {
                    a.Fk_AcountTeamGameWeek
                }).FirstOrDefault();

                if (accountTeam != null && accountTeam.Fk_AcountTeamGameWeek > 0)
                {
                    AccountTeamCustemClac clac = _fantasyUnitOfWork.AccountTeamCalc.AccountTeamPlayersCalculations(accountTeam.Fk_AcountTeamGameWeek, parameters.Fk_AccountTeam, currentGamWeak, currentGamWeak.Fk_Season, false);
                    if (clac != null && clac.Players != null && clac.Players.Any())
                    {
                        data.ForEach(player =>
                        {
                            {
                                if (player.AccountTeamPlayerGameWeak != null &&
                                    clac.Players.Any(a => a.Fk_AccountTeamPlayer == player.AccountTeamPlayerGameWeak.Fk_AccountTeamPlayer))
                                {
                                    AccountTeamPlayerGameWeak newVal = clac.Players.First(a => a.Fk_AccountTeamPlayer == player.AccountTeamPlayerGameWeak.Fk_AccountTeamPlayer);

                                    player.AccountTeamPlayerGameWeak.IsPrimary = newVal.IsPrimary;
                                    player.AccountTeamPlayerGameWeak.Points = newVal.Points;
                                    player.AccountTeamPlayerGameWeak.HavePoints = newVal.HavePoints;
                                    player.AccountTeamPlayerGameWeak.HavePointsInTotal = newVal.HavePointsInTotal;
                                }
                            }
                        });
                    }
                }
            }

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
                _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new AccountTeamGameWeak
                {
                    Fk_AccountTeam = currentTeam.Id,
                    Fk_GameWeak = nextGameWeak.Id
                });
            }

            if (currentTeam.TotalMoney < 100)
            {
                throw new Exception("You not have enough money!");
            }

            if (model.Players != null && model.Players.Any())
            {
                model.Players.ForEach(a =>
                {
                    a.Fk_PlayerPosition = _unitOfWork.AccountTeam.GetAccountTeamPlayers(new AccountTeamPlayerParameters
                    {
                        Fk_Player = a.Fk_Player
                    }, false).Select(b => b.Player.Fk_PlayerPosition).First();
                });

                List<AccountTeamCheckStructureModel> players = _mapper.Map<List<AccountTeamCheckStructureModel>>(model.Players);

                CheckPlayerStructure(players);

                var prices = _unitOfWork.Team.GetPlayers(new PlayerParameters
                {
                    Fk_Players = model.Players.Select(a => a.Fk_Player).ToList(),
                }, otherLang: false)
                    .Select(a => new
                    {
                        a.Id,
                        a.BuyPrice
                    }).ToList();

                if (prices.Select(a => a.BuyPrice).Sum() > currentTeam.TotalMoney)
                {
                    throw new Exception("You not have enough money!");
                }

                double totalPrice = 0;

                bool captain = model.Players.Any(a => a.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.Captian);
                bool viceCaptian = model.Players.Any(a => a.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.ViceCaptian);

                foreach (AccountTeamPlayerCreateModel player in model.Players.OrderByDescending(a => a.IsPrimary).ThenBy(a => a.Order))
                {
                    double price = prices.Where(a => a.Id == player.Fk_Player).Select(a => a.BuyPrice).FirstOrDefault();

                    _unitOfWork.PlayerTransfers.CreatePlayerTransfer(new PlayerTransfer
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_GameWeak = nextGameWeak.Id,
                        Fk_Player = player.Fk_Player,
                        TransferTypeEnum = TransferTypeEnum.Buying,
                        Cost = price
                    });

                    if (!captain)
                    {
                        player.Fk_TeamPlayerType = (int)TeamPlayerTypeEnum.Captian;
                        captain = true;
                    }
                    else if (!viceCaptian)
                    {
                        player.Fk_TeamPlayerType = (int)TeamPlayerTypeEnum.ViceCaptian;
                        viceCaptian = true;
                    }

                    _unitOfWork.AccountTeam.CreateAccountTeamPlayer(new AccountTeamPlayer
                    {
                        Fk_AccountTeam = currentTeam.Id,
                        Fk_Player = player.Fk_Player,
                        AccountTeamPlayerGameWeaks = new List<AccountTeamPlayerGameWeak>
                        {
                            new AccountTeamPlayerGameWeak
                            {
                                Fk_GameWeak = nextGameWeak.Id,
                                Fk_TeamPlayerType = player.Fk_TeamPlayerType,
                                IsPrimary = player.IsPrimary,
                                Order = player.Order
                            }
                        }
                    });

                    totalPrice += price;
                }
                //await _unitOfWork.Save();

                AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);
                accountTeam.TotalMoney -= totalPrice;
                await _unitOfWork.Save();
            }
            return true;
        }


        [HttpPut]
        [Route(nameof(Update))]
        public async Task<bool> Update([FromBody] AccountTeamPlayerBulkUpdateModel model)
        {
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

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
                _unitOfWork.AccountTeam.CreateAccountTeamGameWeak(new AccountTeamGameWeak
                {
                    Fk_AccountTeam = currentTeam.Id,
                    Fk_GameWeak = nextGameWeak.Id
                });
            }

            if (model.Players.Count(a => a.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.Captian) != 1)
            {
                throw new Exception("You must select one captain only!");
            }
            if (model.Players.Count(a => a.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.ViceCaptian) != 1)
            {
                throw new Exception("You must select one vice-captain only!");
            }

            if (model.Players != null && model.Players.Any())
            {
                model.Players.ForEach(a =>
                {
                    a.Fk_PlayerPosition = _unitOfWork.AccountTeam.GetAccountTeamPlayers(new AccountTeamPlayerParameters
                    {
                        Id = a.Fk_AccountTeamPlayer
                    }, false).Select(b => b.Player.Fk_PlayerPosition).First();
                });

                List<AccountTeamCheckStructureModel> players = _mapper.Map<List<AccountTeamCheckStructureModel>>(model.Players);

                CheckPlayerStructure(players);

                foreach (AccountTeamPlayerUpdateModel player in model.Players)
                {
                    _unitOfWork.AccountTeam.CreateAccountTeamPlayerGameWeak(new AccountTeamPlayerGameWeak
                    {
                        Fk_GameWeak = nextGameWeak.Id,
                        Fk_AccountTeamPlayer = player.Fk_AccountTeamPlayer,
                        Fk_TeamPlayerType = player.Fk_TeamPlayerType,
                        IsPrimary = player.IsPrimary,
                        Order = player.Order,
                    });
                }
            }
            await _unitOfWork.Save();

            return true;
        }

        private void CheckPlayerStructure(List<AccountTeamCheckStructureModel> Players)
        {
            if (Players.Count != 15)
            {
                throw new Exception("The team must have 15 players!");
            }

            if (Players.Count(a => a.IsPrimary) != 11)
            {
                throw new Exception("The team must have 11 players on the pitch!");
            }

            if (Players.Count(a => a.IsPrimary == false) != 4)
            {
                throw new Exception("The team must have 4 players on the bench!");
            }

            if (Players.Count(a => a.IsPrimary && a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper) != 1)
            {
                throw new Exception("The team must have 1 goalkeeper on the pitch!");
            }

            if (Players.Count(a => !a.IsPrimary && a.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper) != 1)
            {
                throw new Exception("The team must have 1 goalkeeper on the bench!");
            }

            if (Players.Count(a => a.IsPrimary && a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender) < 3)
            {
                throw new Exception("The team must have at least 3 defenders on the pitch!");
            }

            if (Players.Count(a => a.IsPrimary && a.Fk_PlayerPosition == (int)PlayerPositionEnum.Attacker) < 1)
            {
                throw new Exception("The team must have at least 1 attacker on the pitch!");
            }

            if (Players.Count(a => !a.IsPrimary && a.Fk_PlayerPosition == (int)PlayerPositionEnum.Defender) > 2)
            {
                throw new Exception("The team must have max 2 defenders on the bench!");
            }
        }
    }
}

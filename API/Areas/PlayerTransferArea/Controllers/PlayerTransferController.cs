using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayersTransfersModels;

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
        public async Task<PlayerTransferModel> Create([FromBody] PlayerTransferCreateModel model)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

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

            var price = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Id = model.Fk_Player,
            }, otherLang: false).Select(a => a.BuyPrice).FirstOrDefault();

            AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);
            accountTeam.TotalMoney -= (int)price;

            PlayerTransfer playerTransfer = _mapper.Map<PlayerTransfer>(model);
            playerTransfer.CreatedBy = auth.Name;
            playerTransfer.Fk_AccountTeam = currentTeam.Id;

            if (!model.IsFree)
            {
                playerTransfer.Cost = (int)price;
            }

            _unitOfWork.PlayerTransfers.CreatePlayerTransfer(playerTransfer);
            await _unitOfWork.Save();

            return _unitOfWork.PlayerTransfers.GetPlayerTransferbyId(playerTransfer.Id, otherLang);
        }
    }
}

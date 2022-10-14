using Dashboard.Areas.AccountTeamEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.AccountTeamEntity.Controllers
{
    [Area("AccountTeamEntity")]
    [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.View)]
    public class AccountTeamPlayerController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public AccountTeamPlayerController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork,
                 LinkGenerator linkGenerator,
                 IWebHostEnvironment environment)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _linkGenerator = linkGenerator;
            _environment = environment;
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountTeamPlayerDto data = _mapper.Map<AccountTeamPlayerDto>(_unitOfWork.AccountTeam
                                                           .GetAccountTeamPlayerbyId(id, otherLang));


            data.AccountTeamPlayerGameWeaks = _mapper.Map<List<AccountTeamPlayerGameWeakDto>>
                (_unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
                {
                    Fk_AccountTeamPlayer = id
                }, otherLang));
            return View(data);
        }

    }
}

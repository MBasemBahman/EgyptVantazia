using Dashboard.Areas.PrivateLeagueEntity.Models;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.PrivateLeagueEntity.Controllers
{
    [Area("PrivateLeagueEntity")]
    [Authorize(DashboardViewEnum.PrivateLeagueMember, AccessLevelEnum.View)]
    public class PrivateLeagueMemberController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PrivateLeagueMemberController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index(int Fk_PrivateLeague)
        {

            PrivateLeagueMemberFilter filter = new()
            {
                Fk_PrivateLeague = Fk_PrivateLeague
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData();
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PrivateLeagueMemberFilter dtParameters)
        {

            PrivateLeagueMemberParameters parameters = new()
            {
                SearchColumns = "Id"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PrivateLeagueMemberModel> data = await _unitOfWork.PrivateLeague.GetPrivateLeagueMemberPaged(parameters);

            List<PrivateLeagueMemberDto> resultDto = _mapper.Map<List<PrivateLeagueMemberDto>>(data);

            DataTable<PrivateLeagueMemberDto> dataTableManager = new();

            DataTableResult<PrivateLeagueMemberDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PrivateLeague.GetPrivateLeagueMemberCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        [Authorize(DashboardViewEnum.PrivateLeagueMember, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PrivateLeagueMember data = await _unitOfWork.PrivateLeague.FindPrivateLeagueMemberbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PrivateLeagueMember, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PrivateLeague.DeletePrivateLeagueMember(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        //helper methods
        private void SetViewData()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["PrivateLeague"] = _unitOfWork.PrivateLeague.GetPrivateLeagueLookUp(new PrivateLeagueParameters(), otherLang);
        }
    }
}

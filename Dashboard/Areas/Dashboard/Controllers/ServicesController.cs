using Entities.CoreServicesModels.SeasonModels;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class ServicesController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly DbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ServicesController(
            ILoggerManager logger,
            IMapper mapper,
            UnitOfWork unitOfWork,
            DbContext context,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public JsonResult GetGameWeak(int fk_Season)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            var result = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
                Fk_Season = fk_Season
            }, otherLang).OrderByDescending(a => a._365_GameWeakId).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return Json(result);
        }
    }
}

using Dashboard.Areas.NewsEntity.Models;
using Entities.CoreServicesModels.NewsModels;
using Entities.DBModels.NewsModels;

namespace Dashboard.Areas.NewsEntity.Controllers
{
    [Area("NewsEntity")]
    [Authorize(DashboardViewEnum.NewsAttachment, AccessLevelEnum.View)]
    public class NewsAttachmentController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public NewsAttachmentController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index(int fk_News, bool ProfileLayOut = false)
        {

            IQueryable<NewsAttachmentModel> data = _unitOfWork.News.GetNewsAttachments(new NewsAttachmentParameters { Fk_News = fk_News });

            List<NewsAttachmentDto> resultDto = _mapper.Map<List<NewsAttachmentDto>>(data);

            ViewData["ProfileLayOut"] = ProfileLayOut;
            ViewData["Fk_News"] = fk_News;

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(resultDto);
        }

        [Authorize(DashboardViewEnum.NewsAttachment, AccessLevelEnum.Create)]
        public async Task<IActionResult> Uploud(int fk_News)
        {
            IFormFile file = HttpContext.Request.Form.Files["file"];
            if (file != null)
            {
                NewsAttachment attachment = new()
                {
                    FileUrl = await _unitOfWork.News.UploudFile(_environment.WebRootPath, file),
                    StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString()),
                    Fk_News = fk_News,
                    FileLength = file.Length,
                    FileName = file.FileName,
                    FileType = file.ContentType,
                };
                _unitOfWork.News.CreateNewsAttachment(attachment);
                await _unitOfWork.Save();
            }
            return Ok();
        }

        [HttpPost]
        [Authorize(DashboardViewEnum.NewsAttachment, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitOfWork.News.DeleteNewsAttachment(id);
            await _unitOfWork.Save();

            return Ok();
        }
    }
}

namespace API.Controllers
{
    [Route("apple-app-site-association")]
    public class AppleController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AppSettings _appSettings;

        public AppleController(IWebHostEnvironment environment,
                               IOptions<AppSettings> appSettings)
        {
            _hostingEnvironment = environment;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Content(
                await FileUtil.ReadAllTextAsync(Path.Combine(_hostingEnvironment.WebRootPath, "apple-app-site-association-" + _appSettings.TenantEnvironment)),
                "application/pkcs7-mime"
            );
        }
    }

    public static class FileUtil
    {
        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            StringBuilder stringBuilder = new();
            using FileStream fileStream = File.OpenRead(filePath);
            using StreamReader streamReader = new(fileStream);
            string line = await streamReader.ReadLineAsync();
            while (line != null)
            {
                _ = stringBuilder.AppendLine(line);
                line = await streamReader.ReadLineAsync();
            }
            return stringBuilder.ToString();
        }
    }
}

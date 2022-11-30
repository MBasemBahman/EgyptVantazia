using Microsoft.AspNetCore.Mvc;
using Site.Models;
using System.Diagnostics;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [Route("Download")]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
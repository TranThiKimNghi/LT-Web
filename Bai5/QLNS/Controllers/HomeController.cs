using Microsoft.AspNetCore.Mvc;
using QLNS.Models;
using System.Diagnostics;

namespace QLNS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string data = await GetDataAsync();
            ViewBag.Message = data;
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<string> GetDataAsync()
        {
            await Task.Delay(5000);
            return "Xin chaof cac ban nhe";
        }




    }
}

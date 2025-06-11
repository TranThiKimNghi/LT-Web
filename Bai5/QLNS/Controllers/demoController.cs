using Microsoft.AspNetCore.Mvc;

namespace QLNS.Controllers
{
    public class demoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

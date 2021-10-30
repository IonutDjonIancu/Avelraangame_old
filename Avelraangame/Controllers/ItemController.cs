using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Merchant()
        {
            return View();
        }
    }
}

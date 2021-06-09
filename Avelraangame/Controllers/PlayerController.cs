using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers
{
    public class PlayerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

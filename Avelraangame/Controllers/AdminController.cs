using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin_index()
        {
            return View();
        }
    }
}

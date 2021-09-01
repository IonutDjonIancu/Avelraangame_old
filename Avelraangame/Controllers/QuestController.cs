using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers
{
    public class QuestController : Controller
    {
        public IActionResult Quest_Index()
        {
            return View();
        }
    }
}

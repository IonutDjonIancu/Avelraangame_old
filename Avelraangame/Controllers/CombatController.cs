using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers
{
    public class CombatController : Controller
    {
        public IActionResult Combat_index()
        {
            return View();
        }
    }
}

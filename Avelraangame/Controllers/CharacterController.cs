using System.Collections.Generic;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Avelraangame.Controllers
{
    public class CharacterController : Controller
    {
        public IActionResult Character_index()
        {
            var playersService = new PlayersService();

            var list = JsonConvert.DeserializeObject<List<string>>(playersService.GetAllPlayers());

            ViewData.Add("listOfPlayers", list);
            return View();
        }

        public IActionResult Character_roll()
        {
            return View();
        }

        public IActionResult Character_select()
        {
            return View();
        }

        public IActionResult Character_levelup()
        {
            return View();
        }

        public IActionResult Character_model()
        {
           return View();
        }

       
    }
}

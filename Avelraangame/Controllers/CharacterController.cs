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
            return View();
        }

        public IActionResult Character_roll()
        {
            var playersService = new PlayersService();

            var list = JsonConvert.DeserializeObject<List<string>>(playersService.GetAllPlayers());
            
            ViewData.Add("listOfPlayers", list);
            return View();
        }

        public IActionResult Character_select()
        {
            var playersService = new PlayersService();

            var list = JsonConvert.DeserializeObject<List<string>>(playersService.GetAllPlayers());

            ViewData.Add("listOfPlayers", list);
            return View();
        }

        public IActionResult Character_levelup([FromQuery] string request)
        {
            CharacterVm charVm;

            if (request == null)
            {
                return RedirectToAction("Character_select", "Character");
            }

            try
            {
                charVm = JsonConvert.DeserializeObject<CharacterVm>(request);
            }
            catch (System.Exception)
            {
                return RedirectToAction("Character_select", "Character");
            }


            return View();
        }

        public IActionResult Character_model([FromQuery]string request)
        {
            CharacterVm charVm;

            if (request == null)
            {
                return RedirectToAction("Character_select", "Character");
            }

            try
            {
                charVm = JsonConvert.DeserializeObject<CharacterVm>(request);
            }
            catch (System.Exception)
            {
                return RedirectToAction("Character_select", "Character");
            }


            return View();
        }

       
    }
}

﻿using System;
using System.Collections.Generic;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Avelraangame.Controllers
{
    public class CharacterController : Controller
    {
        public IActionResult Index()
        {
            var playersService = new PlayersService();

            var list = JsonConvert.DeserializeObject<List<string>>(playersService.GetAllPlayers());
            
            ViewData.Add("listOfPlayers", list);
            return View();
        }

        public IActionResult CreateCharacter([FromQuery]string characterId)
        {
            if (string.IsNullOrWhiteSpace(characterId))
            {
                return RedirectToAction("Index", "Character");
            }

            return View();
        }

       
    }
}
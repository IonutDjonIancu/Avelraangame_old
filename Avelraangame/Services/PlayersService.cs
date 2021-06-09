using Avelraangame.Data;
using Avelraangame.Models;
using Avelraangame.Models.ApiModels;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Avelraangame.Services.Validations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Avelraangame.Services
{
    public class PlayersService : PlayersSubService
    {
        public PlayerValidations PlayerValidations { get; set; }
        public DataService DataService { get; set; }

        public PlayersService()
        {
            DataService = new DataService();
            PlayerValidations = new PlayerValidations();
        }

        public bool CreatePlayer(RequestModel request)
        {
            var player = JsonConvert.DeserializeObject<Player>(request.RequestPayload);






            //Context.Players.Add(player); <--------------------------- must be moved in DataService
            //Context.SaveChanges();

            return true;
        }
    }
}

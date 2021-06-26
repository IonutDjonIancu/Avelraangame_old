using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using System;
using Avelraangame.Models;
using Newtonsoft.Json;

namespace Avelraangame.Services
{
    public class PlayersService : PlayersSubService
    {
        public bool IsPlayerValid(Guid playerid)
        {
            ValidatePlayerById(playerid);

            return true;
        }

        public string CreatePlayer(RequestVm request)
        {
            PlayerVm playerVm;

            playerVm = ValidateRequestDeserialization_PlayerVm(request.Message);
            ValidatePlayerDetails(playerVm);
            ValidatePlayerUnicity(playerVm.Name);
            ValidateNumberOfPlayers();

            var player = new Player()
            {
                Id = Guid.NewGuid(),
                Name = playerVm.Name,
                Ward = playerVm.Ward,
                LastLogin = DateTime.Now
            };

            DataService.SavePlayer(player);

            return Scribe.ShortMessages.Success.ToString();
        }

        public Player GetPlayerByName(string name)
        {
            return DataService.GetPlayerByName(name);
        }

        public Player GetPlayerById(Guid id)
        {
            return DataService.GetPlayerById(id);
        }

        public string GetAllPlayers()
        {
            return JsonConvert.SerializeObject(DataService.GetPlayersNames());
        }
    }
}

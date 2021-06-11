using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using Avelraangame.Models;

namespace Avelraangame.Services
{
    public class PlayersService : PlayersSubService
    {
        public string CreatePlayer(RequestVm request)
        {
            PlayerVm playerVm;

            try
            {
                playerVm = ValidateRequest(request.Message);
                ValidatePlayerDetails(playerVm);
                ValidatePlayerUnicity(playerVm.Name);
                ValidateNumberOfPlayers();
            }
            catch (Exception ex)
            {
                return string.Concat(Scribe.ShortMessages.Failure, ": ", ex.Message);
            }


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
    }
}

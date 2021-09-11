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
        #region Business logic
        public string CreatePlayer(RequestVm request)
        {
            PlayerVm playervm;

            playervm = ValidateRequestDeserializationIntoPlayerVm(request.Message);
            ValidateNumberOfPlayers();
            ValidatePlayerDetailsOnCreate(playervm);
            ValidatePlayerUnicity(playervm.PlayerName);

            var player = new Player()
            {
                Id = Guid.NewGuid(),
                Name = playervm.PlayerName,
                Ward = playervm.Ward,
                CreationDate = DateTime.Now
            };

            DataService.SavePlayer(player);

            return Scribe.ShortMessages.Success.ToString();
        }

        public string Logon(RequestVm request)
        {
            PlayerVm playervm;

            playervm = ValidateRequestDeserializationIntoPlayerVm(request.Message);

            ValidatePlayerDetailsOnLogon(playervm);
            var player = ValidatePlayerBySymbolWard(playervm.Symbol, playervm.Ward);

            var result = new PlayerVm
            {
                PlayerId = player.Id,
                PlayerName = player.Name
            };

            return JsonConvert.SerializeObject(result);
        }
        #endregion

        #region Getters
        public string GetPlayerIdByName(RequestVm request)
        {
            var playervm = ValidateRequestDeserializationIntoPlayerVm(request.Message);
            ValidatePlayerName(playervm.PlayerName);
            var player = ValidatePlayerByName(playervm.PlayerName);
            ValidateWard(playervm.Ward, player.Ward);

            return player.Id.ToString();
        }

        public PlayerVm GetPlayerById(Guid playerId)
        {
            ValidatePlayerId(playerId);
            var playervm = new PlayerVm(DataService.GetPlayerById(playerId));

            return playervm;
        }

        public string GetAllPlayers()
        {
            return JsonConvert.SerializeObject(DataService.GetPlayersNames());
        }
        #endregion
    }
}

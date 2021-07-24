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
            PlayerVm playerVm;

            playerVm = ValidateRequestDeserialization(request.Message);
            ValidatePlayerDetails(playerVm);
            ValidatePlayerUnicity(playerVm.PlayerName);
            ValidateNumberOfPlayers();

            var player = new Player()
            {
                Id = Guid.NewGuid(),
                Name = playerVm.PlayerName,
                Ward = playerVm.Ward,
                LastLogin = DateTime.Now
            };

            DataService.SavePlayer(player);

            return Scribe.ShortMessages.Success.ToString();
        }
        #endregion

        #region Validators
        public bool IsPlayerValid(Guid playerid)
        {
            try
            {
                ValidatePlayerById(playerid);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void ValidatePlayerByIdNamePair(Guid playerId, string playerName)
        {
            ValidatePlayerIdNamePair(playerId, playerName);
        }

        public new void ValidatePlayerId(Guid playerId)
        {
            base.ValidatePlayerId(playerId);
        }

        public new void ValidatePlayerName(string playerName)
        {
            base.ValidatePlayerName(playerName);
        }
        #endregion

        #region Getters
        public string GetPlayerIdByName(RequestVm request)
        {
            var playerVm = ValidateRequestDeserialization(request.Message);
            ValidatePlayerName(playerVm.PlayerName);

            var playerId = DataService.GetPlayerByName(playerVm.PlayerName).Id;

            return playerId.ToString();
        }

        public Player GetPlayerById(Guid id)
        {
            return DataService.GetPlayerById(id);
        }

        public string GetAllPlayers()
        {
            return JsonConvert.SerializeObject(DataService.GetPlayersNames());
        }
        #endregion
    }
}

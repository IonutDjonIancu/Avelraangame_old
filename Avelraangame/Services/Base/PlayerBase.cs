using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Avelraangame.Services.Base
{
    public class PlayerBase
    {
        protected DataService DataService { get; set; }

        protected PlayerBase()
        {
            DataService = new DataService();
        }

        protected void ValidatePlayerByName(string playerName)
        {
            ValidatePlayerName(playerName);

            var playerExists = DataService.PlayerExists(playerName);

            if (!playerExists)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, $": player: {playerName}"));
            }
        }

        protected void ValidatePlayerIdNamePair(Guid playerId, string playerName)
        {
            ValidatePlayerName(playerName);
            ValidatePlayerId(playerId);
            ValidatePlayerById(playerId);

            var player = DataService.GetPlayerById(playerId);

            if (!player.Id.Equals(playerId))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player name does not match its id."));
            }
        }

        protected void ValidatePlayerId(Guid playerId)
        {
            if (playerId.Equals(Guid.Empty) ||
                playerId.Equals(null))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player id is missing or invalid."));
            }
        }

        protected void ValidatePlayerName(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player name is missing or invalid."));
            }
        }

        protected PlayerVm ValidateRequestDeserialization(string request)
        {
            PlayerVm playerVm;

            try
            {
                playerVm = JsonConvert.DeserializeObject<PlayerVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, $": {ex.Message}"));
            }

            return playerVm;
        }

        protected void ValidatePlayerById(Guid playerId)
        {
            ValidatePlayerId(playerId);

            Player player;
            try
            {
                player = DataService.GetPlayerById(playerId);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, $": {ex.Message}."));
            }

            if (player.Equals(null))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": player not found."));
            }
        }

        protected void ValidatePlayerDetails(PlayerVm playerVm)
        {
            ValidatePlayerName(playerVm.PlayerName);
            ValidateWards(playerVm.Ward, playerVm.Wardcheck);
        }

        protected void ValidatePlayerUnicity(string name)
        {
            var playerExists = DataService.GetPlayersNames().Any(s => s.Contains(name));

            if (playerExists)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": player exists."));
            }
        }

        protected void ValidateNumberOfPlayers()
        {
            var numOfPlayers = DataService.GetPlayersCount();

            if (numOfPlayers >= 100)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": number of player accounts reached."));
            }
        }



        // privates

        private void ValidateWards(string ward, string wardCheck)
        {
            if (string.IsNullOrWhiteSpace(ward)) 
            { 
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": ward is missing or invalid.")); 
            }

            if (string.IsNullOrWhiteSpace(wardCheck)) 
            { 
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": wardcheck is missing or invalid.")); 
            }

            if (ward.Length > 4) 
            { 
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ward is too long, 4 characters maximum.")); 
            }

            if (!wardCheck.Equals(ward)) 
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": wards don't match.")); 
            }
        }

        






    }
}

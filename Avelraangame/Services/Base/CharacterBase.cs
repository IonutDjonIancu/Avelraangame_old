using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;

namespace Avelraangame.Services.Base
{
    public class CharacterBase
    {
        protected DataService DataService { get; set; }

        protected CharacterBase()
        {
            DataService = new DataService();
        }

        protected void ValidateCharacterRoll(int roll)
        {
            if (roll == 0)
            {
                throw new Exception(string.Concat(Scribe.ShortMessages.Failure, ": character roll was 0."));
            }
        }

        protected void ValidatePlayerById(Guid playerId)
        {
            var playerService = new PlayersService();

            var player = playerService.GetPlayerById(playerId);

            if (player == null)
            {
            }
        }

        protected void ValidateCharVm(CharacterVm charVm)
        {
            if (charVm.PlayerId == null ||
                charVm.PlayerId == Guid.Empty)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": player id is missing."));
            }

            if (string.IsNullOrWhiteSpace(charVm.PlayerName))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": player name is missing."));
            }
        }

        protected void ValidateCharDiceBeforeReturn(Guid playerId, int roll)
        {
            if (playerId == null || playerId == Guid.Empty)
            {
                throw new Exception(message: Scribe.ShortMessages.ResourceNotFound.ToString());
            }

            if (roll == 0)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character roll was 0"));
            }
        }

        protected CharacterVm ValidateRequestDeserialization(string request)
        {
            CharacterVm charVm;

            try
            {
                charVm = JsonConvert.DeserializeObject<CharacterVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return charVm;
        }

        protected Player ValidatePlayerByName(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": ", $"player: {playerName}"));
            }

            var player = DataService.GetPlayerByName(playerName);

            if (player == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": ", $"player: {playerName}"));
            }

            return player;
        }
    }
}

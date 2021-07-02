using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace Avelraangame.Services.Base
{
    public class CharacterBase
    {
        protected DataService DataService { get; set; }
        protected PlayersService Players { get; set; }
        protected ItemsService Items { get; set; }
        protected TempsService Temps { get; set; }

        protected CharacterBase()
        {
            DataService = new DataService();
            Players = new PlayersService();
            Items = new ItemsService();
            Temps = new TempsService();
        }

        protected bool IsCharacterValid(Guid playerId, Guid charId)
        {
            ValidateCharacterByIdAndReturn(playerId, charId);

            return true;
        }

        protected Character ValidateCharacterByIdAndReturn(Guid playerId, Guid charId)
        {
            if (playerId.Equals(Guid.Empty) || playerId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": playerId is invalid or missing."));
            }

            if (charId.Equals(Guid.Empty) || charId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": charId is invalid or missing."));
            }

            Character character;

            try
            {
                character = DataService.GetCharacterById(charId);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, $": {ex.Message}."));
            }

            return character;
        }

        protected string ValidateCharacterName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Scribe.Characters_GenericName;
            }

            return string.Concat(char.ToUpper(name[0]), name.Substring(1));
        }

        protected void ValidateCharacterRoll(int roll)
        {
            if (roll == 0)
            {
                throw new Exception(string.Concat(Scribe.ShortMessages.Failure, ": character roll was 0."));
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

        protected Character ValidatePlayerAndCharacterAndReturn(Guid playerId, Guid charId)
        {
            Players.IsPlayerValid(playerId);
            var chr = ValidateCharacterByIdAndReturn(playerId, charId);

            return chr;
        }

        protected void ValidateCharacterById(Guid playerId, Guid charId)
        {
            Players.ValidatePlayerId(playerId);
            ValidateCharacterId(charId);

            var chr = DataService.GetCharacterById(charId);

            if (chr.Equals(null))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": character not found."));
            }

            Assert.That(chr.PlayerId.Equals(playerId));

            if (!chr.PlayerId.Equals(playerId))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character not bound to requested playerId."));
            }
        }

        protected void ValidateCharacterId(Guid charId)
        {
            if (charId.Equals(Guid.Empty) || 
                charId.Equals(null))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": characterId is missing or invalid."));
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

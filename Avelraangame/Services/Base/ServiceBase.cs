using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.Base
{
    public class ServiceBase // move strings to Scribe
    {
        protected DataService DataService { get; set; }

        protected ServiceBase()
        {
            DataService = new DataService();
        }

        #region PlayerValidation
        protected PlayerVm ValidateRequestDeserializationIntoPlayerVm(string request)
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

        protected void ValidatePlayerId(Guid playerId)
        {
            if (playerId.Equals(Guid.Empty) || playerId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player id is missing or invalid."));
            }
        }

        protected Player ValidatePlayerById(Guid playerId)
        {
            ValidatePlayerId(playerId);

            var player = DataService.GetPlayerById(playerId);

            if (player == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": player not found."));
            }

            return player;
        }

        protected void ValidatePlayerName(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player name is missing or invalid."));
            }
        }

        protected Player ValidatePlayerByName(string playerName)
        {
            ValidatePlayerName(playerName);

            var player = DataService.GetPlayerByName(playerName);

            if (player == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": player not found."));
            }

            return player;
        }

        protected void ValidateWard(string ward, string wardCheck)
        {
            if (string.IsNullOrWhiteSpace(ward))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player ward is missing or invalid."));
            }

            if (ward.Equals(wardCheck)) { return; }

            throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": invalid ward."));
        }

        protected void ValidatePlayerByIdNamePair(Guid playerId, string playerName)
        {
            ValidatePlayerName(playerName);
            ValidatePlayerId(playerId);
            var player = ValidatePlayerById(playerId);

            if (player.Id.Equals(playerId)) { return; }
            
            throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player name does not match its id."));
        }

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

        protected void ValidatePlayerUnicity(string name)
        {
            var playerExists = DataService.GetPlayersNames().Any(s => s.Contains(name));

            if (playerExists)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": player already exists."));
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

        protected void ValidatePlayerDetailsOnCreate(PlayerVm playerVm)
        {
            ValidatePlayerName(playerVm.PlayerName);
            ValidateWards(playerVm.Ward, playerVm.Wardcheck);
        }

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

        #endregion

        #region CharacterValidation
        protected CharacterVm ValidateRequestDeserializationIntoCharacterVm(string request)
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

        protected void ValidateCharVm(CharacterVm charVm)
        {
            ValidateCharacterId(charVm.CharacterId);
            ValidatePlayerId(charVm.PlayerId);
        }

        protected void ValidateCharacterId(Guid charId)
        {
            if (charId.Equals(Guid.Empty) || charId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": character id is missing or is invalid."));
            }
        }

        protected Character ValidateCharacterById(Guid charId)
        {
            ValidateCharacterId(charId);

            var chr = DataService.GetCharacterById(charId);

            if (chr == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": character not found."));
            }

            return chr;
        }

        protected Character ValidateCharacterByPlayerId(Guid charId, Guid playerId)
        {
            var chr = ValidateCharacterById(charId);

            if (chr.PlayerId.Equals(playerId)) { return chr; }

            throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character does not match player id."));
        }

        protected bool IsCharacterValid(Guid playerId, Guid charId)
        {
            try
            {
                ValidateCharacterByPlayerId(playerId, charId);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        protected string ValidateCharacterName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Scribe.Characters_GenericName;
            }

            return string.Concat(char.ToUpper(name[0]), name.Substring(1));
        }

        protected void ValidateCharDiceBeforeReturn(Guid playerId, int roll)
        {
            ValidatePlayerId(playerId);

            if (roll == 0)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character roll was 0"));
            }
        }

        public CharacterVm ValidateRollDetailsBeforeStoring(RequestVm request)
        {
            var charvm = ValidateRequestDeserializationIntoCharacterVm(request.Message);
            ValidatePlayerByIdNamePair(charvm.PlayerId, charvm.PlayerName);

            return charvm;
        }

        #endregion

        #region ItemValidation



        #endregion






    }
}

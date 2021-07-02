using Avelraangame.Models;
using Avelraangame.Models.ModelScraps;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Avelraangame.Services
{
    public class CharactersService : CharacterSubService
    {
        #region incoming GET requests
        public Character GetCharacterById(Guid charId)
        {
            return DataService.GetCharacterById(charId);
        }

        public CharacterVm GetCharacter(RequestVm request)
        {
            var reqCharVm = ValidateRequestDeserialization(request.Message);
            ValidateCharacterById(reqCharVm.PlayerId.GetValueOrDefault(), reqCharVm.CharacterId);

            var chr = DataService.GetCharacterById(reqCharVm.CharacterId);
            var charVm = GetCalculatedCharacter(chr);

            return charVm;
        }

        public CharacterVm GetCharacterWithLevelUp(RequestVm request)
        {
            var requestCharVm = ValidateRequestDeserialization(request.Message);
            var chr = ValidatePlayerAndCharacterAndReturn(requestCharVm.PlayerId.GetValueOrDefault(), requestCharVm.CharacterId);

            var responseCharVm = new CharacterVm(chr);

            var tempBonuses = Temps.GetTempInfosByCharacterId(requestCharVm.CharacterId);
            if (tempBonuses.Count.Equals(0))
            {
                return requestCharVm;
            }

            var listOfTempBonuses = new List<TempsVm>();

            foreach (var bonus in tempBonuses)
            {
                var tembo = new TempsVm(bonus);

                listOfTempBonuses.Add(tembo);
            }

            responseCharVm.Bonuses = listOfTempBonuses;

            return responseCharVm;
        }


        /// <summary>
        /// Gets the list of characters by player name.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CharacterVm> GetCharacters(RequestVm request)
        {
            var charVm = ValidateRequestDeserialization(request.Message);

            var playerId = ValidatePlayerByName(charVm.PlayerName).Id;

            var list = DataService.GetCharactersByPlayerId(playerId);

            var returnList = new List<CharacterVm>();

            foreach (var item in list)
            {

                var charvm = new CharacterVm(item);

                returnList.Add(charvm);
            }

            return returnList;
        }
              
        public CharacterVm StoreRoll(CharacterVm charVm)
        {
            if (charVm.PlayerId == null || charVm.PlayerId.Equals(Guid.Empty))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": playerId is null."));
            }

            if (string.IsNullOrWhiteSpace(charVm.PlayerName))
            {
                var playersService = new PlayersService();
                charVm.PlayerName = playersService.GetPlayerById(charVm.PlayerId.GetValueOrDefault())?.Name;
            }

            ValidateCharacterRoll(charVm.Logbook.StatsRoll);

            return charVm;
        }

        public (string responseMessage, Guid playerId, int roll) CharacterRoll20(RequestVm request)
        {
            CharacterVm charVm;

            charVm = ValidateRequestDeserialization(request.Message);
                
            if (charVm.PlayerId == null || charVm.PlayerId.Equals(Guid.Empty))
            {
                var player = ValidatePlayerByName(charVm.PlayerName);
                charVm.PlayerId = player.Id;
            }

            charVm.Logbook.StatsRoll = Dice.Roll_d_20();

            ValidateCharDiceBeforeReturn(charVm.PlayerId.GetValueOrDefault(), charVm.Logbook.StatsRoll);

            return (JsonConvert.SerializeObject(charVm), charVm.PlayerId.GetValueOrDefault(), charVm.Logbook.StatsRoll);
        }
        #endregion

        #region incoming POST requests

        public string SaveCharacterWithLevelUp(RequestVm request)
        {
            var charVm = ValidateRequestDeserialization(request.Message);
            ValidateCharacterById(charVm.PlayerId.GetValueOrDefault(), charVm.CharacterId);

            // first get the unmodified character from db
            var chr = GetCharacterById(charVm.CharacterId);
            // get the temp data associated with the char
            var temps = Temps.GetTempInfosByCharacterId(charVm.CharacterId);
            // compare and throw if necessary
            CompareTempsWithRequest(charVm, chr, temps);

            // set new values
            chr.Stats = JsonConvert.SerializeObject(charVm.Stats);
            chr.Skills = JsonConvert.SerializeObject(charVm.Skills);
            // remove levelup marker
            chr.HasLevelup = false;

            try
            {
                DataService.UpdateCharacter(chr);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, $": {ex.Message}."));
            }

            Temps.RemoveTempsInfo(temps);

            return string.Concat(Scribe.ShortMessages.Success);
        }


        /// <summary>
        /// Adds stats roll and playerId to the newly created character, returns characterId.
        /// </summary>
        /// <param name="roll"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Guid CreateCharacter(CharacterVm charVm) // must refactor to POST method
        {
            ValidateCharVm(charVm);

            var chr = CreateHumanCharacter(charVm);

            DataService.SaveCharacter(chr);

            Temps.SaveTempCharInfo(chr.Id, TempUtils.BonusTo.Stats, charVm.Logbook.StatsRoll + (10 * GetEntityLevelByRoll(charVm.Logbook.StatsRoll)));
            Temps.SaveTempCharInfo(chr.Id, TempUtils.BonusTo.Skills, 20);

            return chr.Id;
        }

        #endregion

    }
}

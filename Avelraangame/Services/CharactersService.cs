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
        #region Business logic

        /// <summary>
        /// Adds stats roll and playerId to the newly created character, returns characterId.
        /// </summary>
        /// <param name="roll"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public string CreateCharacter(CharacterVm charVm) // must refactor to POST method
        {
            ValidateCharVm(charVm);

            var chr = CreateHumanCharacter(charVm);

            DataService.SaveCharacter(chr);

            Temps.SaveTempCharInfo(chr.Id, TempUtils.BonusTo.Stats, charVm.Logbook.StatsRoll + (10 * GetEntityLevelByRoll(charVm.Logbook.StatsRoll)));
            Temps.SaveTempCharInfo(chr.Id, TempUtils.BonusTo.Skills, 20);

            return string.Concat(Scribe.ShortMessages.Success);
        }

        public string SaveCharacterWithLevelUp(RequestVm request)
        {
            var charVm = ValidateRequestDeserialization(request.Message);
            ValidateCharacterById(charVm.PlayerId, charVm.CharacterId);

            // first get the unmodified character from db
            var chr = DataService.GetCharacterById(charVm.CharacterId);
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

        public (string responseMessage, string keyPlayerId, int roll) CharacterCreationRoll20(RequestVm request)
        {
            var charVm = ValidateRequestDeserialization(request.Message);
            Players.ValidatePlayerByIdNamePair(charVm.PlayerId, charVm.PlayerName);

            charVm.Logbook.StatsRoll = Dice.Roll_d_20();

            ValidateCharDiceBeforeReturn(charVm.PlayerId, charVm.Logbook.StatsRoll);

            return (JsonConvert.SerializeObject(charVm), charVm.PlayerId.ToString(), charVm.Logbook.StatsRoll);
        }

        public CharacterVm StoreRollValidation(RequestVm request)
        {
            var charVm = ValidateRequestDeserialization(request.Message);
            Players.ValidatePlayerByIdNamePair(charVm.PlayerId, charVm.PlayerName);

            return charVm;
        }
        #endregion

        #region Getters
        public string GetCharacter(RequestVm request)
        {
            var reqCharVm = ValidateRequestDeserialization(request.Message);
            ValidateCharacterByPlayerId(reqCharVm.CharacterId, reqCharVm.PlayerId);

            var charVm = GetCalculatedCharacter(reqCharVm.CharacterId);

            return JsonConvert.SerializeObject(charVm);
        }

        public string GetCharacterWithLevelUp(RequestVm request)
        {
            var reqCharVm = ValidateRequestDeserialization(request.Message);
            ValidateCharacterByPlayerId(reqCharVm.CharacterId, reqCharVm.PlayerId);

            var charVm = new CharacterVm(DataService.GetCharacterById(reqCharVm.CharacterId));

            var tempBonuses = Temps.GetTempInfosByCharacterId(reqCharVm.CharacterId);
            if (tempBonuses.Count.Equals(0))
            {
                return JsonConvert.SerializeObject(reqCharVm);
            }

            var listOfTempBonuses = new List<TempsVm>();

            foreach (var bonus in tempBonuses)
            {
                listOfTempBonuses.Add(new TempsVm(bonus));
            }

            charVm.Bonuses = listOfTempBonuses;

            return JsonConvert.SerializeObject(charVm);
        }


        /// <summary>
        /// Gets the list of characters by a combination of player id and player name.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetCharactersByPlayer(RequestVm request)
        {
            var charVm = ValidateRequestDeserialization(request.Message);
            Players.ValidatePlayerByIdNamePair(charVm.PlayerId, charVm.PlayerName);

            var list = DataService.GetCharactersByPlayerId(charVm.PlayerId);

            var returnList = new List<CharacterVm>();

            foreach (var item in list)
            {
                returnList.Add(new CharacterVm(item));
            }

            return JsonConvert.SerializeObject(returnList);
        }

        #endregion

        #region Validators
        #endregion

    }
}

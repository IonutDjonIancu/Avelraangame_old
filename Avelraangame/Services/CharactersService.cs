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
        public Character GetCharacterById(Guid charId)
        {
            return DataService.GetCharacterById(charId);
        }

        public CharacterVm GetCharacterLevelUp(RequestVm request)
        {
            var charVm = ValidateRequestDeserialization(request.Message);
            ValidatePlayerAndCharacter(charVm.PlayerId.GetValueOrDefault(), charVm.CharacterId);


            var tempBonuses = Temps.GetTempInfosByCharacterId(charVm.CharacterId);
            if (tempBonuses.Count.Equals(0))
            {
                return charVm;
            }

            var listOfTempBonuses = new List<TempsVm>();

            foreach (var bonus in tempBonuses)
            {
                var tembo = new TempsVm(bonus);

                listOfTempBonuses.Add(tembo);
            }

            charVm.Bonuses = listOfTempBonuses;

            return charVm;
        }



        public CharacterCalculatedVm GetCalculatedCharacter(RequestVm request)
        {
            var reqCharVm = ValidateRequestDeserialization(request.Message);
            Players.IsPlayerValid(reqCharVm.PlayerId.GetValueOrDefault());
            IsCharacterValid(reqCharVm.PlayerId.GetValueOrDefault(), reqCharVm.CharacterId);

            var chr = GetCharacterById(reqCharVm.CharacterId);

            if (chr == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": character not found."));
            }

            var tempBonuses = Temps.GetTempInfosByCharacterId(chr.Id);
            var retTempBonusesList = new List<TempsVm>();

            foreach (var item in tempBonuses)
            {
                var tembo = new TempsVm(item);

                retTempBonusesList.Add(tembo);
            }

            var charVm = new CharacterVm(chr)
            {
                Bonuses = retTempBonusesList
            };

            var result = new CharacterCalculatedVm(charVm);

            return result;
        }

        public bool IsCharacterValid(Guid playerId, Guid charId)
        {
            ValidateCharacterById(playerId, charId);

            return true;
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

        /// <summary>
        /// Gets the list of draft characters by player name
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //public List<CharacterVm> GetCharactersDraft(RequestVm request)
        //{
        //    var charVm = ValidateRequestDeserialization(request.Message);

        //    var playerId = ValidatePlayerByName(charVm.PlayerName).Id;

        //    var dbCharacters = DataService.GetCharactersDraftByPlayerId(playerId);

        //    var returnList = new List<CharacterVm>();

        //    foreach (var chr in dbCharacters)
        //    {
        //        var charvm = new CharacterVm
        //        {
        //            CharacterId = chr.Id,
        //            PlayerId = chr.PlayerId,

        //            Name = chr.Name,
        //            Race = ((CharactersUtils.Races)chr.Race).ToString(),
        //            Culture = ((CharactersUtils.Cultures)chr.Culture).ToString(),

        //            EntityLevel = chr.EntityLevel,

        //            Logbook = JsonConvert.DeserializeObject<Logbook>(chr.Logbook),
        //            Supplies = Items.GetSuppliesItemsByCharacterId(chr.Id)
        //        };

        //        returnList.Add(charvm);
        //    }

        //    return returnList;
        //}


        public CharacterVm CreateCharacter_storeRoll(CharacterVm charVm)
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

        public (string responseMessage, Guid playerId, int roll) CreateCharacter_roll20(RequestVm request)
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

        /// <summary>
        /// Adds stats roll and playerId to the newly created character, returns characterId.
        /// </summary>
        /// <param name="roll"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Guid CreateCharacter_step1(CharacterVm charVm)
        {
            ValidateCharVm(charVm);

            var chr = CreateHumanCharacter(charVm);

            DataService.SaveCharacter(chr);

            Temps.SaveTempCharInfo(chr.Id, TempUtils.BonusTo.Stats, charVm.Logbook.StatsRoll + (10 * chr.EntityLevel));
            Temps.SaveTempCharInfo(chr.Id, TempUtils.BonusTo.Skills, 20);


            return chr.Id;
        }

    }
}

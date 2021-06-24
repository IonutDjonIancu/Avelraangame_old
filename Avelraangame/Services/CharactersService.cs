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

                var charvm = new CharacterVm
                {
                    CharacterId = item.Id,
                    PlayerId = item?.PlayerId,
                    PlayerName = item.Player.Name
                };

                returnList.Add(charvm);
            }

            return returnList;
        }

        /// <summary>
        /// Gets the list of draft characters by player name
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CharacterVm> GetCharactersDraft(RequestVm request)
        {
            var charVm = ValidateRequestDeserialization(request.Message);

            var playerId = ValidatePlayerByName(charVm.PlayerName).Id;

            var dbCharacters = DataService.GetCharactersDraftByPlayerId(playerId);

            var returnList = new List<CharacterVm>();

            foreach (var chr in dbCharacters)
            {
                var charvm = new CharacterVm
                {
                    CharacterId = chr.Id,
                    PlayerId = chr.PlayerId,

                    Name = chr.Name,
                    Race = ((CharactersUtils.Races)chr.Race).ToString(),
                    Culture = ((CharactersUtils.Cultures)chr.Culture).ToString(),

                    EntityLevel = chr.EntityLevel,

                    Logbook = JsonConvert.DeserializeObject<Logbook>(chr.Logbook)
                };

                returnList.Add(charvm);
            }

            return returnList;
        }


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

            return chr.Id;
        }

    }
}

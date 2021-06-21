using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;

namespace Avelraangame.Services
{
    public class CharactersService : CharacterSubService
    {
        //public CharacterVm GetCharacterById(Guid charId)
        //{
        //    var chr = DataService.GetCharacterById(charId);
        //    var proto = 

        //    var charVm = new CharacterVm
        //    {
        //        CharacterId = chr.Id,
        //        PlayerId = cchr.PlayerId,
                
        //        Stats = chr.StatsBase,


        //    };



        //    return charVm;
        //}

        public CharacterVm CreateCharacter_storeRoll(CharacterVm charVm)
        {
            if (charVm.PlayerId == null || charVm.PlayerId.Equals(Guid.Empty))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": playerId is null."));
            }
            else
            {
                ValidatePlayerById(charVm.PlayerId);
            }

            if (string.IsNullOrWhiteSpace(charVm.PlayerName))
            {
                var playersService = new PlayersService();
                charVm.PlayerName = playersService.GetPlayerById(charVm.PlayerId).Name;
            }

            ValidateCharacterRoll(charVm.StatsRoll);

            return charVm;
        }

        public (string responseMessage, string playerId, string roll) CreateCharacter_roll20(RequestVm request)
        {
            CharacterVm charVm;

            charVm = ValidateRequestDeserialization(request.Message);
                
            if (charVm.PlayerId == null || charVm.PlayerId.Equals(Guid.Empty))
            {
                var player = ValidatePlayerByName(charVm.PlayerName);
                charVm.PlayerId = player.Id;
            }

            charVm.StatsRoll = Dice.Roll_d_20();

            ValidateCharDiceBeforeReturn(charVm.PlayerId.ToString(), charVm.StatsRoll);

            return (JsonConvert.SerializeObject(charVm), charVm.PlayerId.ToString(), charVm.StatsRoll.ToString());
        }

        /// <summary>
        /// Adds stats roll and playerId to the newly created character, returns characterId.
        /// </summary>
        /// <param name="roll"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        //public Guid CreateCharacter_step1(CharacterVm charVm)
        //{
        //    ValidateCharVm(charVm);

        //    var chr = new Character()
        //    {
        //        Id = Guid.NewGuid(),
        //        PlayerId = charVm.PlayerId,

        //        StatsBase = JsonConvert.SerializeObject(GenerateStatsBase()),
        //        ExpertiseBase = JsonConvert.SerializeObject(GenerateExpertiseBase()),
        //        AssetsBase = JsonConvert.SerializeObject(GenerateAssetsBase()),
        //        Logbook = JsonConvert.SerializeObject(GenerateLogbookBase(charVm)),
        //    };

        //    DataService.SaveCharacter(chr);

        //    return chr.Id;
        //}

    }
}

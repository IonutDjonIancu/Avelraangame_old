using Avelraangame.Models;
using Avelraangame.Models.ModelScraps;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services
{
    public class CombatService : CombatSubService
    {
        #region Business logic
        public string GoToParty(RequestVm request)
        {
            var charVm = ValidateRequestDeserializationIntoCharacterVm(request.Message);
            var charService = new CharactersService();

            charVm = charService.ValidateCharacter(charVm.CharacterId, charVm.PlayerId);

            var character = DataService.GetCharacterById(charVm.CharacterId);
            character.InParty = true;

            var party = new Party()
            {
                Id = Guid.NewGuid()
            };

            character.PartyId = party.Id;

            try
            {
                DataService.CreateParty(party);
                DataService.UpdateCharacter(character);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, $": {ex.Message}"));
            }

            return JsonConvert.SerializeObject(string.Concat(Scribe.ShortMessages.Success));
        }

        #endregion

        #region Getters
        public string GetWeakNpcFight(RequestVm request)
        {
            var characterService = new CharactersService();
            var fightId = Guid.NewGuid();

            var charVm = ValidateRequestDeserializationIntoCharacterVm(request.Message);
            var character = ValidateCharacterById(charVm.CharacterId);

            if (character.InFight.GetValueOrDefault())
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character already in combat."));
            }

            character.InFight = true;
            character.FightId = fightId;
            DataService.UpdateCharacter(character);

            charVm = characterService.GetCalculatedCharacterById(character.Id);

            var combat = new Combat
            {
                FightId = fightId,
                GoodGuys = new List<CharacterVm>
                {
                    charVm
                },
                BadGuys = new List<CharacterVm>
                {
                    characterService.GenerateWeakNpc()
                },
                FightDate = DateTime.Now.Date.ToShortDateString()
            };

            var snapshot = new Storage
            {
                Id = fightId,
                Value = JsonConvert.SerializeObject(combat)
            };
            DataService.CreateStorage(snapshot);

            return string.Concat(Scribe.ShortMessages.Success, ": weak npc fight generated!");
        }

        public string GetFightByCharacter(RequestVm request)
        {
            var charVm = ValidateRequestDeserializationIntoCharacterVm(request.Message);
            var character = ValidateCharacterByPlayerId(charVm.CharacterId, charVm.PlayerId);

            var storage = new StorageService();

            if (character.InFight.GetValueOrDefault())
            {
                return storage.GetStorageValueById(character.FightId.GetValueOrDefault());
            }

            return string.Concat(Scribe.ShortMessages.ResourceNotFound, ": character is not fighting.");
        }
        #endregion


    }
}

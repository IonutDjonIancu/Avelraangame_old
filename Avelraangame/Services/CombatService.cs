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
        public string Attack(RequestVm request)
        {
            var attack = ValidateRequestDeserializationIntoAttack(request.Message);
            ValidateAttackDetails(attack);
            
            var store = new StorageService();
            var storeValue = store.GetStorageValueById(attack.FightId);
            var combat = JsonConvert.DeserializeObject<Combat>(storeValue);

            var attacker = combat.GoodGuys.Where(s => s.CharacterId.Equals(attack.Attacker)).FirstOrDefault();
            if (attacker == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": attacker incompatible with fight."));
            }
            var defender = combat.BadGuys.Where(s => s.CharacterId.Equals(attack.Defender)).FirstOrDefault();
            if (defender == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": defender incompatible with fight."));
            }

            var (att, def) = RollAttack(attacker, defender);

            attacker = att;
            defender = def;

            var combatString = JsonConvert.SerializeObject(combat);

            var snapshot = new Storage
            {
                Id = attack.FightId,
                Value = combatString
            };
            DataService.UpdateStorage(snapshot);

            return JsonConvert.SerializeObject(combat);
        }


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

using Avelraangame.Models;
using Avelraangame.Models.POCOs;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Avelraangame.Services.ServiceUtils.CombatUtils;

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
            var fight = JsonConvert.DeserializeObject<Fight>(storeValue);

            var attacker = fight.GoodGuys.Where(s => s.CharacterId.Equals(attack.Attacker)).FirstOrDefault();
            if (attacker.AttackToken.Equals(false))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": attacker has no token."));
            }
            if (attacker.IsAlive.Equals(false))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": attacker is dead."));
            }

            var defender = fight.BadGuys.Where(s => s.CharacterId.Equals(attack.Defender)).FirstOrDefault();
            if (defender.IsAlive.Equals(false))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": defender is dead already."));
            }

            var (att, def, dmg) = RollAttack(attacker, defender);

            attacker = att;
            defender = def;

            if (dmg > 0)
            {
                fight.LastActionResult = string.Concat(Scribe.ShortMessages.Success, $": {dmg} dmg done.");
            }
            else
            {
                fight.LastActionResult = string.Concat(Scribe.ShortMessages.Failure, $": miss");
            }

            fight.LastActionResult = EndFight(fight);

            var storeVal = JsonConvert.SerializeObject(fight);
            var snapshot = new Storage
            {
                Id = attack.FightId,
                Value = storeVal
            };

            DataService.UpdateStorage(snapshot);

            return JsonConvert.SerializeObject(fight);
        }

        public string Defend(RequestVm request)
        {
            var defend = ValidateRequestDeserializationIntoDefend(request.Message);

            var characterService = new CharactersService();
            characterService.ValidateCharacter(defend.MainCharacterId, defend.PlayerId);

            var store = new StorageService();
            var storeValue = store.GetStorageValueById(defend.FightId);
            var fight = JsonConvert.DeserializeObject<Fight>(storeValue);

            var isMainCharInFight = fight.GoodGuys.Where(s => s.CharacterId.Equals(defend.MainCharacterId)).Any();
            if (!isMainCharInFight)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character incompatible with fight."));
            }

            var allEnemiesAreDead = !fight.BadGuys.Where(s => s.IsAlive).Any();
            if (allEnemiesAreDead)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": all enemies are defeated."));
            }

            fight = RollNpcAttack(fight);
            fight = ReimburseTokens(fight);

            if (fight.TacticalSituation.Equals(TacticalSituation.Major_tactical_disadvantage) ||
                fight.TacticalSituation.Equals(TacticalSituation.Slight_tactical_disadvantage))
            {
                fight = RollNpcAttack(fight);
            }

            fight.LastActionResult = EndFight(fight);

            var storeVal = JsonConvert.SerializeObject(fight);
            var snapshot = new Storage
            {
                Id = fight.FightId,
                Value = storeVal
            };
            DataService.UpdateStorage(snapshot);

            return JsonConvert.SerializeObject(fight);
        }

        public string EndCombat(RequestVm request)
        {
            var combatEnd = ValidateRequestDeserializationIntoEndOfCombat(request.Message);
            var stored = DataService.GetStorage(combatEnd.FightId);
            var fight = JsonConvert.DeserializeObject<Fight>(stored.Value);


            if (fight.BadGuys.Where(s => s.IsAlive).Any() && fight.GoodGuys.Where(s => s.IsAlive).Any())
            {
                throw new Exception(string.Concat(Scribe.ShortMessages.Failure, ": there are still enemies about."));
            }

            DataService.DeleteStorage(stored);

            return string.Concat(Scribe.ShortMessages.Success, ": combat ended.");
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

        public string GenerateWeakNpcFight(RequestVm request)
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

            var fight = new Fight
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
                FightDate = DateTime.Now.Date.ToShortDateString(),
            };

            fight.TacticalSituation = DecideTacticalSituation(fight.GoodGuys, fight.BadGuys);
            
            if (fight.TacticalSituation.Equals(TacticalSituation.Major_tactical_disadvantage) ||
                fight.TacticalSituation.Equals(TacticalSituation.Slight_tactical_disadvantage))
            {
                fight = RollNpcAttack(fight);
            }
            fight.LastActionResult = string.Concat(fight.TacticalSituation);
            
            var snapshot = new Storage
            {
                Id = fightId,
                Value = JsonConvert.SerializeObject(fight)
            };
            DataService.CreateStorage(snapshot);

            return string.Concat(Scribe.ShortMessages.Success, $": {fight.LastActionResult}.");
        }
        #endregion

        #region Getters

        public string GetFight(RequestVm request)
        {
            var charVm = ValidateRequestDeserializationIntoCharacterVm(request.Message);
            var character = ValidateCharacterByPlayerId(charVm.CharacterId, charVm.PlayerId);
            var storage = new StorageService();

            if (!character.InFight.GetValueOrDefault())
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": character is not fighting."));
            }

            return storage.GetStorageValueById(character.FightId.GetValueOrDefault());
        }
        #endregion


    }
}

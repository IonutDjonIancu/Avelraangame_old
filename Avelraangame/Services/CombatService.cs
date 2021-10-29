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
        //public string Attack(RequestVm request)
        //{
        //    var attack = ValidateRequestDeserializationIntoAttack(request.Message);
        //    ValidateAttackDetails(attack);
            
        //    var store = new StorageService();
        //    var storeValue = store.GetStorageValueById(attack.FightId);
        //    var fight = JsonConvert.DeserializeObject<FightVmOld>(storeValue);

        //    var attacker = fight.GoodGuys.Where(s => s.CharacterId.Equals(attack.Attacker)).FirstOrDefault();
        //    if (attacker.AttackToken.Equals(false))
        //    {
        //        throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": attacker has no token."));
        //    }
        //    if (attacker.IsAlive.Equals(false))
        //    {
        //        throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": attacker is dead."));
        //    }

        //    var defender = fight.BadGuys.Where(s => s.CharacterId.Equals(attack.Defender)).FirstOrDefault();
        //    if (defender.IsAlive.Equals(false))
        //    {
        //        throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": defender is dead already."));
        //    }

        //    var (att, def, dmg) = RollAttack(attacker, defender);

        //    attacker = att;
        //    defender = def;

        //    if (dmg > 0)
        //    {
        //        fight.LastActionResult = string.Concat(Scribe.ShortMessages.Success, $": {dmg} dmg done.");
        //    }
        //    else
        //    {
        //        fight.LastActionResult = string.Concat(Scribe.ShortMessages.Failure, $": miss");
        //    }

        //    fight.LastActionResult = EndFight(fight);

        //    var storeVal = JsonConvert.SerializeObject(fight);
        //    var snapshot = new Storage
        //    {
        //        Id = attack.FightId,
        //        Value = storeVal
        //    };

        //    DataService.UpdateStorage(snapshot);

        //    return JsonConvert.SerializeObject(fight);
        //}

        //public string Defend(RequestVm request)
        //{
        //    var defend = ValidateRequestDeserializationIntoDefend(request.Message);

        //    var characterService = new CharactersService();
        //    characterService.ValidateCharacter(defend.MainCharacterId, defend.PlayerId);

        //    var store = new StorageService();
        //    var storeValue = store.GetStorageValueById(defend.FightId);
        //    var fight = JsonConvert.DeserializeObject<FightVm>(storeValue);

        //    var isMainCharInFight = fight.GoodGuys.Where(s => s.CharacterId.Equals(defend.MainCharacterId)).Any();
        //    if (!isMainCharInFight)
        //    {
        //        throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character incompatible with fight."));
        //    }

        //    var allEnemiesAreDead = !fight.BadGuys.Where(s => s.IsAlive).Any();
        //    if (allEnemiesAreDead)
        //    {
        //        throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": all enemies are defeated."));
        //    }

        //    fight = RollNpcAttack(fight);
        //    fight = ReimburseTokens(fight);

        //    if (fight.TacticalSituation.Equals(TacticalSituation.Major_tactical_disadvantage) ||
        //        fight.TacticalSituation.Equals(TacticalSituation.Slight_tactical_disadvantage))
        //    {
        //        fight = RollNpcAttack(fight);
        //    }

        //    fight.LastActionResult = EndFight(fight);

        //    var storeVal = JsonConvert.SerializeObject(fight);
        //    var snapshot = new Storage
        //    {
        //        Id = fight.FightId,
        //        Value = storeVal
        //    };
        //    DataService.UpdateStorage(snapshot);

        //    return JsonConvert.SerializeObject(fight);
        //}

        public string EndCombat(RequestVm request)
        {
            var combatEnd = ValidateRequestDeserializationIntoEndOfCombat(request.Message);
            var stored = DataService.GetStorage(combatEnd.FightId);
            var fight = JsonConvert.DeserializeObject<FightVmOld>(stored.Value);


            if (fight.BadGuys.Where(s => s.IsAlive).Any() && fight.GoodGuys.Where(s => s.IsAlive).Any())
            {
                throw new Exception(string.Concat(Scribe.ShortMessages.Failure, ": there are still enemies about."));
            }

            DataService.DeleteStorage(stored);

            return string.Concat(Scribe.ShortMessages.Success, ": combat ended.");
        }

        public string JoinParty(RequestVm request)
        {
            var charVm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            var chr = ValidateCharacterByPlayerId(charVm.CharacterId, charVm.PlayerId);

            if (!chr.IsAlive)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is dead."));
            }
            if (chr.IsInFight)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is fighting."));
            }

            var charService = new CharactersService();
            if (chr.IsInParty)
            {
                charService.LeaveParty(chr);
                return Scribe.ShortMessages.Success.ToString();
            }
            else
            {
                charService.JoinParty(chr);
                return Scribe.ShortMessages.Success.ToString();
            }
        }

        public string StartCombatFromStory(RequestVm request)
        {
            var storyvm = ValidateRequestDeserializationInto_CombatStoryVm(request.Message);

            var requester = ValidateCharacterByPlayerId(storyvm.CharacterId, storyvm.PlayerId);
            if (!requester.IsAlive)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is dead."));
            }
            if (requester.IsInFight)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is already in a fight."));
            }

            var requesterVm = new CharacterVm(requester);

            var act = DataService.GetActById(storyvm.ActId);

            return StartCombat(requesterVm, act.Name);
        }

        public string StartCombat(CharacterVm requester, string renown)
        {
            var fightService = new FightService();

            var fightvm = fightService.StartAFight(requester); // the fight starts here
            fightvm.FightDetails.Renown = renown;

            fightvm.FightDetails.TacticalSituation = DecideTacticalSituation(fightvm.GoodGuys, fightvm.BadGuys);
            if (fightvm.FightDetails.TacticalSituation.Equals(TacticalSituation.Major_tactical_disadvantage) ||
                fightvm.FightDetails.TacticalSituation.Equals(TacticalSituation.Slight_tactical_disadvantage))
            {
                fightvm = RollNpcAttack(fightvm);
            }
            else
            {
                fightvm.FightDetails.LastActionResult = "The advantage is yours";
            }

            var fight = new Fight()
            {
                Id = fightvm.FightId,
                GoodGuys = JsonConvert.SerializeObject(fightvm.GoodGuys),
                BadGuys = JsonConvert.SerializeObject(fightvm.BadGuys),
                FightDetails = JsonConvert.SerializeObject(fightvm.FightDetails)
            };

            DataService.UpdateFight(fight);

            return JsonConvert.SerializeObject(fightvm);
        }
        
        #endregion

        #region Getters
        #endregion


    }
}

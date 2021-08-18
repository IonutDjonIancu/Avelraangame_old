using Avelraangame.Models;
using Avelraangame.Models.ModelScraps;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static Avelraangame.Services.ServiceUtils.CombatUtils;

namespace Avelraangame.Services.SubService
{
    public class CombatSubService : ServiceBase
    {
        protected TacticalSituation DecideTacticalSituation(List<CharacterVm> goodGuys, List<CharacterVm> badGuys) // the highest skill in the party decides the tactics
        {
            var goodGuyTactician = goodGuys.OrderByDescending(s => s.Skills.Tactics).First();
            var badGuyTactician = badGuys.OrderByDescending(s => s.Skills.Tactics).First();

            var goodRoll = goodGuyTactician.Skills.Tactics * Dice.Roll_d_20();
            var badRoll = badGuyTactician.Skills.Tactics * Dice.Roll_d_20();

            var diff = goodRoll - badRoll;

            if (diff < -100)
            {
                return TacticalSituation.Major_tactical_disadvantage;
            }
            else if (diff >= -100 && diff < 0)
            {
                return TacticalSituation.Slight_tactical_disadvantage;
            }
            else if (diff > 50 && diff <= 100)
            {
                return TacticalSituation.Slight_tactical_advantage;
            }
            else if (diff > 100)
            {
                return TacticalSituation.Major_tactical_advantage;
            }
            else
            {
                return TacticalSituation.OnPar;
            }
        }

        protected (CharacterVm att, CharacterVm def, int dmgDone) RollAttack(CharacterVm attacker, CharacterVm defender)
        {
            var attackerRoll = Dice.Roll_d_20();
            var attackResult = attacker.Skills.Melee * attackerRoll / 10;
            var defenderRoll = Dice.Roll_d_20();
            var defendResult = defender.Skills.Melee * defenderRoll / 10;

            attacker.AttackToken = false;

            var rollResult = attackResult - defendResult;
            if (rollResult <= 0)
            {
                return (attacker, defender, rollResult);
            }

            rollResult += attacker.Assets.Harm;

            var damage = rollResult - rollResult * defender.Expertise.DRM / 100;
            defender.Assets.Health -= damage;

            if (defender.Assets.Health <= 0)
            {
                defender.IsAlive = false;
                MarkForDeath(defender.CharacterId);
            }

            return (attacker, defender, rollResult);
        }

        protected Fight RollNpcAttack(Fight fight)
        {
            var attackingNpc = fight.BadGuys.Where(s => s.AttackToken).FirstOrDefault();
            if (attackingNpc == null) { return fight; }

            var howManyGoodGuys = fight.GoodGuys.Where(s => s.IsAlive.Equals(true)).ToList().Count - 1;
            var attackedGoodGuy = Dice.Roll_0_to_max(howManyGoodGuys);

            var (npc, goodguy, dmg) = RollAttack(attackingNpc, fight.GoodGuys.Where(s => s.IsAlive.Equals(true)).ToList()[attackedGoodGuy]);

            var indexGoood = fight.GoodGuys.FindIndex(s => s.CharacterId == goodguy.CharacterId);
            fight.GoodGuys.RemoveAt(indexGoood);
            fight.GoodGuys.Add(goodguy);

            if (dmg > 0)
            {
                fight.LastActionResult = string.Concat(Scribe.ShortMessages.NpcDmg, $": {dmg} dmg taken.");
            }
            else
            {
                fight.LastActionResult = string.Concat(Scribe.ShortMessages.NpcDmg, $": npc missed!");
            }

            var result = RollNpcAttack(fight);

            return result;
        }

        protected Fight ReimburseTokens(Fight fight)
        {
            for (int i = 0; i < fight.GoodGuys.Count; i++)
            {
                fight.GoodGuys[i].AttackToken = true;
            }
            for (int j = 0; j < fight.BadGuys.Count; j++)
            {
                fight.BadGuys[j].AttackToken = true;
            }

            return fight;
        }

        protected List<Guid> GenerateAttackOrder(List<CharacterVm> Goodguys, List<CharacterVm> Badguys)
        {
            var allCharacters = new List<CharacterVm>();
            foreach (var item in Goodguys.Where(s => s.IsAlive))
            {
                allCharacters.Add(item);
            }
            foreach (var item in Badguys.Where(s => s.IsAlive))
            {
                allCharacters.Add(item);
            }

            var charRollDictionary = new Dictionary<Guid, int>();

            foreach (var chr in allCharacters)
            {
                var roll = chr.Skills.Tactics * Dice.Roll_d_20();
                charRollDictionary.Add(chr.CharacterId, roll);
            }

            var result = charRollDictionary.OrderByDescending(s => s.Value);

            return result.Select(s => s.Key).ToList();
        }

        protected void MarkForDeath(Guid characterId)
        {
            var character = DataService.GetCharacterById(characterId);
            if (character == null) { return; }
            character.IsAlive = false;

            DataService.UpdateCharacter(character);
        }

        protected string EndFight(Fight fight)
        {
            if (fight.GoodGuys.Where(s => s.IsAlive).Count() <= 0)
            {
                foreach (var chr in fight.GoodGuys)
                {
                    var dbchr = DataService.GetCharacterById(chr.CharacterId);
                    dbchr.FightId = null;
                    dbchr.InFight = false;

                    DataService.UpdateCharacter(dbchr);
                }

                return string.Concat(Scribe.ShortMessages.Failure, ": defeat!");
            }

            if (fight.BadGuys.Where(s => s.IsAlive).Count() <= 0)
            {
                foreach (var chr in fight.GoodGuys)
                {
                    var dbchr = DataService.GetCharacterById(chr.CharacterId);
                    dbchr.FightId = null;
                    dbchr.InFight = false;
                    dbchr.Logbook = IncrementFightsWon(dbchr.Logbook);

                    DataService.UpdateCharacter(dbchr);
                }

                return string.Concat(Scribe.ShortMessages.Success, ": victory!");
            }
            
            return fight.LastActionResult;
        }

        private string IncrementFightsWon(string logbook)
        {
            var log = JsonConvert.DeserializeObject<Logbook>(logbook);

            log.Fights++;

            return JsonConvert.SerializeObject(log);
        }
    }
}

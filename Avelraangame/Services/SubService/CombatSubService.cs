using Avelraangame.Models;
using Avelraangame.Models.POCOs;
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
        protected string DecideTacticalSituation(List<CharacterVm> goodGuys, List<CharacterVm> badGuys) // the highest skill in the party decides the tactics
        {
            var goodGuyTactician = goodGuys.OrderByDescending(s => s.Skills.Tactics).First();
            var badGuyTactician = badGuys.OrderByDescending(s => s.Skills.Tactics).First();

            double attackerRoll = Dice.Roll_d_20() * 0.05;
            double attackResult = goodGuyTactician.Skills.Tactics * attackerRoll;
            double defenderRoll = Dice.Roll_d_20() * 0.05;
            double defendResult = badGuyTactician.Skills.Tactics * defenderRoll;

            var diff = attackResult - defendResult;

            if (diff < -100)
            {
                return TacticalSituation.Major_tactical_disadvantage.ToString();
            }
            else if (diff >= -100 && diff < 0)
            {
                return TacticalSituation.Slight_tactical_disadvantage.ToString();
            }
            else if (diff > 50 && diff <= 100)
            {
                return TacticalSituation.Slight_tactical_advantage.ToString();
            }
            else if (diff > 100)
            {
                return TacticalSituation.Major_tactical_advantage.ToString();
            }
            else
            {
                return TacticalSituation.OnPar.ToString();
            }
        }

        protected (CharacterVm attacker, CharacterVm target, int dmgDone) RollAttack(CharacterVm attacker, CharacterVm target)
        {
            double attackerRoll = Dice.Roll_d_20() * 0.05;
            double attackResult = attacker.Skills.Melee * attackerRoll;
            double defenderRoll = Dice.Roll_d_20() * 0.05;
            double defendResult = target.Skills.Melee * defenderRoll;

            attacker.AttackToken = false;

            double hitResult = attackResult - defendResult;
            if (hitResult <= 0)
            {
                return (attacker, target, 0);
            }

            double hitDmg = attacker.Assets.Harm * hitResult / 100;

            double damage = hitDmg - hitDmg * target.Expertise.DRM / 100;
            target.Assets.Health -= (int)Math.Round(damage);

            if (target.Assets.Health <= 0)
            {
                target.IsAlive = false;
                MarkForDeath(target.CharacterId);
            }

            return (attacker, target, (int)damage);
        }

        protected FightVm RollNpcAttack(FightVm fight)
        {
            var attackingNpc = fight.BadGuys.Where(s => s.AttackToken && s.IsAlive).FirstOrDefault();
            if (attackingNpc == null) 
            { 
                return fight; 
            }

            var howManyGoodGuys = fight.GoodGuys.Where(s => s.IsAlive).ToList().Count - 1;
            if (howManyGoodGuys < 0)
            {
                return fight;
            }

            var attackedGoodGuy = Dice.Roll_0_to_max(howManyGoodGuys);

            var (npc, goodguy, dmg) = RollAttack(attackingNpc, fight.GoodGuys.Where(s => s.IsAlive).ToList()[attackedGoodGuy]);

            var indexGoood = fight.GoodGuys.FindIndex(s => s.CharacterId.Equals(goodguy.CharacterId));
            fight.GoodGuys.RemoveAt(indexGoood);
            fight.GoodGuys.Add(goodguy);

            if (dmg <= 0)
            {
                fight.FightDetails.LastActionResult = "You deflect glancing blows";
            }
            else if (dmg > 0 && dmg <= 200)
            {
                fight.FightDetails.LastActionResult = "You sustain superficial wounds";
            }
            else if (dmg > 200 && dmg <= 1000)
            {
                fight.FightDetails.LastActionResult = "You\'re injured";
            }
            else
            {
                fight.FightDetails.LastActionResult = "You suffer heavy injuries";
            }

            var result = RollNpcAttack(fight);
                       
            return result;
        }

        protected FightVm ReimburseTokens(FightVm fight)
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
            if (character == null)
            { 
                return; 
            }
            character.IsAlive = false;

            DataService.UpdateCharacter(character);
        }

        protected FightVm EndFight(FightVm fightvm)
        {
            if (fightvm.GoodGuys.Where(s => s.IsAlive).Count() <= 0)
            {
                foreach (var chr in fightvm.GoodGuys)
                {
                    var dbchr = DataService.GetCharacterById(chr.CharacterId);
                    if (!dbchr.IsAlive)
                    {
                        continue;
                    }

                    dbchr.IsAlive = false;
                    dbchr.FightId = null;
                    dbchr.IsInFight = false;
                    dbchr.PartyId = null;
                    dbchr.IsInParty = false;

                    DataService.UpdateCharacter(dbchr);
                }

                fightvm.FightDetails.LastActionResult = string.Join(": ", Scribe.ShortMessages.Failure, "defeat!");
                return fightvm;
            }

            if (fightvm.BadGuys.Where(s => s.IsAlive).Count() <= 0)
            {
                foreach (var chr in fightvm.GoodGuys)
                {
                    var dbchr = DataService.GetCharacterById(chr.CharacterId);
                    dbchr.FightId = null;
                    dbchr.IsInFight = false;
                    dbchr.Logbook = IncrementFightsWon(dbchr.Logbook);
                    dbchr.Supplies = LootSupplies(dbchr.Supplies, dbchr.Id, fightvm.FightDetails.Loot);

                    DataService.UpdateCharacter(dbchr);
                }

                fightvm.FightDetails.LastActionResult = string.Concat(Scribe.ShortMessages.Success, ": victory! You loot supplies and leave.");
                return fightvm;
            }

            return fightvm;
        }

        protected FightVm ValidateFightDetails(AttackVm attackvm)
        {
            var fservice = new FightService();

            var fightvm = fservice.GetFightById(attackvm.FightId);

            if (!fightvm.GoodGuys.Where(s => s.CharacterId.Equals(attackvm.AttackerId)).Any())
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.BadRequest, "attacker id is incompatible with fight."));
            }

            if (!fightvm.BadGuys.Where(s => s.CharacterId.Equals(attackvm.TargetId)).Any())
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.BadRequest, "target id is incompatible with fight."));
            }

            return fightvm;
        }

        private string LootSupplies(string oldSupplies, Guid characterId, List<ItemVm> loot)
        {
            var supplies = JsonConvert.DeserializeObject<List<ItemVm>>(oldSupplies);
            var itemsService = new ItemsService();

            foreach (var item in loot)
            {
                supplies.Add(item);
                item.InSlot = ItemsUtils.Slots.Supplies;
                item.IsEquipped = false;

                var itm = new Item
                {
                    Id = item.Id,
                    Bonuses = JsonConvert.SerializeObject(item.Bonuses),
                    CharacterId = characterId,
                    InSlot = item.InSlot,
                    IsConsumable = item.IsConsumable,
                    IsEquipped = item.IsEquipped,
                    Level = item.Level,
                    Name = item.Name,
                    Slots = JsonConvert.SerializeObject(item.Slots),
                    Type = itemsService.GetItemType(item.Type),
                    Worth = item.Worth
                };

                DataService.CreateItem(itm);
            }

            return JsonConvert.SerializeObject(supplies);
        }

        private string IncrementFightsWon(string logbook)
        {
            var log = JsonConvert.DeserializeObject<Logbook>(logbook);

            log.Fights++;

            return JsonConvert.SerializeObject(log);
        }

       
    }
}

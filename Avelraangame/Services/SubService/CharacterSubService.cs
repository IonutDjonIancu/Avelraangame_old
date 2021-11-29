using Avelraangame.Models;
using Avelraangame.Models.POCOs;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avelraangame.Services.SubService
{

    public class CharacterSubService : ServiceBase
    {
        protected CharacterVm GenerateNPC(int minHitMarker, int maxHitMarker, Guid fightId)
        {
            var entityLevel = NPC_level();
            var experience = NPC_experience();
            var stats = NPC_stats(experience);
            var expertise = NPC_expertise(experience);
            var assets = NPC_assets(experience);
            var skills = NPC_skills(minHitMarker, maxHitMarker, experience);
            var logbook = NPC_logbook(entityLevel);
            var equipment = NPC_Equipment();

            var npc = new CharacterVm
            {
                CharacterId = Guid.NewGuid(),
                Name = "Enemy",

                Stats = stats,
                Expertise = expertise,
                Assets = assets,
                Skills = skills,
                Logbook = logbook,
                Equippment = equipment,

                FightId = fightId,
                AttackToken = true,
                InFight = true,
                IsAlive = true
            };

            return GetCalculatedCharacter(npc.CharacterId, npc);
        }


        protected void CompareTempsWithRequest(CharacterVm charVm, Character chr, List<TempInfo> temps)
        {
            var charVmStatsSum = charVm.Stats.Strength + charVm.Stats.Toughness + charVm.Stats.Awareness + charVm.Stats.Abstract;
            var stats = JsonConvert.DeserializeObject<Stats>(chr.Stats);
            var chrStatsSum = stats.Strength + stats.Toughness + stats.Awareness + stats.Abstract;
            var tempStatsSum = temps.Where(s => s.BonusTo.Equals(TempUtils.BonusTo.Stats)).Select(s => s.Value).Sum();
            CompareStatsWithRequest(charVmStatsSum, chrStatsSum, tempStatsSum);

            var charVmSkillsSum = charVm.Skills.Apothecary + // because C# is so retarded it doesn't have a fucking sum method in math
                charVm.Skills.Arcane +
                charVm.Skills.Dodge +
                charVm.Skills.Hide +
                charVm.Skills.Melee +
                charVm.Skills.Navigation +
                charVm.Skills.Psionics +
                charVm.Skills.Ranged +
                charVm.Skills.Resistance +
                charVm.Skills.Scouting +
                charVm.Skills.Social +
                charVm.Skills.Spot +
                charVm.Skills.Survival +
                charVm.Skills.Tactics +
                charVm.Skills.Traps +
                charVm.Skills.Unarmed;
            var skills = JsonConvert.DeserializeObject<Skills>(chr.Skills);
            var chrSkillsSum = skills.Apothecary +
                skills.Arcane +
                skills.Dodge +
                skills.Hide +
                skills.Melee +
                skills.Navigation +
                skills.Psionics +
                skills.Ranged +
                skills.Resistance +
                skills.Scouting +
                skills.Social +
                skills.Spot +
                skills.Survival +
                skills.Tactics +
                skills.Traps +
                skills.Unarmed;
            var tempSkillsSum = temps.Where(s => s.BonusTo.Equals(TempUtils.BonusTo.Skills)).Select(s => s.Value).Sum();
            CompareSkillsWithRequest(charVmSkillsSum, chrSkillsSum, tempSkillsSum);
        }

        private void CompareSkillsWithRequest(int charVmSkillsSum, int chrSkillsSum, int tempSkillsSum)
        {
            if (charVmSkillsSum > chrSkillsSum + tempSkillsSum)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": requested skills do not equal stored skills."));
            }
        }

        private void CompareStatsWithRequest(int charVmStatsSum, int chrStatsSum, int tempStatsSum)
        {
            if (charVmStatsSum > chrStatsSum + tempStatsSum)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": requested stats do not equal stored stats."));
            }
        }


        protected int GetEntityLevelByRoll(int roll)
        {
            if (roll < 20)
            {
                return 1;
            }
            else if (roll >= 20 && roll < 40)
            {
                return 2;
            }
            else if (roll >= 40 && roll < 60)
            {
                return 3;
            }
            else if (roll >= 60 && roll < 80)
            {
                return 4;
            }
            else if (roll >= 80 && roll < 100)
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }

        protected Character CreateHumanCharacter(CharacterVm charVm)
        {
            var items = new ItemsService();

            var chrId = Guid.NewGuid();
            var roll = charVm.Logbook.StatsRoll;

            var stats = new Stats
            {
                Strength = 10,
                Toughness = 10,
                Awareness = 10,
                Abstract = 10
            };

            var expertise = new Expertise
            {
                DRM = 0,
                Experience = 0
            };

            var assets = new Assets
            {
                Harm = 1,
                Health = 10,
                Mana = 0
            };

            var race = (CharactersUtils.Races)1;
            var culture = (CharactersUtils.Cultures)1;
            var logbook = new Logbook
            {
                Wealth = 0,
                EntityLevel = GetEntityLevelByRoll(roll),
                StatsRoll = roll,
                ItemsRoll = Dice.Roll_min_to_max(2, 12),
                PortraitNr = Dice.Roll_min_to_max(1, 7),
                Race = race.ToString(),
                Culture = culture.ToString(),
            };
            
            var skills = new Skills
            {
                Apothecary = 10,
                Arcane = 10,
                Dodge = 10,
                Hide = 10,
                Melee = 10,
                Navigation = 10,
                Psionics = 10,
                Ranged = 10,
                Resistance = 10,
                Scouting = 10,
                Social = 10,
                Spot = 10,
                Survival = 10,
                Tactics = 10,
                Traps = 10,
                Unarmed = 10
            };
            skills = ApplyCultureTraits(culture, skills);

            var supplies = new List<ItemVm>();
            for (int i = 0; i < logbook.ItemsRoll; i++)
            {
                var item = items.GenerateRandomItem(chrId.ToString());
                supplies.Add(item);
            }
            supplies = HeroesBoon(supplies, chrId); // To be removed after the perks system is created

            var chr = new Character
            {
                Id = chrId,
                PlayerId = charVm.PlayerId,

                Name = ValidateCharacterName(charVm.Name),

                Stats = JsonConvert.SerializeObject(stats),
                Expertise = JsonConvert.SerializeObject(expertise),
                Assets = JsonConvert.SerializeObject(assets),
                Skills = JsonConvert.SerializeObject(skills),

                IsAlive = true,
                HasLevelup = true,
                IsInParty = false,
                Logbook = JsonConvert.SerializeObject(logbook),
                Supplies = JsonConvert.SerializeObject(supplies)
            };

            return chr;
        }

        protected CharacterVm GetCalculatedCharacter(Guid charId, CharacterVm characterVm = null)
        {
            CharacterVm charVm;

            if (characterVm != null)
            {
                charVm = characterVm;
            }
            else
            {
                var chr = DataService.GetCharacterById(charId);
                charVm = new CharacterVm(chr);
            }

            charVm.Stats.Strength = FormulaStr(charVm);
            charVm.Stats.Toughness = FormulaTou(charVm);
            charVm.Stats.Awareness = FormulaAwa(charVm);
            charVm.Stats.Abstract = FormulaAbs(charVm);

            charVm.Expertise.Experience = FormulaExp(charVm);
            charVm.Expertise.DRM = FormulaDRM(charVm);

            charVm.Assets.Harm = FormulaHarm(charVm);
            charVm.Assets.Health = FormulaHealth(charVm);
            charVm.Assets.Mana = FormulaMana(charVm);

            // TODO: calculate skills as well
            charVm = FormulaSkills(charVm);

            return charVm;
        }

        private Skills ApplyCultureTraits(CharactersUtils.Cultures culture, Skills skills)
        {
            switch (culture)
            {
                case CharactersUtils.Cultures.Danarian:
                    skills.Melee += 120;
                    skills.Ranged += 60;
                    skills.Navigation -= 200;
                    break;
                case CharactersUtils.Cultures.Ravanon:
                    skills.Melee += 120;
                    skills.Ranged += 60;
                    skills.Navigation -= 200;
                    break;
                case CharactersUtils.Cultures.Midheim:
                    skills.Melee += 120;
                    skills.Ranged += 60;
                    skills.Navigation -= 200;
                    break;
                case CharactersUtils.Cultures.Endarian:
                    skills.Melee += 120;
                    skills.Ranged += 60;
                    skills.Navigation -= 200;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return skills;
        }

        private List<ItemVm> HeroesBoon(List<ItemVm> supplies, Guid charId)
        {
            var items = new ItemsService();
            var ring = items.GenerateRandomItem(charId.ToString());
            var item = items.GetItemById(ring.Id);

            var heroBoonRing = new ItemVm(item);
            heroBoonRing.Bonuses.ToHealth += 2000;
            heroBoonRing.Level = 4;
            heroBoonRing.IsConsumable = false;
            heroBoonRing.Name = "Hero's Boon ring";
            heroBoonRing.Slots = items.GetItemSlotsByType(ItemsUtils.Types.Apparatus);
            heroBoonRing.Type = ItemsUtils.Types.Apparatus.ToString();
            supplies.Add(heroBoonRing);

            item.Id = heroBoonRing.Id;
            item.CharacterId = heroBoonRing.CharacterId;
            item.Bonuses = JsonConvert.SerializeObject(heroBoonRing.Bonuses);
            item.InSlot = heroBoonRing.InSlot;
            item.IsConsumable = heroBoonRing.IsConsumable;
            item.IsEquipped = false;
            item.Level = 3;
            item.Name = heroBoonRing.Name;
            item.Slots = heroBoonRing.Slots.ToString();
            item.Type = ItemsUtils.Types.Apparatus;
            item.Worth = heroBoonRing.Worth;
            DataService.UpdateItem(item);

            return supplies;
        }

        #region Formulas
        #region Assets formulae
        private int FormulaMana(CharacterVm charVm)
        {
            var fromBase = charVm.Assets != null ? charVm.Assets.Mana : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToMana : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToMana : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToMana : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToMana : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToMana : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            // ((2*TOU + EXP/5) * type + bonuses + base) * ABS/100
            var result = ((2 * charVm.Stats.Toughness + charVm.Expertise.Experience / 5) * charVm.Logbook.EntityLevel + (int)fromItems + fromBase) * charVm.Stats.Abstract / 100;

            return result;
        }
        private int FormulaHealth(CharacterVm charVm)
        {
            var fromBase = charVm.Assets != null ? charVm.Assets.Health : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToHealth : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToHealth : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToHealth : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToHealth : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToHealth : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            // (5*TOU + 2*STR + AWA + EXP/10) * type + bonuses + base
            var result = (5 * charVm.Stats.Toughness + 2 * charVm.Stats.Strength + charVm.Stats.Awareness + charVm.Expertise.Experience / 10) * charVm.Logbook.EntityLevel + (int)fromItems + fromBase;

            return result;
        }
        private int FormulaHarm(CharacterVm charVm)
        {
            var fromBase = charVm.Assets != null ? charVm.Assets.Harm : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToHarm : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToHarm : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToHarm : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToHarm : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToHarm : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            // (2*STR + 2*AWA + EXP/2) * type + bonuses + base
            var result = (2 * charVm.Stats.Strength + 2 * charVm.Stats.Awareness + charVm.Expertise.Experience / 2) * charVm.Logbook.EntityLevel + (int)fromItems + fromBase;

            return result;
        }
        #endregion

        #region Expertise formulae
        private int FormulaDRM(CharacterVm charVm)
        {
            var fromBase = charVm.Expertise != null ? charVm.Expertise.DRM : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToDRM : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToDRM : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToDRM : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToDRM : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToDRM : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            var result = fromBase + (int)fromItems;

            return result;
        }
        private int FormulaExp(CharacterVm charVm)
        {
            var fromBase = charVm.Expertise != null ? charVm.Expertise.Experience : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToExperience : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToExperience : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToExperience : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToExperience : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToExperience : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            var result = fromBase + (int)fromItems;

            return result;
        }
        #endregion

        #region Stats formulae
        private int FormulaAbs(CharacterVm charVm)
        {
            var fromBase = charVm.Stats != null ? charVm.Stats.Abstract : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToAbstract : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToAbstract : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToAbstract : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToAbstract : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses.ToAbstract : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            var result = fromBase + (int)fromItems;

            return result;
        }
        private int FormulaAwa(CharacterVm charVm)
        {
            var fromBase = charVm.Stats != null ? charVm.Stats.Awareness : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToAwareness : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToAwareness : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToAwareness : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToAwareness : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToAwareness : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            var result = fromBase + (int)fromItems;

            return result;
        }
        private int FormulaTou(CharacterVm charVm)
        {
            var fromBase = charVm.Stats != null ? charVm.Stats.Toughness : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToToughness : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToToughness : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToToughness : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToToughness : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s!= null ? s.Bonuses?.ToToughness : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            var result = fromBase + (int)fromItems;

            return result;
        }
        private int FormulaStr(CharacterVm charVm)
        {
            var fromBase = charVm.Stats != null ? charVm.Stats.Strength : 0;
            int? fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToStrength : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToStrength : 0 ) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToStrength : 0 )+
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToStrength : 0)+
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToStrength : 0).Sum() : 0);
            }

            if (fromItems == null)
            {
                fromItems = 0;
            }

            var result = fromBase + (int)fromItems;

            return result;
        }
        #endregion

        #region Skills formulae
        private CharacterVm FormulaSkills(CharacterVm charVm)
        {
            if (charVm.Equippment != null)
            {
                var apothecary = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToApothecary : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToApothecary : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToApothecary : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToApothecary : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToApothecary : 0).Sum() : 0);
                charVm.Skills.Apothecary += (int)apothecary;

                var arcane = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToArcane : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToArcane : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToArcane : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToArcane : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToArcane : 0).Sum() : 0);
                charVm.Skills.Arcane += (int)arcane;

                var dodge = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToDodge : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToDodge : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToDodge : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToDodge : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToDodge : 0).Sum() : 0);
                charVm.Skills.Dodge += (int)dodge;

                var hide = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToHide : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToHide : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToHide : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToHide : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToHide : 0).Sum() : 0);
                charVm.Skills.Hide += (int)hide;

                var melee = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToMelee : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToMelee : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToMelee : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToMelee : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToMelee : 0).Sum() : 0);
                charVm.Skills.Melee += (int)melee;

                var navigation = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToNavigation : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToNavigation : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToNavigation : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToNavigation : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToNavigation : 0).Sum() : 0);
                charVm.Skills.Navigation += (int)navigation;

                var psionics = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToPsionics : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToPsionics : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToPsionics : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToPsionics : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToPsionics : 0).Sum() : 0);
                charVm.Skills.Psionics += (int)psionics;

                var ranged = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToRanged : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToRanged : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToRanged : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToRanged : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToRanged : 0).Sum() : 0);
                charVm.Skills.Ranged += (int)ranged;

                var resistance = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToResistance : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToResistance : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToResistance : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToResistance : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToResistance : 0).Sum() : 0);
                charVm.Skills.Resistance += (int)resistance;

                var scouting = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToScouting : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToScouting : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToScouting : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToScouting : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToScouting : 0).Sum() : 0);
                charVm.Skills.Scouting += (int)scouting;

                var social = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToSocial : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToSocial : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToSocial : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToSocial : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToSocial : 0).Sum() : 0);
                charVm.Skills.Social += (int)social;

                var spot = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToSpot : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToSpot : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToSpot : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToSpot : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToSpot : 0).Sum() : 0);
                charVm.Skills.Spot += (int)spot;

                var survival = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToSurvival : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToSurvival : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToSurvival : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToSurvival : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToSurvival : 0).Sum() : 0);
                charVm.Skills.Survival += (int)survival;

                var tactics = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToTactics : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToTactics : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToTactics : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToTactics : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToTactics : 0).Sum() : 0);
                charVm.Skills.Tactics += (int)tactics;

                var traps = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToTraps : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToTraps : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToTraps : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToTraps : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToTraps : 0).Sum() : 0);
                charVm.Skills.Traps += (int)traps;

                var unarmed = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses?.ToUnarmed : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses?.ToUnarmed : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses?.ToUnarmed : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses?.ToUnarmed : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s != null ? s.Bonuses?.ToUnarmed : 0).Sum() : 0);
                charVm.Skills.Unarmed += (int)unarmed;
            }

            return charVm;
        }

        #endregion

        #region NPC formulae
        private int NPC_level()
        {
            var roll = Dice.Roll_0_to_100();

            if (roll <= 80)
            {
                return 1;
            }
            else if (roll > 80 && roll <= 96)
            {
                return 2;
            }
            else if (roll > 96 && roll <= 99)
            {
                return 3;
            }
            else
            {
                var secondRoll = Dice.Roll_0_to_100();
                
                if (secondRoll <= 90)
                {
                    return 4;
                }
                else if (secondRoll > 90 && secondRoll <= 99)
                {
                    return 5;
                }
                else
                {
                    return 6;
                }
            }
        }

        private int NPC_experience()
        {
            var roll = Dice.Roll_d_20();

            if (roll < 20) // level 1
            {
                return Dice.Roll_min_to_max(20, 100);
            }
            else if (roll > 20 && roll < 40) // level 2
            {
                return Dice.Roll_min_to_max(50, 300);
            }
            else if (roll > 40 && roll < 60) // level 3
            {
                return Dice.Roll_min_to_max(200, 600);
            }
            else if (roll > 60 && roll < 80) // level 4
            {
                return Dice.Roll_min_to_max(400, 1000);
            }
            else if (roll > 80 && roll < 100) // level 5
            {
                return Dice.Roll_min_to_max(800, 3000);
            }
            else // level 6
            {
                return Dice.Roll_min_to_max(2000, 20000);
            }
        }

        private Stats NPC_stats(int exp)
        {
            return new Stats
            {
                Strength = Dice.Roll_min_to_max(10, exp),
                Toughness = Dice.Roll_min_to_max(10, exp),
                Awareness = Dice.Roll_min_to_max(10, exp),
                Abstract = Dice.Roll_min_to_max(10, exp)
            };
        }

        private Expertise NPC_expertise(int exp)
        {
            return new Expertise
            {
                DRM = 0,
                Experience = exp
            };
        }

        private Assets NPC_assets(int exp)
        {
            return new Assets
            {
                Harm = Dice.Roll_min_to_max(10, exp),
                Health = Dice.Roll_min_to_max(10, exp),
                Mana = Dice.Roll_min_to_max(10, exp)
            };
        }

        private Skills NPC_skills(int minHitMarker, int maxHitMarker, int exp)
        {
            return new Skills
            {
                Melee = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,
                Ranged = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,
                Arcane = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,
                Dodge = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,
                Hide = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,
                Psionics = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,
                Resistance = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,
                Spot = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,
                Unarmed = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10,

                Tactics = Dice.Roll_min_to_max(minHitMarker, maxHitMarker) + exp / 10
            };
        }

        private Logbook NPC_logbook(int level)
        {
            return new Logbook
            {
                PortraitNr = level,
                EntityLevel = level,
            };
        }

        private Equipment NPC_Equipment()
        {
            var itemService = new ItemsService();

            return new Equipment
            {
                Armour = itemService.GenerateRandomArmour(),
                Mainhand = itemService.GenerateRandomMainHandWeapon(),
                Offhand = itemService.GenerateRandomOffHandWeapon(),
                Ranged = itemService.GenerateRandomRangedWeapon(),
                Trinkets = itemService.GenerateRandomTrinketsStash()
            };
        }


        #endregion
        #endregion

    }

}

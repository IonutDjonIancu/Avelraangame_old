using Avelraangame.Models;
using Avelraangame.Models.ModelScraps;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avelraangame.Services.SubService
{

    public class CharacterSubService : CharacterBase
    {
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

            var logbook = new Logbook
            {
                Wealth = 0,
                EntityLevel = GetEntityLevelByRoll(roll),
                StatsRoll = roll,
                ItemsRoll = Dice.Roll_min_to_max(2, 12),
                PortraitNr = Dice.Roll_min_to_max(1, 7),
                Race = ((CharactersUtils.Races)1).ToString(),
                Culture = ((CharactersUtils.Cultures)1).ToString(),
            };

            var supplies = new List<ItemVm>();

            for (int i = 0; i < logbook.ItemsRoll; i++)
            {
                var item = Items.GenerateRandomItem(chrId.ToString());
                supplies.Add(item);
            }

            var chr = new Character
            {
                Id = Guid.NewGuid(),
                PlayerId = charVm.PlayerId,

                Name = ValidateCharacterName(charVm.Name),

                Stats = JsonConvert.SerializeObject(stats),
                Expertise = JsonConvert.SerializeObject(expertise),
                Assets = JsonConvert.SerializeObject(assets),
                Skills = JsonConvert.SerializeObject(skills),

                IsAlive = true,
                HasLevelup = true,
                InParty = false,
                Logbook = JsonConvert.SerializeObject(logbook),
                Supplies = JsonConvert.SerializeObject(supplies)
            };

            return chr;
        }

        protected CharacterVm GetCalculatedCharacter(Guid charId)
        {
            var chr = DataService.GetCharacterById(charId);
            var charVm = new CharacterVm(chr);

            charVm.Stats.Strength = FormulaStr(charVm);
            charVm.Stats.Toughness = FormulaTou(charVm);
            charVm.Stats.Awareness = FormulaAwa(charVm);
            charVm.Stats.Abstract = FormulaAbs(charVm);

            charVm.Expertise.Experience = FormulaExp(charVm);
            charVm.Expertise.DRM = FormulaDRM(charVm);

            charVm.Assets.Harm = FormulaHarm(charVm);
            charVm.Assets.Health = FormulaHealth(charVm);
            charVm.Assets.Mana = FormulaMana(charVm);

            // will have to calculate skills as well

            return charVm;

        }
        #region Assets formulae
        private int FormulaMana(CharacterVm charVm)
        {
            var fromBase = charVm.Assets.Mana;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToMana : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToMana : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToMana : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToMana : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToMana).Sum() : 0);
            }

            // ((2*TOU + EXP/5) * type + bonuses + base) * ABS/100
            var result = ((2 * charVm.Stats.Toughness + charVm.Expertise.Experience / 5) * charVm.Logbook.EntityLevel + fromItems + fromBase) * charVm.Stats.Abstract / 100;

            return result;
        }
        private int FormulaHealth(CharacterVm charVm)
        {
            var fromBase = charVm.Assets.Health;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToHealth : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToHealth : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToHealth : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToHealth : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToHealth).Sum() : 0);
            }

            // (5*TOU + 2*STR + AWA + EXP/10) * type + bonuses + base
            var result = (5 * charVm.Stats.Toughness + 2 * charVm.Stats.Strength + charVm.Stats.Awareness + charVm.Expertise.Experience / 10) * charVm.Logbook.EntityLevel + fromItems + fromBase;

            return result;
        }
        private int FormulaHarm(CharacterVm charVm)
        {
            var fromBase = charVm.Assets.Harm;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToHarm : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToHarm : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToHarm : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToHarm : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToHarm).Sum() : 0);
            }

            // (2*STR + 2*AWA + EXP/2) * type + bonuses + base
            var result = (2 * charVm.Stats.Strength + 2 * charVm.Stats.Awareness + charVm.Expertise.Experience / 2) * charVm.Logbook.EntityLevel + fromItems + fromBase;

            return result;
        }
        #endregion

        #region Expertise formulae
        private int FormulaDRM(CharacterVm charVm)
        {
            var fromBase = charVm.Expertise.DRM;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToDRM : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToDRM : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToDRM : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToDRM : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToDRM).Sum() : 0);
            }

            var result = fromBase + fromItems;

            return result;
        }
        private int FormulaExp(CharacterVm charVm)
        {
            var fromBase = charVm.Expertise.Experience;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToExperience : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToExperience : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToExperience : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToExperience : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToExperience).Sum() : 0);
            }

            var result = fromBase + fromItems;

            return result;
        }
        #endregion

        #region Stats formulae
        private int FormulaAbs(CharacterVm charVm)
        {
            var fromBase = charVm.Stats.Abstract;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToAbstract : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToAbstract : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToAbstract : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToAbstract : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToAbstract).Sum() : 0);
            }

            var result = fromBase + fromItems;

            return result;
        }
        private int FormulaAwa(CharacterVm charVm)
        {
            var fromBase = charVm.Stats.Awareness;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToAwareness : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToAwareness : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToAwareness : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToAwareness : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToAwareness).Sum() : 0);
            }

            var result = fromBase + fromItems;

            return result;
        }
        private int FormulaTou(CharacterVm charVm)
        {
            var fromBase = charVm.Stats.Toughness;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToToughness : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToToughness : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToToughness : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToToughness : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToToughness).Sum() : 0);
            }

            var result = fromBase + fromItems;

            return result;
        }
        private int FormulaStr(CharacterVm charVm)
        {
            var fromBase = charVm.Stats.Strength;
            var fromItems = 0;
            if (charVm.Equippment != null)
            {
                fromItems = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToStrength : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToStrength : 0 ) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToStrength : 0 )+
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToStrength : 0)+
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToStrength).Sum() : 0);
            }

            var result = fromBase + fromItems;

            return result;
        }
        #endregion





    }

}

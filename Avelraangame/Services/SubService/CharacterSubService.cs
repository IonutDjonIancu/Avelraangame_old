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

    public class CharacterSubService : ServiceBase
    {
        protected CharacterVm GenerateWeakNpc()
        {
            var stats = new Stats
            {
                Strength = 10,
                Toughness = 10,
                Awareness = 10,
                Abstract = 10
            };
            var assets = new Assets
            {
                Harm = 10,
                Health = 100,
                Mana = 0
            };
            var expertise = new Expertise
            {
                DRM = 10,
                Experience = 10
            };
            var skills = new Skills
            {
                Melee = 50
            };
            var logbook = new Logbook
            {
                PortraitNr = 1
            };

            var chr = new Character()
            {
                Id = Guid.NewGuid(),
                Name = "Weak Npc",
                Stats = JsonConvert.SerializeObject(stats),
                Assets = JsonConvert.SerializeObject(assets),
                Expertise = JsonConvert.SerializeObject(expertise),
                Skills = JsonConvert.SerializeObject(skills),
                Logbook = JsonConvert.SerializeObject(logbook),
                IsAlive = true
            };

            return new CharacterVm(chr);
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
                var item = items.GenerateRandomItem(chrId.ToString());
                supplies.Add(item);
            }

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

            // TODO: calculate skills as well
            charVm = FormulaSkills(charVm);

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

        #region Skills formulae
        private CharacterVm FormulaSkills(CharacterVm charVm)
        {
            if (charVm.Equippment != null)
            {
                var apothecary = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToApothecary : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToApothecary : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToApothecary : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToApothecary : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToApothecary).Sum() : 0);
                charVm.Skills.Apothecary += apothecary;

                var arcane = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToArcane : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToArcane : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToArcane : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToArcane : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToArcane).Sum() : 0);
                charVm.Skills.Arcane += arcane;

                var dodge = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToDodge : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToDodge : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToDodge : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToDodge : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToDodge).Sum() : 0);
                charVm.Skills.Dodge += dodge;

                var hide = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToHide : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToHide : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToHide : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToHide : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToHide).Sum() : 0);
                charVm.Skills.Hide += hide;

                var melee = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToMelee : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToMelee : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToMelee : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToMelee : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToMelee).Sum() : 0);
                charVm.Skills.Melee += melee;

                var navigation = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToNavigation : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToNavigation : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToNavigation : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToNavigation : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToNavigation).Sum() : 0);
                charVm.Skills.Navigation += navigation;

                var psionics = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToPsionics : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToPsionics : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToPsionics : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToPsionics : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToPsionics).Sum() : 0);
                charVm.Skills.Psionics += psionics;

                var ranged = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToRanged : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToRanged : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToRanged : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToRanged : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToRanged).Sum() : 0);
                charVm.Skills.Ranged += ranged;

                var resistance = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToResistance : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToResistance : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToResistance : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToResistance : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToResistance).Sum() : 0);
                charVm.Skills.Resistance += resistance;

                var scouting = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToScouting : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToScouting : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToScouting : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToScouting : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToScouting).Sum() : 0);
                charVm.Skills.Scouting += scouting;

                var social = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToSocial : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToSocial : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToSocial : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToSocial : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToSocial).Sum() : 0);
                charVm.Skills.Social += social;

                var spot = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToSpot : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToSpot : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToSpot : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToSpot : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToSpot).Sum() : 0);
                charVm.Skills.Spot += spot;

                var survival = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToSurvival : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToSurvival : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToSurvival : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToSurvival : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToSurvival).Sum() : 0);
                charVm.Skills.Survival += survival;

                var tactics = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToTactics : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToTactics : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToTactics : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToTactics : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToTactics).Sum() : 0);
                charVm.Skills.Tactics += tactics;

                var traps = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToTraps : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToTraps : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToTraps : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToTraps : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToTraps).Sum() : 0);
                charVm.Skills.Traps += traps;

                var unarmed = (charVm.Equippment.Armour != null ? charVm.Equippment.Armour.Bonuses.ToUnarmed : 0) +
                    (charVm.Equippment.Mainhand != null ? charVm.Equippment.Mainhand.Bonuses.ToUnarmed : 0) +
                    (charVm.Equippment.Offhand != null ? charVm.Equippment.Offhand.Bonuses.ToUnarmed : 0) +
                    (charVm.Equippment.Ranged != null ? charVm.Equippment.Ranged.Bonuses.ToUnarmed : 0) +
                    (charVm.Equippment.Trinkets != null ? charVm.Equippment.Trinkets.Select(s => s.Bonuses.ToUnarmed).Sum() : 0);
                charVm.Skills.Unarmed += unarmed;
            }

            return charVm;
        }

        #endregion




    }

}

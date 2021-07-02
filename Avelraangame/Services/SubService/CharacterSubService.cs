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

        //protected Character ModifyHumanCharacter(CharacterVm charVm)
        //{


        //    // calculate, 
        //    // validate and 
        //    // apply modifications before return



        //    var roll = charVm.Logbook.StatsRoll;

        //    var logbook = new Logbook
        //    {
        //        StatsRoll = roll,
        //    };

        //    var chr = new Character
        //    {
        //        Id = Guid.NewGuid(),
        //        PlayerId = charVm.PlayerId,
        //        Name = null,

        //        Strength = 10,
        //        Toughness = 10,
        //        Awareness = 10,
        //        Abstract = 10,

        //        EntityLevel = GetEntityLevelByRoll(roll),
        //        Experience = 0,
        //        DRM = 0,
        //        Wealth = 0,

        //        Health = 10,
        //        Mana = 0,
        //        Harm = 1,

        //        IsAlive = true,
        //        Logbook = JsonConvert.SerializeObject(logbook)
        //    };

        //    return chr;
        //}




    }

}

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


        public Character GenerateHumanCharacter(CharacterVm charVm)
        {
            var roll = charVm.Logbook.StatsRoll;

            var logbook = new Logbook
            {
                StatsRoll = roll,
                EntityLevel = GetEntityLevelByRoll(roll)
            };

            var chr = new Character
            {
                Id = Guid.NewGuid(),
                PlayerId = charVm.PlayerId,
                Name = null,

                Strength = 10,
                Toughness = 10,
                Awareness = 10,
                Abstract = 10,

                Experience = 0,
                DRM = 0,
                Wealth = 0,

                Health = 10,
                Mana = 0,
                Harm = 1,

                IsAlive = true,
                Logbook = JsonConvert.SerializeObject(logbook)
            };

            return chr;
        }



        //public StatsBase GenerateStatsBase()
        //{
        //    var stats = new StatsBase
        //    {
        //        Strength = 10,
        //        Toughness = 10,
        //        Awareness = 10,
        //        Abstract = 10
        //    };

        //    return stats;
        //}

        //public ExpertiseBase GenerateExpertiseBase()
        //{
        //    var exp = new ExpertiseBase
        //    {
        //        DRM = 0,
        //        Experience = 0,
        //        Wealth = 0
        //    };

        //    return exp;
        //}

        //public AssetsBase GenerateAssetsBase()
        //{
        //    var assets = new AssetsBase
        //    {
        //        Health = 10,
        //        Mana = 0,
        //        Harm = 1
        //    };

        //    return assets;
        //}

        //public Logbook GenerateLogbookBase(CharacterVm charVm)
        //{
        //    var logbook = new Logbook
        //    {
        //        EntityLevel = GetEntityLevelByRoll(charVm.StatsRoll),
        //        StatsRoll = charVm.StatsRoll,
        //        IsAlive = true
        //    };

        //    return logbook;
        //}

        //public (StatsVm statsVm, ExpertiseVm expertiseVm, AssetsVm assetsVm) CalculateReturnSEA(Guid charId)
        //{
        //    var chr = DataService.GetCharacterById(charId);

        //    var statsBase = JsonConvert.DeserializeObject<StatsVm>(chr.StatsBase);
        //    var expertiseBase = JsonConvert.DeserializeObject<ExpertiseVm>(chr.ExpertiseBase);
        //    var assetsBase = JsonConvert.DeserializeObject<AssetsVm>(chr.AssetsBase);

        //    var itemsList = DataService.GetEquippedItemsByCharId(charId);

        //    var mainhand = JsonConvert.DeserializeObject<ItemProps>(itemsList.Where(s => s.InSlot == ItemUtils.inventorySlots.Mainhand).FirstOrDefault().Properties);
        //    var offHand = JsonConvert.DeserializeObject<ItemProps>(itemsList.Where(s => s.InSlot == ItemUtils.inventorySlots.Offhand).FirstOrDefault().Properties);
        //    var ranged = JsonConvert.DeserializeObject<ItemProps>(itemsList.Where(s => s.InSlot == ItemUtils.inventorySlots.Ranged).FirstOrDefault().Properties);
        //    var armour = JsonConvert.DeserializeObject<ItemProps>(itemsList.Where(s => s.InSlot == ItemUtils.inventorySlots.Armour).FirstOrDefault().Properties);
        //    var trinkets = GetTrincketsItemPropsTotal(itemsList.Where(s => s.InSlot == ItemUtils.inventorySlots.Trinkets).ToList());


        //    var stats = new StatsVm
        //    {
        //        Strength = statsBase.Strength + mainhand.Stats_toStrength + offHand.Stats_toStrength + ranged.Stats_toStrength + armour.Stats_toStrength + trinkets.Stats_toStrength,
        //        Toughness = statsBase.Toughness + mainhand.Stats_toToughness + offHand.Stats_toToughness + ranged.Stats_toToughness + armour.Stats_toToughness + trinkets.Stats_toToughness,
        //        Awareness = statsBase.Awareness + mainhand.Stats_toAwareness + offHand.Stats_toAwareness + ranged.Stats_toAwareness + armour.Stats_toAwareness + trinkets.Stats_toAwareness,
        //        Abstract = statsBase.Abstract + mainhand.Stats_toAbstract + offHand.Stats_toAbstract + ranged.Stats_toAbstract + armour.Stats_toAbstract + trinkets.Stats_toAbstract
        //    };

        //    var expertise = new ExpertiseVm
        //    {
        //        Experience = expertiseBase.Experience + mainhand.Expertise_toExperience + offHand.Expertise_toExperience + ranged.Expertise_toExperience + armour.Expertise_toExperience + trinkets.Expertise_toExperience,
        //        DRM = expertiseBase.DRM + mainhand.Expertise_toDRM + offHand.Expertise_toDRM + ranged.Expertise_toDRM + armour.Expertise_toDRM + trinkets.Expertise_toDRM,
        //        Wealth = expertiseBase.Wealth + mainhand.Expertise_toWealth + offHand.Expertise_toWealth + ranged.Expertise_toWealth + armour.Expertise_toWealth + trinkets.Expertise_toWealth
        //    };

        //    var assets = new AssetsVm
        //    {
        //        Health = AssetsUtils.CalculateAssets
        //    }


        //}

        //private ItemProps GetTrincketsItemPropsTotal(List<Item> trinkets)
        //{
        //    var trinksProps = new List<ItemProps>();

        //    foreach (var item in trinkets)
        //    {
        //        var props = JsonConvert.DeserializeObject<ItemProps>(item.Properties);
        //        trinksProps.Add(props);
        //    }


        //    var finalProps = new ItemProps
        //    {
        //        Stats_toStrength = trinksProps.Select(s => s.Stats_toStrength).Sum(),
        //        Stats_toToughness = trinksProps.Select(s => s.Stats_toToughness).Sum(),
        //        Stats_toAwareness = trinksProps.Select(s => s.Stats_toAwareness).Sum(),
        //        Stats_toAbstract = trinksProps.Select(s => s.Stats_toAbstract).Sum(),

        //        Expertise_toDRM = trinksProps.Select(s => s.Expertise_toDRM).Sum(),
        //        Expertise_toExperience = trinksProps.Select(s => s.Expertise_toExperience).Sum(),
        //        Expertise_toWealth = trinksProps.Select(s => s.Expertise_toWealth).Sum(),

        //        Assets_toHealth = trinksProps.Select(s => s.Assets_toHealth).Sum(),
        //        Assets_toMana = trinksProps.Select(s => s.Assets_toMana).Sum(),
        //        Assets_toHarm = trinksProps.Select(s => s.Assets_toHarm).Sum(),

        //        //Skills_toApothecary = trinksProps.Select(s => s.Skills_toApothecary).Sum(),
        //        //Skills_toArcane = trinksProps.Select(s => s.Skills_toArcane).Sum(),
        //        //Skills_toDodge = trinksProps.Select(s => s.Skills_toDodge).Sum(),
        //        //Skills_toHide = trinksProps.Select(s => s.Skills_toHide).Sum(),
        //        //Skills_toMelee = trinksProps.Select(s => s.Skills_toMelee).Sum(),
        //        //Skills_toNavigation = trinksProps.Select(s => s.Skills_toNavigation).Sum(),
        //        //Skills_toPsionics = trinksProps.Select(s => s.Skills_toPsionics).Sum(),
        //        //Skills_toRanged = trinksProps.Select(s => s.Skills_toRanged).Sum(),
        //        //Skills_toResistance = trinksProps.Select(s => s.Skills_toResistance).Sum(),
        //        //Skills_toScouting = trinksProps.Select(s => s.Skills_toScouting).Sum(),
        //        //Skills_toSocial = trinksProps.Select(s => s.Skills_toSocial).Sum(),
        //        //Skills_toSpot = trinksProps.Select(s => s.Skills_toSpot).Sum(),
        //        //Skills_toSurvival = trinksProps.Select(s => s.Skills_toSurvival).Sum(),
        //        //Skills_toTactics = trinksProps.Select(s => s.Skills_toTactics).Sum(),
        //        //Skills_toTraps = trinksProps.Select(s => s.Skills_toTraps).Sum(),
        //        //Skills_toUnarmed = trinksProps.Select(s => s.Skills_toUnarmed).Sum()
        //    };


        //    return finalProps;
        //}

    }

}

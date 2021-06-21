using Avelraangame.Models;
using Avelraangame.Models.ModelScraps;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Avelraangame.Services.SubService
{
    public class ItemsSubService : ItemBase
    {
        protected SkillsSubService Skills { get; set; }
        protected AssetsSubService Assets { get; set; }
        protected StatsSubService Stats { get; set; }
        protected ItemsSubService()
        {
            Skills = new SkillsSubService();
            Assets = new AssetsSubService();
            Stats = new StatsSubService();
        }

        protected Item GenerateNormalItem(int itemLevel)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Level = itemLevel,
                Type = GenerateItemType(),
                IsEquipped = false,
                InSlot = ItemUtils.Slots.Supplies,
            };

            item.Slots = GenerateItemSlotsByType(item.Type).ToString();
            item.Name = GenerateItemNameByLevelAndType(item.Level, item.Type);
            item.Worth = GenerateItemWorthByLevelAndType(item.Level, item.Type);
            item.IsConsumable = IsConsumable(item.Type);

            var bonuses = GenerateItemBonusesByLevelAndType(item.Level, item.Type); // bonuses

            if (bonuses.ToWealth > 0)
            {
                item.Worth += bonuses.ToWealth;
            }

            item.Bonuses = JsonConvert.SerializeObject(bonuses);

            return item;
        }

        private List<ItemUtils.Slots> GenerateItemSlotsByType(ItemUtils.Types type)
        {
            if (type.Equals(ItemUtils.itemTypes.Apparatus))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Mainhand,
                        ItemUtils.Slots.Offhand,
                        ItemUtils.Slots.Ranged,
                        ItemUtils.Slots.Armour,
                        ItemUtils.Slots.Trinkets,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemUtils.itemTypes.Armour))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Armour,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemUtils.itemTypes.Axe))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Mainhand,
                        ItemUtils.Slots.Offhand,
                        ItemUtils.Slots.Ranged,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemUtils.itemTypes.Bow))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Ranged,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemUtils.itemTypes.Crossbow))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Mainhand,
                        ItemUtils.Slots.Ranged,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemUtils.itemTypes.Polearm))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Mainhand,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemUtils.itemTypes.Shield))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Mainhand,
                        ItemUtils.Slots.Offhand,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemUtils.itemTypes.Spear))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Mainhand,
                        ItemUtils.Slots.Offhand,
                        ItemUtils.Slots.Ranged,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemUtils.itemTypes.Valuables))
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
            else
            {
                var returnList = new List<ItemUtils.Slots>()
                    {
                        ItemUtils.Slots.Mainhand,
                        ItemUtils.Slots.Offhand,
                        ItemUtils.Slots.Supplies
                    };
                return returnList;
            }
        }

        private bool IsConsumable(ItemUtils.Types type)
        {
            if (type.Equals(ItemUtils.Types.Apparatus))
            {
                var random = Dice.Roll_min_to_max(1, 2);

                if (random == 1) return true;

                return false;
            }
            else if (type.Equals(ItemUtils.Types.Valuables))
            {
                return true;
            }

            return false;
        }

        private int GenerateItemWorthByLevelAndType(int level, ItemUtils.Types type)
        {
            if (type.Equals(ItemUtils.Types.Valuables)) return 0;

            return level switch
            {
                1 => Dice.Roll_x_to_20(1) + 10,
                2 => Dice.Roll_x_to_20(2) + 20,
                3 => Dice.Roll_x_to_20(4) + 30,
                4 => Dice.Roll_x_to_20(8) + 40,
                _ => 0
            };
        }

        private ItemBonuses GenerateItemBonusesByLevelAndType(int level, ItemUtils.Types type)
        {
            var bonuses = new ItemBonuses();

            if (type.Equals(ItemUtils.Types.Armour)) return ReturnBonusesForArmour(bonuses, level);
            else if (type.Equals(ItemUtils.Types.Shield)) return ReturnPropsForShield(bonuses, level);
            else if (type.Equals(ItemUtils.Types.Valuables)) return ReturnPropsForValuables(bonuses, level);
            else if (type.Equals(ItemUtils.Types.Apparatus)) return ReturnPropsForApparatus(bonuses, level);
            else return ReturnPropsForWeapons(bonuses, level, type);
        }


        private ItemBonuses ReturnPropsForWeapons(ItemBonuses bonuses, int level, ItemUtils.Types weaponType)
        {
            if (level == 1)
            {
                bonuses.ToHarm = Dice.Roll_d_20() + 10;

                if (weaponType.Equals(ItemUtils.Types.Sword))
                {
                    bonuses.ToHarm *= 2;
                }
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToHarm = Dice.Roll_d_20() + 20;

                if (weaponType.Equals(ItemUtils.Types.Bow) ||
                    weaponType.Equals(ItemUtils.Types.Crossbow))
                {
                    bonuses.ToRanged = Dice.Roll_d_20() + 20;
                }
                else
                {
                    bonuses.ToMelee = Dice.Roll_d_20() + 20;
                }

                if (weaponType.Equals(ItemUtils.Types.Sword))
                {
                    bonuses.ToHarm *= 2;
                }
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToHarm = Dice.Roll_d_20() + 40;

                if (weaponType.Equals(ItemUtils.Types.Bow) ||
                    weaponType.Equals(ItemUtils.Types.Crossbow))
                {
                    bonuses.ToRanged = Dice.Roll_d_20() + 40;
                }
                else
                {
                    bonuses.ToMelee = Dice.Roll_d_20() + 40;
                }

                if (weaponType.Equals(ItemUtils.Types.Sword))
                {
                    bonuses.ToHarm *= 4;
                }

                bonuses = Stats.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToHarm = Dice.Roll_d_20() + 80;

                if (weaponType.Equals(ItemUtils.Types.Bow) ||
                    weaponType.Equals(ItemUtils.Types.Crossbow))
                {
                    bonuses.ToRanged = Dice.Roll_d_20() + 80;
                }
                else
                {
                    bonuses.ToMelee = Dice.Roll_d_20() + 80;
                }

                if (weaponType.Equals(ItemUtils.Types.Sword))
                {
                    bonuses.ToHarm *= 8;
                }

                bonuses = Stats.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = Assets.ReturnRandomAssetIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else
            {
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private ItemBonuses ReturnPropsForApparatus(ItemBonuses bonuses, int level)
        {
            if (level == 1)
            {
                bonuses.ToHarm = Dice.Roll_d_20();
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToHarm = Dice.Roll_d_20();
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToHarm = Dice.Roll_d_20();
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = Stats.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToHarm = Dice.Roll_d_20();
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 80);
                bonuses = Stats.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = Assets.ReturnRandomAssetIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else
            {
                // TODO: this case covers Artifact and Relics
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private ItemBonuses ReturnPropsForValuables(ItemBonuses bonuses, int level)
        {
            if (level == 1)
            {
                bonuses.ToWealth = Dice.Roll_d_20();
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToWealth = Dice.Roll_d_20() +
                       Dice.Roll_d_20() + 10;
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToWealth = Dice.Roll_d_20() +
                        Dice.Roll_d_20() + 10 +
                        Dice.Roll_d_20() + 20;
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToWealth = Dice.Roll_d_20() +
                       Dice.Roll_d_20() + 10 +
                       Dice.Roll_d_20() + 20 +
                       Dice.Roll_d_20() + 30;
                return bonuses;
            }
            else
            {
                // TODO: this case covers Artifact and Relics
                // TODO: Artifact and Relics should have their own worth and prices
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);

            }
        }

        private ItemBonuses ReturnBonusesForArmour(ItemBonuses bonuses, int level)
        {
            if (level == 1)
            {
                bonuses.ToDRM = Dice.Roll_x_to_20(1) + 5;
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToDRM = Dice.Roll_x_to_20(1) + 10;
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToDRM = Dice.Roll_x_to_20(1) + 15;
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = Stats.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToDRM = Dice.Roll_x_to_20(1) + 20;
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 80);
                bonuses = Stats.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = Assets.ReturnRandomAssetIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else
            {
                // TODO: this case covers Artifact and Relics
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private ItemBonuses ReturnPropsForShield(ItemBonuses bonuses, int level)
        {
            if (level == 1)
            {
                bonuses.ToDRM = Dice.Roll_min_to_max(1, 6) + 5;
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToDRM = Dice.Roll_min_to_max(1, 6) + 10;
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToDRM = Dice.Roll_min_to_max(1, 6) + 15;
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = Stats.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToDRM = Dice.Roll_min_to_max(1, 6) + 20;
                bonuses = Skills.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 80);
                bonuses = Stats.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = Assets.ReturnRandomAssetIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else
            {
                // TODO: this case covers Artifact and Relics
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        protected int GenerateItemLevel()
        {
            var random = Dice.Roll_d_20();

            if (20 < random && random < 40) return 2;
            else if (40 < random && random < 60) return 3;
            else if (60 < random && random < 80) return 4;
            else if (80 < random && random < 100) return 5;
            else if (random > 100) return 6;
            else return 1;
        }

        private ItemUtils.Types GenerateItemType()
        {
            var index = Dice.Roll_min_to_max(1, 13);

            return (ItemUtils.Types)index;
        }

        private string GenerateItemNameByLevelAndType(int level, ItemUtils.Types type)
        {
            if (level == 1) return $"{ItemUtils.List_of_CommonNamePrefixes[Dice.Roll_0_to_max(4)]} {type}";
            else if (level == 2) return $"{ItemUtils.List_of_RefinedNamePrefixes[Dice.Roll_0_to_max(4)]} {type}";
            else if (level == 3) return $"{ItemUtils.List_of_MasterworkNamePrefixes[Dice.Roll_0_to_max(4)]} {type}";
            else if (level == 4) return $"{ItemUtils.itemNames.Heirloom} {type}";
            else return $"{ItemUtils.itemNames.ObjectFromAfar}";

            // levels 5 and 6 (Artifacts and Relics) will have their own names
        }
    }
}

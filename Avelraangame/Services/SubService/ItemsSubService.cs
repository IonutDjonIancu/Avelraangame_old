using Avelraangame.Models;
using Avelraangame.Models.ModelProps;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;

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
                InSlot = ItemUtils.inventorySlots.Supplies
            };

            item.Name = GenerateItemNameByLevelAndType(item.Level, item.Type);
            item.Worth = GenerateItemWorthByLevelAndType(item.Level, item.Type);
            item.IsConsumable = IsConsumable(item.Type);

            var props = GenerateItemPropertiesByLevelAndType(item.Level, item.Type); // properties

            if (props.Assets_toWealth > 0)
            {
                item.Worth += props.Assets_toWealth;
            }
            item.Properties = JsonConvert.SerializeObject(props);

            return item;
        }

        private bool IsConsumable(string type)
        {
            if (type == ItemUtils.itemTypes.Apparatus)
            {
                var random = Dice.Roll_min_to_max(1, 2);

                if (random == 1) return true;

                return false;
            }
            else if (type == ItemUtils.itemTypes.Valuables)
            {
                return true;
            }

            return false;
        }

        private int GenerateItemWorthByLevelAndType(int level, string type)
        {
            if (type == ItemUtils.itemTypes.Valuables) return 0;

            return level switch
            {
                1 => Dice.Roll_x_to_20(1) + 10,
                2 => Dice.Roll_x_to_20(2) + 20,
                3 => Dice.Roll_x_to_20(4) + 30,
                4 => Dice.Roll_x_to_20(8) + 40,
                _ => 0
            };
        }

        private ItemProperties GenerateItemPropertiesByLevelAndType(int level, string type)
        {
            var props = new ItemProperties();

            props = SetItemSlotsByType(props, type);

            if (type == ItemUtils.itemTypes.Armour) return ReturnPropsForArmour(props, level);
            else if (type == ItemUtils.itemTypes.Shield) return ReturnPropsForShield(props, level);
            else if (type == ItemUtils.itemTypes.Valuables) return ReturnPropsForValuables(props, level);
            else if (type == ItemUtils.itemTypes.Apparatus) return ReturnPropsForApparatus(props, level);
            else return ReturnPropsForWeapons(props, level, type);
        }

        private ItemProperties SetItemSlotsByType(ItemProperties props, string type)
        {
            if (type == ItemUtils.itemTypes.Apparatus)
            {
                props.Slots_MainHand = true;
                props.Slots_OffHand = true;
                props.Slots_Ranged = true;
                props.Slots_Armour = true;
                props.Slots_Trinkets = true;
                props.Slots_Supplies = true;

                return props;
            }
            else if (type == ItemUtils.itemTypes.Valuables)
            {
                props.Slots_MainHand = false;
                props.Slots_OffHand = false;
                props.Slots_Ranged = false;
                props.Slots_Armour = false;
                props.Slots_Trinkets = false;
                props.Slots_Supplies = true;

                return props;
            }
            else if (type == ItemUtils.itemTypes.Armour)
            {
                props.Slots_MainHand = false;
                props.Slots_OffHand = false;
                props.Slots_Ranged = false;
                props.Slots_Armour = true;
                props.Slots_Trinkets = false;
                props.Slots_Supplies = true;

                return props;
            }
            else if (type == ItemUtils.itemTypes.Shield)
            {
                props.Slots_MainHand = true;
                props.Slots_OffHand = true;
                props.Slots_Ranged = false;
                props.Slots_Armour = false;
                props.Slots_Trinkets = false;
                props.Slots_Supplies = true;

                return props;
            }
            else if (type == ItemUtils.itemTypes.Axe || type == ItemUtils.itemTypes.Spear)
            {
                props.Slots_MainHand = true;
                props.Slots_OffHand = true;
                props.Slots_Ranged = true;
                props.Slots_Armour = false;
                props.Slots_Trinkets = false;
                props.Slots_Supplies = true;

                return props;
            }
            else if (type == ItemUtils.itemTypes.Crossbow)
            {
                props.Slots_MainHand = true;
                props.Slots_OffHand = false;
                props.Slots_Ranged = true;
                props.Slots_Armour = false;
                props.Slots_Trinkets = false;
                props.Slots_Supplies = true;

                return props;
            }
            else if (type == ItemUtils.itemTypes.Bow)
            {
                props.Slots_MainHand = false;
                props.Slots_OffHand = false;
                props.Slots_Ranged = true;
                props.Slots_Armour = false;
                props.Slots_Trinkets = false;
                props.Slots_Supplies = true;

                return props;
            }
            else if (type == ItemUtils.itemTypes.Polearm)
            {
                props.Slots_MainHand = true;
                props.Slots_OffHand = false;
                props.Slots_Ranged = false;
                props.Slots_Armour = false;
                props.Slots_Trinkets = false;
                props.Slots_Supplies = true;

                return props;
            }
            else
            {
                {
                    props.Slots_MainHand = true;
                    props.Slots_OffHand = true;
                    props.Slots_Ranged = false;
                    props.Slots_Armour = false;
                    props.Slots_Trinkets = false;
                    props.Slots_Supplies = true;

                    return props;
                }
            }
        }

        private ItemProperties ReturnPropsForWeapons(ItemProperties props, int level, string weaponType)
        {
            if (level == 1)
            {
                props.Assets_toHarm = Dice.Roll_d_20();

                if (weaponType == ItemUtils.itemTypes.Sword)
                {
                    props.Assets_toHarm *= 2;
                }
                return props;
            }
            else if (level == 2)
            {
                props.Assets_toHarm = Dice.Roll_d_20();

                if (weaponType == ItemUtils.itemTypes.Bow ||
                    weaponType == ItemUtils.itemTypes.Crossbow)
                {
                    props.Skills_toRanged = Dice.Roll_d_20() + 20;
                }
                else
                {
                    props.Skills_toMelee = Dice.Roll_d_20() + 20;
                }

                if (weaponType == ItemUtils.itemTypes.Sword)
                {
                    props.Assets_toHarm *= 2;
                }
                return props;
            }
            else if (level == 3)
            {
                props.Assets_toHarm = Dice.Roll_d_20();

                if (weaponType == ItemUtils.itemTypes.Bow ||
                    weaponType == ItemUtils.itemTypes.Crossbow)
                {
                    props.Skills_toRanged = Dice.Roll_d_20() + 40;
                }
                else
                {
                    props.Skills_toMelee = Dice.Roll_d_20() + 40;
                }

                if (weaponType == ItemUtils.itemTypes.Sword)
                {
                    props.Assets_toHarm *= 4;
                }

                props = Stats.ReturnRandomStatIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else if (level == 4)
            {
                props.Assets_toHarm = Dice.Roll_d_20();

                if (weaponType == ItemUtils.itemTypes.Bow ||
                    weaponType == ItemUtils.itemTypes.Crossbow)
                {
                    props.Skills_toRanged = Dice.Roll_d_20() + 80;
                }
                else
                {
                    props.Skills_toMelee = Dice.Roll_d_20() + 80;
                }

                if (weaponType == ItemUtils.itemTypes.Sword)
                {
                    props.Assets_toHarm *= 8;
                }

                props = Stats.ReturnRandomStatIncreaseForItem(props, Dice.Roll_d_20() + 40);
                props = Assets.ReturnRandomAssetIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else
            {
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private ItemProperties ReturnPropsForApparatus(ItemProperties props, int level)
        {
            if (level == 1)
            {
                props.Assets_toHarm = Dice.Roll_d_20();
                return props;
            }
            else if (level == 2)
            {
                props.Assets_toHarm = Dice.Roll_d_20();
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else if (level == 3)
            {
                props.Assets_toHarm = Dice.Roll_d_20();
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 40);
                props = Stats.ReturnRandomStatIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else if (level == 4)
            {
                props.Assets_toHarm = Dice.Roll_d_20();
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 80);
                props = Stats.ReturnRandomStatIncreaseForItem(props, Dice.Roll_d_20() + 40);
                props = Assets.ReturnRandomAssetIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else
            {
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private ItemProperties ReturnPropsForValuables(ItemProperties props, int level)
        {
            if (level == 1)
            {
                props.Assets_toWealth = Dice.Roll_d_20();
                return props;
            }
            else if (level == 2)
            {
                props.Assets_toWealth = Dice.Roll_d_20() +
                       Dice.Roll_d_20() + 10;
                return props;
            }
            else if (level == 3)
            {
                props.Assets_toWealth = Dice.Roll_d_20() +
                        Dice.Roll_d_20() + 10 +
                        Dice.Roll_d_20() + 20;
                return props;
            }
            else if (level == 4)
            {
                props.Assets_toWealth = Dice.Roll_d_20() +
                       Dice.Roll_d_20() + 10 +
                       Dice.Roll_d_20() + 20 +
                       Dice.Roll_d_20() + 30;
                return props;
            }
            else
            {
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);

            }
        }

        private ItemProperties ReturnPropsForArmour(ItemProperties props, int level)
        {
            if (level == 1)
            {
                props.Assets_toDRM = Dice.Roll_x_to_20(1) + 5;
                return props;
            }
            else if (level == 2)
            {
                props.Assets_toDRM = Dice.Roll_x_to_20(1) + 10;
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else if (level == 3)
            {
                props.Assets_toDRM = Dice.Roll_x_to_20(1) + 15;
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 40);
                props = Stats.ReturnRandomStatIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else if (level == 4)
            {
                props.Assets_toDRM = Dice.Roll_x_to_20(1) + 20;
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 80);
                props = Stats.ReturnRandomStatIncreaseForItem(props, Dice.Roll_d_20() + 40);
                props = Assets.ReturnRandomAssetIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else
            {
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private ItemProperties ReturnPropsForShield(ItemProperties props, int level)
        {
            if (level == 1)
            {
                props.Assets_toDRM = Dice.Roll_min_to_max(1, 6) + 5;
                return props;
            }
            else if (level == 2)
            {
                props.Assets_toDRM = Dice.Roll_min_to_max(1, 6) + 10;
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else if (level == 3)
            {
                props.Assets_toDRM = Dice.Roll_min_to_max(1, 6) + 15;
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 40);
                props = Stats.ReturnRandomStatIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else if (level == 4)
            {
                props.Assets_toDRM = Dice.Roll_min_to_max(1, 6) + 20;
                props = Skills.ReturnRandomSkillIncreaseForItem(props, Dice.Roll_d_20() + 80);
                props = Stats.ReturnRandomStatIncreaseForItem(props, Dice.Roll_d_20() + 40);
                props = Assets.ReturnRandomAssetIncreaseForItem(props, Dice.Roll_d_20() + 20);
                return props;
            }
            else
            {
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

        private string GenerateItemType()
        {
            var index = Dice.Roll_min_to_max(0, 12);

            return ItemUtils.List_of_ItemTypes[index];
        }

        private string GenerateItemNameByLevelAndType(int level, string type)
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

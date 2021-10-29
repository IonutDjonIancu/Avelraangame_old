using Avelraangame.Models;
using Avelraangame.Models.POCOs;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Avelraangame.Services.SubService
{
    public class ItemsSubService : ServiceBase
    {
        protected Item GenerateNormalArmour(int itemLevel)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Level = itemLevel,
                Type = ItemsUtils.Types.Armour,
                IsEquipped = true,
                InSlot = ItemsUtils.Slots.Armour
            };

            item.Name = GenerateItemNameByLevelAndType(itemLevel, ItemsUtils.Types.Armour, false);
            item.Worth = GenerateItemWorthByLevelAndType(itemLevel, ItemsUtils.Types.Armour);
            item.IsConsumable = false;
            item.Slots = JsonConvert.SerializeObject(GenerateItemSlotsByType(item.Type));

            var bonuses = GenerateItemBonusesByLevelAndType(itemLevel, ItemsUtils.Types.Armour);

            if (bonuses.ToWealth > 0)
            {
                item.Worth += bonuses.ToWealth;
            }

            item.Bonuses = JsonConvert.SerializeObject(bonuses);


            return item;
        }

        protected Item GenerateNormalMainHand(int itemLevel)
        {
            var type = GetRandomItemWeaponType();

            var item = new Item
            {
                Id = Guid.NewGuid(),
                Level = itemLevel,
                Type = type,
                IsEquipped = true,
                InSlot = ItemsUtils.Slots.Mainhand
            };

            item.Name = GenerateItemNameByLevelAndType(itemLevel, type, false);
            item.Worth = GenerateItemWorthByLevelAndType(itemLevel, type);
            item.IsConsumable = false;
            item.Slots = JsonConvert.SerializeObject(GenerateItemSlotsByType(item.Type));

            var bonuses = GenerateItemBonusesByLevelAndType(itemLevel, type);

            if (bonuses.ToWealth > 0)
            {
                item.Worth += bonuses.ToWealth;
            }

            item.Bonuses = JsonConvert.SerializeObject(bonuses);

            return item;
        }

        protected Item GenerateNormalOffHand(int itemLevel)
        {
            var type = GetRandomItemWeaponType();

            var item = new Item
            {
                Id = Guid.NewGuid(),
                Level = itemLevel,
                Type = type,
                IsEquipped = true,
                InSlot = ItemsUtils.Slots.Offhand
            };

            item.Name = GenerateItemNameByLevelAndType(itemLevel, type, false);
            item.Worth = GenerateItemWorthByLevelAndType(itemLevel, type);
            item.IsConsumable = false;
            item.Slots = JsonConvert.SerializeObject(GenerateItemSlotsByType(item.Type));

            var bonuses = GenerateItemBonusesByLevelAndType(itemLevel, type);

            if (bonuses.ToWealth > 0)
            {
                item.Worth += bonuses.ToWealth;
            }

            item.Bonuses = JsonConvert.SerializeObject(bonuses);

            return item;
        }

        protected Item GenerateNormalRanged(int itemLevel)
        {
            var type = GetRandomItemRangedType();

            var item = new Item
            {
                Id = Guid.NewGuid(),
                Level = itemLevel,
                Type = type,
                IsEquipped = true,
                InSlot = ItemsUtils.Slots.Ranged
            };

            item.Name = GenerateItemNameByLevelAndType(itemLevel, type, false);
            item.Worth = GenerateItemWorthByLevelAndType(itemLevel, type);
            item.IsConsumable = false;
            item.Slots = JsonConvert.SerializeObject(GenerateItemSlotsByType(item.Type));

            var bonuses = GenerateItemBonusesByLevelAndType(itemLevel, type);

            if (bonuses.ToWealth > 0)
            {
                item.Worth += bonuses.ToWealth;
            }

            item.Bonuses = JsonConvert.SerializeObject(bonuses);

            return item;
        }

        protected Item GenerateNormalTrinket(int itemLevel)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Level = itemLevel,
                Type = ItemsUtils.Types.Apparatus,
                IsEquipped = true,
                InSlot = ItemsUtils.Slots.Trinkets
            };

            item.Name = GenerateItemNameByLevelAndType(itemLevel, ItemsUtils.Types.Apparatus, false);
            item.Worth = GenerateItemWorthByLevelAndType(itemLevel, ItemsUtils.Types.Apparatus);
            item.IsConsumable = false;
            item.Slots = JsonConvert.SerializeObject(GenerateItemSlotsByType(item.Type));

            var bonuses = GenerateItemBonusesByLevelAndType(itemLevel, ItemsUtils.Types.Apparatus);

            if (bonuses.ToWealth > 0)
            {
                item.Worth += bonuses.ToWealth;
            }

            item.Bonuses = JsonConvert.SerializeObject(bonuses);

            return item;
        }

        protected Item GenerateNormalItem(int itemLevel, string charId = null)
        {
            var characterId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(charId))
            {
                characterId = Guid.Parse(charId);
            }

            var item = new Item
            {
                Id = Guid.NewGuid(),
                CharacterId = characterId,
                Level = itemLevel,
                Type = GenerateItemType(),
                IsEquipped = false,
                InSlot = ItemsUtils.Slots.Supplies,
            };

            item.Slots = JsonConvert.SerializeObject(GenerateItemSlotsByType(item.Type));
            item.IsConsumable = IsConsumable(item.Type);
            item.Name = GenerateItemNameByLevelAndType(item.Level, item.Type, item.IsConsumable);
            item.Worth = GenerateItemWorthByLevelAndType(item.Level, item.Type);

            var bonuses = GenerateItemBonusesByLevelAndType(item.Level, item.Type); // bonuses

            if (bonuses.ToWealth > 0)
            {
                item.Worth += bonuses.ToWealth;
            }

            item.Bonuses = JsonConvert.SerializeObject(bonuses);

            return item;
        }

        private List<ItemsUtils.Slots> GenerateItemSlotsByType(ItemsUtils.Types type)
        {
            if (type.Equals(ItemsUtils.itemTypes.Apparatus))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Mainhand,
                        ItemsUtils.Slots.Offhand,
                        ItemsUtils.Slots.Ranged,
                        ItemsUtils.Slots.Armour,
                        ItemsUtils.Slots.Trinkets,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemsUtils.itemTypes.Armour))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Armour,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemsUtils.itemTypes.Axe))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Mainhand,
                        ItemsUtils.Slots.Offhand,
                        ItemsUtils.Slots.Ranged,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemsUtils.itemTypes.Bow))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Ranged,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemsUtils.itemTypes.Crossbow))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Mainhand,
                        ItemsUtils.Slots.Ranged,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemsUtils.itemTypes.Polearm))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Mainhand,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemsUtils.itemTypes.Shield))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Mainhand,
                        ItemsUtils.Slots.Offhand,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemsUtils.itemTypes.Spear))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Mainhand,
                        ItemsUtils.Slots.Offhand,
                        ItemsUtils.Slots.Ranged,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else if (type.Equals(ItemsUtils.itemTypes.Valuables))
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
            else
            {
                var returnList = new List<ItemsUtils.Slots>()
                    {
                        ItemsUtils.Slots.Mainhand,
                        ItemsUtils.Slots.Offhand,
                        ItemsUtils.Slots.Supplies
                    };
                return returnList;
            }
        }

        private bool IsConsumable(ItemsUtils.Types type)
        {
            if (type.Equals(ItemsUtils.Types.Apparatus))
            {
                var random = Dice.Roll_min_to_max(1, 2);

                if (random == 1) return true;

                return false;
            }
            else if (type.Equals(ItemsUtils.Types.Valuables))
            {
                return true;
            }

            return false;
        }

        private int GenerateItemWorthByLevelAndType(int level, ItemsUtils.Types type)
        {
            if (type.Equals(ItemsUtils.Types.Valuables)) return 0;

            return level switch
            {
                1 => Dice.Roll_x_to_20(1) + 10,
                2 => Dice.Roll_x_to_20(2) + 20,
                3 => Dice.Roll_x_to_20(4) + 30,
                4 => Dice.Roll_x_to_20(8) + 40,
                _ => 0
            };
        }

        private Bonuses GenerateItemBonusesByLevelAndType(int level, ItemsUtils.Types type)
        {
            var bonuses = new Bonuses();

            if (type.Equals(ItemsUtils.Types.Armour)) return ReturnBonusesForArmour(bonuses, level);
            else if (type.Equals(ItemsUtils.Types.Shield)) return ReturnPropsForShield(bonuses, level);
            else if (type.Equals(ItemsUtils.Types.Valuables)) return ReturnPropsForValuables(bonuses, level);
            else if (type.Equals(ItemsUtils.Types.Apparatus)) return ReturnPropsForApparatus(bonuses, level);
            else return ReturnPropsForWeapons(bonuses, level, type);
        }


        private Bonuses ReturnPropsForWeapons(Bonuses bonuses, int level, ItemsUtils.Types weaponType)
        {
            var statsSubs = new StatsSubService();
            var assetsSubs = new AssetsSubService();

            if (level == 1)
            {
                bonuses.ToHarm = Dice.Roll_d_20() + 10;

                if (weaponType.Equals(ItemsUtils.Types.Sword))
                {
                    bonuses.ToHarm *= 2;
                }
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToHarm = Dice.Roll_d_20() + 20;

                if (weaponType.Equals(ItemsUtils.Types.Bow) ||
                    weaponType.Equals(ItemsUtils.Types.Crossbow))
                {
                    bonuses.ToRanged = Dice.Roll_d_20() + 20;
                }
                else
                {
                    bonuses.ToMelee = Dice.Roll_d_20() + 20;
                }

                if (weaponType.Equals(ItemsUtils.Types.Sword))
                {
                    bonuses.ToHarm *= 2;
                }
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToHarm = Dice.Roll_d_20() + 40;

                if (weaponType.Equals(ItemsUtils.Types.Bow) ||
                    weaponType.Equals(ItemsUtils.Types.Crossbow))
                {
                    bonuses.ToRanged = Dice.Roll_d_20() + 40;
                }
                else
                {
                    bonuses.ToMelee = Dice.Roll_d_20() + 40;
                }

                if (weaponType.Equals(ItemsUtils.Types.Sword))
                {
                    bonuses.ToHarm *= 4;
                }

                bonuses = statsSubs.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToHarm = Dice.Roll_d_20() + 80;

                if (weaponType.Equals(ItemsUtils.Types.Bow) ||
                    weaponType.Equals(ItemsUtils.Types.Crossbow))
                {
                    bonuses.ToRanged = Dice.Roll_d_20() + 80;
                }
                else
                {
                    bonuses.ToMelee = Dice.Roll_d_20() + 80;
                }

                if (weaponType.Equals(ItemsUtils.Types.Sword))
                {
                    bonuses.ToHarm *= 8;
                }

                bonuses = statsSubs.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = assetsSubs.ReturnRandomAssetIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else
            {
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private Bonuses ReturnPropsForApparatus(Bonuses bonuses, int level)
        {
            var statsSubs = new StatsSubService();
            var assetsSubs = new AssetsSubService();
            var skillsSubs = new SkillsSubService();

            if (level == 1)
            {
                bonuses.ToHarm = Dice.Roll_d_20();
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToHarm = Dice.Roll_d_20();
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToHarm = Dice.Roll_d_20();
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = statsSubs.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToHarm = Dice.Roll_d_20();
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 80);
                bonuses = statsSubs.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = assetsSubs.ReturnRandomAssetIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else
            {
                // TODO: this case covers Artifact and Relics
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private Bonuses ReturnPropsForValuables(Bonuses bonuses, int level)
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

        private Bonuses ReturnBonusesForArmour(Bonuses bonuses, int level)
        {
            var statsSubs = new StatsSubService();
            var assetsSubs = new AssetsSubService();
            var skillsSubs = new SkillsSubService();

            if (level == 1)
            {
                bonuses.ToDRM = Dice.Roll_x_to_20(1) + 5;
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToDRM = Dice.Roll_x_to_20(1) + 10;
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToDRM = Dice.Roll_x_to_20(1) + 15;
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = statsSubs.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToDRM = Dice.Roll_x_to_20(1) + 20;
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 80);
                bonuses = statsSubs.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = assetsSubs.ReturnRandomAssetIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else
            {
                // TODO: this case covers Artifact and Relics
                throw new Exception(message: Scribe.Error_IfElseIf_notCoveredCase);
            }
        }

        private Bonuses ReturnPropsForShield(Bonuses bonuses, int level)
        {
            var statsSubs = new StatsSubService();
            var assetsSubs = new AssetsSubService();
            var skillsSubs = new SkillsSubService();

            if (level == 1)
            {
                bonuses.ToDRM = Dice.Roll_min_to_max(1, 6) + 5;
                return bonuses;
            }
            else if (level == 2)
            {
                bonuses.ToDRM = Dice.Roll_min_to_max(1, 6) + 10;
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 3)
            {
                bonuses.ToDRM = Dice.Roll_min_to_max(1, 6) + 15;
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = statsSubs.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
                return bonuses;
            }
            else if (level == 4)
            {
                bonuses.ToDRM = Dice.Roll_min_to_max(1, 6) + 20;
                bonuses = skillsSubs.ReturnRandomSkillIncreaseForItem(bonuses, Dice.Roll_d_20() + 80);
                bonuses = statsSubs.ReturnRandomStatIncreaseForItem(bonuses, Dice.Roll_d_20() + 40);
                bonuses = assetsSubs.ReturnRandomAssetIncreaseForItem(bonuses, Dice.Roll_d_20() + 20);
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

        private ItemsUtils.Types GenerateItemType()
        {
            var index = Dice.Roll_min_to_max(1, 13);

            return (ItemsUtils.Types)index;
        }

        private string GenerateItemNameByLevelAndType(int level, ItemsUtils.Types type, bool isConsumable)
        {
            if (type.Equals(ItemsUtils.Types.Apparatus) && isConsumable)
            {
                return GeneratePotion(level);
            }

            if (level == 1) return $"{ItemsUtils.List_of_CommonNamePrefixes[Dice.Roll_0_to_max(4)]} {type}";
            else if (level == 2) return $"{ItemsUtils.List_of_RefinedNamePrefixes[Dice.Roll_0_to_max(4)]} {type}";
            else if (level == 3) return $"{ItemsUtils.List_of_MasterworkNamePrefixes[Dice.Roll_0_to_max(4)]} {type}";
            else if (level == 4) return $"{ItemsUtils.itemNames.Heirloom} {type}";
            else return $"{ItemsUtils.itemNames.ObjectFromAfar}";

            // levels 5 and 6 (Artifacts and Relics) will have their own names
        }

        private ItemsUtils.Types GetRandomItemWeaponType()
        {
            var roll = Dice.Roll_min_to_max(1, 8);
            return roll switch
            {
                1 => ItemsUtils.Types.Axe,
                2 => ItemsUtils.Types.Club,
                3 => ItemsUtils.Types.Mace,
                4 => ItemsUtils.Types.Polearm,
                5 => ItemsUtils.Types.Shield,
                6 => ItemsUtils.Types.Spear,
                7 => ItemsUtils.Types.Warhammer,
                _ => ItemsUtils.Types.Sword,
            };
        }

        private ItemsUtils.Types GetRandomItemRangedType()
        {
            var roll = Dice.Roll_min_to_max(1, 3);
            return roll switch
            {
                1 => ItemsUtils.Types.Bow,
                2 => ItemsUtils.Types.Crossbow,
                _ => ItemsUtils.Types.Spear,
            };
        }

        private string GeneratePotion(int level)
        {
            if (level == 1)
            {
                return "Viscous mixture";
            }
            else if (level == 2)
            {
                return "Decanted blend";
            }
            else if (level == 3)
            {
                return "Purified melange";
            }
            else if (level == 4)
            {
                return "Alchemical potion";
            }
            else
            {
                return $"{ItemsUtils.itemNames.ObjectFromAfar}";
            }
        }
    }
}

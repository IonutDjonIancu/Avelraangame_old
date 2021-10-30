using Avelraangame.Models;
using Avelraangame.Models.POCOs;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avelraangame.Services
{
    public class ItemsService : ItemsSubService
    {
        #region Business logic

        public string SellItem(RequestVm request)
        {
            var itemVm = ValidateRequestDeserializationIntoItemVm(request.Message);
            ValidateItemByCharacterId(itemVm.Id, itemVm.CharacterId.GetValueOrDefault());

            var character = DataService.GetCharacterById(itemVm.CharacterId.GetValueOrDefault());
            var item = DataService.GetItemById(itemVm.Id);

            var supplies = JsonConvert.DeserializeObject<List<ItemVm>>(character.Supplies);
            var itemVmToRemove = supplies.Where(s => s.Id.Equals(item.Id)).FirstOrDefault();
            supplies.Remove(itemVmToRemove);
            character.Supplies = JsonConvert.SerializeObject(supplies);

            var logbook = JsonConvert.DeserializeObject<Logbook>(character.Logbook);
            logbook.Wealth += item.Worth;
            character.Logbook = JsonConvert.SerializeObject(logbook);
            DataService.UpdateCharacter(character);
            DataService.DeleteItem(item);

            return string.Concat(Scribe.ShortMessages.Success, $": item sold for {item.Worth}");
        }

        public Character EquipItemToSlot(Character chr, Item item)
        {
            var supps = new List<ItemVm>();
            var equipp = new Equipment();

            if (chr.Supplies != null)
            {
                supps = JsonConvert.DeserializeObject<List<ItemVm>>(chr.Supplies);
            }

            if (chr.Equippment != null)
            {
                equipp = JsonConvert.DeserializeObject<Equipment>(chr.Equippment);
            }

            if (item.Type.Equals(ItemsUtils.Types.Apparatus))
            {
                var itm = supps.Where(s => s.Id.Equals(item.Id)).FirstOrDefault();
                equipp.Trinkets.Add(itm);
                supps.Remove(itm);
            }
            else if (item.Type.Equals(ItemsUtils.Types.Armour))
            {
                var itm = supps.Where(s => s.Id.Equals(item.Id)).FirstOrDefault();

                if (equipp.Armour != null)
                {
                    supps.Add(equipp.Armour);
                }

                equipp.Armour = itm;
                supps.Remove(itm);
            }
            else if (item.Type.Equals(ItemsUtils.Types.Bow) || item.Type.Equals(ItemsUtils.Types.Crossbow))
            {
                var itm = supps.Where(s => s.Id.Equals(item.Id)).FirstOrDefault();

                if (equipp.Ranged != null)
                {
                    supps.Add(equipp.Ranged);
                }

                equipp.Ranged = itm;
                supps.Remove(itm);
            }
            else if (item.Type.Equals(ItemsUtils.Types.Shield))
            {
                var itm = supps.Where(s => s.Id.Equals(item.Id)).FirstOrDefault();

                if (equipp.Offhand != null)
                {
                    supps.Add(equipp.Offhand);
                }

                equipp.Offhand = itm;
                supps.Remove(itm);
            }
            else
            {
                var itm = supps.Where(s => s.Id.Equals(item.Id)).FirstOrDefault();

                if (equipp.Mainhand != null)
                {
                    supps.Add(equipp.Mainhand);
                }

                equipp.Mainhand = itm;
                supps.Remove(itm);
            }

            chr.Equippment = JsonConvert.SerializeObject(equipp);
            chr.Supplies = JsonConvert.SerializeObject(supps);

            return chr;
        }

        public ItemsUtils.Slots MoveItemInSlot(ItemsUtils.Types type)
        {
            return type switch
            {
                ItemsUtils.Types.Apparatus  => ItemsUtils.Slots.Trinkets,
                ItemsUtils.Types.Armour     => ItemsUtils.Slots.Armour,
                ItemsUtils.Types.Shield     => ItemsUtils.Slots.Offhand,
                ItemsUtils.Types.Bow        => ItemsUtils.Slots.Ranged,
                ItemsUtils.Types.Crossbow   => ItemsUtils.Slots.Ranged,
                ItemsUtils.Types.Valuables  => ItemsUtils.Slots.Supplies,
                _ => ItemsUtils.Slots.Mainhand
            };
        }

        public ItemVm GenerateRandomItem(string charId = null)
        {
            var itemLevel = GenerateItemLevel();
            Item item;

            if (itemLevel == 5)
            {
                //return GenerateArtifactItem(); // <------ should return ArtifactVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else if (itemLevel == 6)
            {
                //return GenerateRelicItem(); // <------ should return RelicVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else
            {
                item = GenerateNormalItem(itemLevel, charId);
            }

            DataService.CreateItem(item);
            var itemVm = new ItemVm(item);

            return itemVm;
        }

        public ItemVm GenerateRandomArmour(Guid? charId = null)
        {
            var itemLevel = GenerateItemLevel();
            Item item;

            if (itemLevel == 5)
            {
                //return GenerateArtifactItem(); // <------ should return ArtifactVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else if (itemLevel == 6)
            {
                //return GenerateRelicItem(); // <------ should return RelicVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else
            {
                item = GenerateNormalArmour(itemLevel);
            }

            if (charId != null)
            {
                item.CharacterId = charId;
                DataService.CreateItem(item);
            }

            var itemVm = new ItemVm(item);

            return itemVm;
        }

        public ItemVm GenerateRandomMainHandWeapon(Guid? charId = null)
        {
            var itemLevel = GenerateItemLevel();
            Item item;

            if (itemLevel == 5)
            {
                //return GenerateArtifactItem(); // <------ should return ArtifactVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else if (itemLevel == 6)
            {
                //return GenerateRelicItem(); // <------ should return RelicVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else
            {
                item = GenerateNormalMainHand(itemLevel);
            }

            if (charId != null)
            {
                item.CharacterId = charId;
                DataService.CreateItem(item);
            }

            var itemVm = new ItemVm(item);

            return itemVm;
        }

        public ItemVm GenerateRandomOffHandWeapon(Guid? charId = null)
        {
            var itemLevel = GenerateItemLevel();
            Item item;

            if (itemLevel == 5)
            {
                //return GenerateArtifactItem(); // <------ should return ArtifactVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else if (itemLevel == 6)
            {
                //return GenerateRelicItem(); // <------ should return RelicVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else
            {
                item = GenerateNormalOffHand(itemLevel);
            }

            if (charId != null)
            {
                item.CharacterId = charId;
                DataService.CreateItem(item);
            }

            var itemVm = new ItemVm(item);

            return itemVm;
        }

        public ItemVm GenerateRandomRangedWeapon(Guid? charId = null)
        {
            var itemLevel = GenerateItemLevel();
            Item item;

            if (itemLevel == 5)
            {
                //return GenerateArtifactItem(); // <------ should return ArtifactVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else if (itemLevel == 6)
            {
                //return GenerateRelicItem(); // <------ should return RelicVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else
            {
                item = GenerateNormalRanged(itemLevel);
            }

            if (charId != null)
            {
                item.CharacterId = charId;
                DataService.CreateItem(item);
            }

            var itemVm = new ItemVm(item);

            return itemVm;
        }

        public List<ItemVm> GenerateRandomTrinketsStash(Guid? charId = null)
        {
            var itemLevel = GenerateItemLevel();
            Item item;
            var trinkets = new List<ItemVm>();

            if (itemLevel == 5)
            {
                //return GenerateArtifactItem(); // <------ should return ArtifactVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else if (itemLevel == 6)
            {
                //return GenerateRelicItem(); // <------ should return RelicVm
                throw new NotImplementedException(message: $"{Scribe.ShortMessages.Failure}: not implemented exception");
            }
            else
            {
                var roll = Dice.Roll_min_to_max(1, 12);
                for (int i = 0; i < roll; i++)
                {
                    item = GenerateNormalTrinket(itemLevel);
                    if (charId != null)
                    {
                        item.CharacterId = charId;
                        DataService.CreateItem(item);
                    }

                    var itemVm = new ItemVm(item);
                    trinkets.Add(itemVm);
                }
            }

            return trinkets;
        }

        #endregion

        #region Public getters
        public ItemsUtils.Types GetItemType(string itemType)
        {
            switch (itemType)
            {
                case Scribe.ItemTypes_Apparatus:
                    return ItemsUtils.Types.Apparatus;
                case Scribe.ItemTypes_Armour:
                    return ItemsUtils.Types.Armour;
                case Scribe.ItemTypes_Axe:
                    return ItemsUtils.Types.Axe;
                case Scribe.ItemTypes_Bow:
                    return ItemsUtils.Types.Bow;
                case Scribe.ItemTypes_Club:
                    return ItemsUtils.Types.Club;
                case Scribe.ItemTypes_Crossbow:
                    return ItemsUtils.Types.Crossbow;
                case Scribe.ItemTypes_Mace:
                    return ItemsUtils.Types.Mace;
                case Scribe.ItemTypes_Polearm:
                    return ItemsUtils.Types.Polearm;
                case Scribe.ItemTypes_Shield:
                    return ItemsUtils.Types.Shield;
                case Scribe.ItemTypes_Spear:
                    return ItemsUtils.Types.Spear;
                case Scribe.ItemTypes_Sword:
                    return ItemsUtils.Types.Sword;
                case Scribe.ItemTypes_Valuables:
                    return ItemsUtils.Types.Valuables;
                case Scribe.ItemTypes_Warhammer:
                    return ItemsUtils.Types.Warhammer;
                default:
                    throw new NotImplementedException();
            }
        }

        public List<ItemsUtils.Slots> GetItemSlotsByType(ItemsUtils.Types type)
        {
            return GenerateItemSlotsByType(type);
        }

        public string GetItemsByCharacter(RequestVm request)
        {
            var reqCharVm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            var chr = ValidateCharacterByPlayerId(reqCharVm.CharacterId, reqCharVm.PlayerId);

            return chr.Supplies;
        }

        public Item GetItemById(Guid itemId)
        {
            return DataService.GetItemById(itemId);
        }

        public int GetItemsCount()
        {
            return DataService.GetItemsCount();
        }

        public List<ItemVm> GetSuppliesItemsByCharacterId(Guid charId)
        {
            ValidateCharacterId(charId);

            var itemsList = DataService.GetSuppliesItemsByCharacterId(charId);
            var returnList = new List<ItemVm>();

            foreach (var item in itemsList)
            {
                var itemVm = new ItemVm(item);
                returnList.Add(itemVm);
            }

            return returnList;
        }
        #endregion

    }
}

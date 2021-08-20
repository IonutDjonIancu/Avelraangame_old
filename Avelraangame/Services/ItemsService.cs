using Avelraangame.Models;
using Avelraangame.Models.ModelScraps;
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
            var equipp = new Equippment();

            if (chr.Supplies != null)
            {
                supps = JsonConvert.DeserializeObject<List<ItemVm>>(chr.Supplies);
            }

            if (chr.Equippment != null)
            {
                equipp = JsonConvert.DeserializeObject<Equippment>(chr.Equippment);
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

        public ItemVm GenerateRandomArmour(Guid? charId)
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

            item.CharacterId = charId;
            DataService.CreateItem(item);

            var itemVm = new ItemVm(item);

            return itemVm;
        }
        #endregion

        #region Public getters
        public string GetItemsByCharacter(RequestVm request)
        {
            var reqCharVm = ValidateRequestDeserializationIntoCharacterVm(request.Message);
            var chr = ValidateCharacterByPlayerId(reqCharVm.CharacterId, reqCharVm.PlayerId);

            return chr.Supplies;
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

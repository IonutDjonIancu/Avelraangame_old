using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Avelraangame.Services
{
    public class ItemsService : ItemsSubService
    {
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

            DataService.SaveItem(item);
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
            DataService.SaveItem(item);

            var itemVm = new ItemVm(item);

            return itemVm;
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

    }
}

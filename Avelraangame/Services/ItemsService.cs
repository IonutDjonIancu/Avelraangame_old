using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;

namespace Avelraangame.Services
{
    public class ItemsService : ItemsSubService
    {
        public string GenerateRandomItem()
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
                item = GenerateNormalItem(itemLevel);
            }

            DataService.SaveItem(item);
            var itemVm = new ItemVm(item);

            return JsonConvert.SerializeObject(itemVm);
        }

        public int GetItemsCount()
        {
            return DataService.GetItemsCount();
        }

    }
}

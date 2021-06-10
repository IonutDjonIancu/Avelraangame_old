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
        private DataService DataService { get; set; }

        public ItemsService()
        {
            DataService = new DataService();
        }

        public string GenerateRandomItem()
        {
            var itemLevel = GenerateItemLevel();
            var response = string.Empty;
            Item item;

            if (itemLevel == 5)
            {
                //return GenerateArtifactItem(); // <------ should return ArtifactVm
                throw new NotImplementedException();
            }
            else if (itemLevel == 6)
            {
                //return GenerateRelicItem(); // <------ should return RelicVm
                throw new NotImplementedException();
            }
            else
            {
                try
                {
                    item = GenerateNormalItem(itemLevel);
                }
                catch (Exception ex)
                {
                    response = string.Concat(Scribe.ShortMessages.Failure, ex);
                    return response;
                }
            }

            DataService.SaveItem(item);
            var itemVm = new ItemVm(item);

            response = JsonConvert.SerializeObject(itemVm);
            return response;
        }

        public List<ItemVm> GetItems()
        {
            var allItems = GetAllItems();
            var returnList = new List<ItemVm>();

            foreach (var item in allItems)
            {
                var i = new ItemVm(item);
                returnList.Add(i);
            }

            return returnList;
        }

    }
}

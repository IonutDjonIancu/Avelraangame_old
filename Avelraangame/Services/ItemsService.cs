using Avelraangame.Data;
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
        private AvelraanContext Context { get; set; }

        public ItemsService()
        {
            Context = new AvelraanContext();
        }

        public RequestVm GenerateRandomItem()
        {
            var request = new RequestVm();
            var itemVm = new ItemVm();

            var itemLevel = GenerateItemLevel();

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
                    var item = GenerateNormalItem(itemLevel);
                    itemVm.Convert(item);
                }
                catch (Exception ex)
                {
                    request.OperationSuccess = false;
                    request.Message = ex.Message;
                    return request;
                }

            }

            request.Response = JsonConvert.SerializeObject(itemVm);

            return request;
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

        public void CreateItem()
        {
            var itemLevel = GenerateItemLevel();
            var item = GenerateNormalItem(itemLevel);

            Context.Items.Add(item); // should cross through Validators

            Context.SaveChanges();
        }


    }
}

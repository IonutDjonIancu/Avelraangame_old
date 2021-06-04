using Avelraangame.Data;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceBase;
using System;
using System.Collections.Generic;

namespace Avelraangame.Services
{
    public class ItemsService : ItemsServiceBase
    {
        private AvelraanContext Context { get; set; }

        public ItemsService()
        {
            Context = new AvelraanContext();
        }

        public ItemVm GenerateRandomItem()
        {
            var itemLevel = GenerateItemLevel();

            if (itemLevel == 5)
            {
                //return GenerateArtifactItem(); // <------ should return a Vm
                throw new NotImplementedException();
            }
            else if (itemLevel == 6)
            {
                //return GenerateRelicItem(); // <------ should return a Vm
                throw new NotImplementedException();
            }
            else
            {
                var item = GenerateNormalItem(itemLevel);
                var itemVm = new ItemVm(item);

                return itemVm;
            }
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

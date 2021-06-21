using Avelraangame.Models.ModelScraps;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avelraangame.Models.ViewModels
{
    public class ItemVm
    {
        public Guid Id { get; set; }

        public Guid? CharacterId { get; set; }

        public string Name { get; set; }

        public ItemUtils.Types Type { get; set; }

        public int Level { get; set; }

        public bool IsEquipped { get; set; }

        public int Worth { get; set; }

        public ItemUtils.Slots InSlot { get; set; }
        public List<ItemUtils.Slots> Slots { get; set; }

        public ItemBonuses Bonuses { get; set; }

        public bool IsConsumable { get; set; }

        public ItemVm()
        {

        }

        public ItemVm(Item item)
        {
            Id = item.Id;
            CharacterId = item.CharacterId;
            Name = item.Name;
            Type = item.Type;
            Level = item.Level;
            IsEquipped = item.IsEquipped;
            Worth = item.Worth;
            InSlot = item.InSlot;
            Slots = ConvertItemSlots(item.Slots);
            Bonuses = JsonConvert.DeserializeObject<ItemBonuses>(item.Bonuses);
            IsConsumable = item.IsConsumable;
        }

        private List<ItemUtils.Slots> ConvertItemSlots(string slots)
        {
            List<string> stringsInSlots = slots.Split(",").ToList();

            var myList = new List<ItemUtils.Slots>();

            foreach (var item in stringsInSlots)
            {
                var a = int.Parse(item);
                myList.Add((ItemUtils.Slots)a);
            }

            return myList;
        }

    }
}

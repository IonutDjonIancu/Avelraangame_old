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
        public Guid? PlayerId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Level { get; set; }

        public bool IsEquipped { get; set; }

        public int Worth { get; set; }

        public ItemsUtils.Slots InSlot { get; set; }
        public List<ItemsUtils.Slots> Slots { get; set; }

        public Bonuses Bonuses { get; set; }

        public bool IsConsumable { get; set; }

        public ItemVm()
        {

        }

        public ItemVm(Item item)
        {
            Id = item.Id;
            CharacterId = item.CharacterId;
            Name = item.Name;
            Type = item.Type.ToString();
            Level = item.Level;
            IsEquipped = item.IsEquipped;
            Worth = item.Worth;
            InSlot = item.InSlot;
            Slots = JsonConvert.DeserializeObject<List<ItemsUtils.Slots>>(item.Slots);
            Bonuses = JsonConvert.DeserializeObject<Bonuses>(item.Bonuses);
            IsConsumable = item.IsConsumable;
        }

    }
}

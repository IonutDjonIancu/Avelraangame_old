using Avelraangame.Models.ModelProps;
using Newtonsoft.Json;
using System;

namespace Avelraangame.Models.ViewModels
{
    public class ItemVm
    {
        public Guid Id { get; set; }

        public Guid? Owner { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Level { get; set; }

        public bool IsEquipped { get; set; }

        public int Worth { get; set; }

        public string InSlot { get; set; }

        public ItemProperties Properties { get; set; }

        public bool IsConsumable { get; set; }

        public ItemVm(Item item)
        {
            Id = item.Id;
            Owner = item.Owner;
            Name = item.Name;
            Type = item.Type;
            Level = item.Level;
            IsEquipped = item.IsEquipped;
            Worth = item.Worth;
            InSlot = item.InSlot;
            Properties = JsonConvert.DeserializeObject<ItemProperties>(item.Properties);
            IsConsumable = item.IsConsumable;
        }

    }
}

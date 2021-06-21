using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;

namespace Avelraangame.Models
{
    public partial class Item
    {
        public Guid Id { get; set; }
        public Guid? CharacterId { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsEquipped { get; set; }
        public ItemUtils.Types Type { get; set; }

        public ItemUtils.Slots InSlot { get; set; }
        public string Slots { get; set; }

        public bool IsConsumable { get; set; }
        public int Worth { get; set; }

        public string Bonuses { get; set; }
    }
}

using System;

namespace Avelraangame.Models
{
    public partial class Item
    {
        public Guid Id { get; set; }

        public Guid? Owner { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Level { get; set; }

        public bool IsEquipped { get; set; }

        public int Worth { get; set; }

        public string InSlot { get; set; }

        public string Properties { get; set; }

        public bool IsConsumable { get; set; }
    }
}

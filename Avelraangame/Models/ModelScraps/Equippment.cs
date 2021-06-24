using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.ModelScraps
{
    public class Equippment
    {
        public Item Armour { get; set; }
        public Item Mainhand { get; set; }
        public Item Offhand { get; set; }
        public Item Ranged { get; set; }
        public List<Item> Trinkets { get; set; }
    }
}

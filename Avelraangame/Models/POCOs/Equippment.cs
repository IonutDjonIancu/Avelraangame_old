﻿using Avelraangame.Models.ViewModels;
using System.Collections.Generic;

namespace Avelraangame.Models.POCOs
{
    public class Equippment
    {
        public ItemVm Armour { get; set; }
        public ItemVm Mainhand { get; set; }
        public ItemVm Offhand { get; set; }
        public ItemVm Ranged { get; set; }
        public List<ItemVm> Trinkets { get; set; }

        public Equippment()
        {
            Trinkets = new List<ItemVm>();
        }
    }
}